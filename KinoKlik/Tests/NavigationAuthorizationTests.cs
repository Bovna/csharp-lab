using FluentAssertions;
using System.Net;
using System.Text.RegularExpressions;

namespace KinoKlik.Tests;

public sealed class NavigationAuthorizationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

    public NavigationAuthorizationTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task MainNavigation_HidesSensitiveLinks_WhenUserIsNotAuthenticated()
    {
        var response = await _factory.CreateClient().GetAsync("/");

        var html = WebUtility.HtmlDecode(await response.Content.ReadAsStringAsync());

        AssertPublicNavigation(html);
        AssertManagementNavigationIsHidden(html);
    }

    [Fact]
    public async Task MainNavigation_HidesSensitiveLinks_WhenUserHasNoRole()
    {
        var response = await _factory.CreateAuthenticatedClient().GetAsync("/");

        var html = WebUtility.HtmlDecode(await response.Content.ReadAsStringAsync());

        AssertPublicNavigation(html);
        AssertManagementNavigationIsHidden(html);
    }

    [Theory]
    [InlineData("Admin")]
    [InlineData("Manager")]
    public async Task MainNavigation_ShowsManagementDropdown_WhenUserHasManagementRole(string role)
    {
        var response = await _factory.CreateAuthenticatedClient(role).GetAsync("/");

        var html = await response.Content.ReadAsStringAsync();

        AssertPublicNavigation(html);
        html.Should().Contain("Upravljanje");
        html.Should().Contain("href=\"/dvorana");
        html.Should().Contain("href=\"/sjedala");
        html.Should().Contain("href=\"/kupci");
        html.Should().Contain("href=\"/ulaznice");
    }

    [Theory]
    [InlineData("/filmovi/pretraga", "Na programu", "Filmovi")]
    [InlineData("/projekcije/pretraga", "Raspored", "Projekcije")]
    [InlineData("/kina", "Kina", null)]
    public async Task Breadcrumb_UsesPublicLabel_ForPublicListing(
        string path,
        string expectedLabel,
        string? unexpectedLabel)
    {
        var response = await _factory.CreateClient().GetAsync(path);
        response.EnsureSuccessStatusCode();

        var breadcrumb = ExtractBreadcrumb(await response.Content.ReadAsStringAsync());

        breadcrumb.Should().Contain(">Početna<");
        breadcrumb.Should().Contain($">{expectedLabel}<");
        if (unexpectedLabel is not null)
        {
            breadcrumb.Should().NotContain($">{unexpectedLabel}<");
        }
    }

    [Theory]
    [InlineData("Admin", "/filmovi/pretraga?management=true", "Filmovi")]
    [InlineData("Manager", "/filmovi/pretraga?management=true", "Filmovi")]
    [InlineData("Admin", "/projekcije/pretraga?management=true", "Projekcije")]
    [InlineData("Manager", "/projekcije/pretraga?management=true", "Projekcije")]
    [InlineData("Admin", "/kina?management=true", "Kina")]
    [InlineData("Manager", "/kina?management=true", "Kina")]
    public async Task Breadcrumb_UsesManagementLabel_ForManagementListing(
        string role,
        string path,
        string expectedLabel)
    {
        var response = await _factory.CreateAuthenticatedClient(role).GetAsync(path);
        response.EnsureSuccessStatusCode();

        var breadcrumb = ExtractBreadcrumb(await response.Content.ReadAsStringAsync());

        breadcrumb.Should().Contain(">Početna<");
        breadcrumb.Should().Contain($">{expectedLabel}<");
    }

    [Fact]
    public async Task ManagementQuery_DoesNotExposeManagementPresentation_ToAnonymousUser()
    {
        var response = await _factory.CreateClient().GetAsync("/filmovi/pretraga?management=true");
        response.EnsureSuccessStatusCode();

        var html = WebUtility.HtmlDecode(await response.Content.ReadAsStringAsync());
        var breadcrumb = ExtractBreadcrumb(html);

        breadcrumb.Should().Contain(">Na programu<");
        breadcrumb.Should().NotContain(">Filmovi<");
        html.Should().Contain("Pronađite svoj sljedeći film");
        html.Should().NotContain("Popis filmova");
    }

    [Fact]
    public async Task Footer_ContainsOnlyCinemaTicketingCopy()
    {
        var response = await _factory.CreateClient().GetAsync("/");
        response.EnsureSuccessStatusCode();

        var html = WebUtility.HtmlDecode(await response.Content.ReadAsStringAsync());

        html.Should().Contain("© 2026 KinoKlik - Online kupnja kino ulaznica");
        html.Should().NotContain("Portfolio demo bez stvarne naplate");
        html.Should().NotContain("Izvorni kod na GitHubu");
    }

    private static void AssertPublicNavigation(string html)
    {
        html.Should().Contain("KinoKlik");
        html.Should().NotContain("Kino Sustav");
        html.Should().Contain("aria-label=\"KinoKlik — početna\"");
        html.Should().Contain("href=\"/\"");
        html.Should().Contain("Na programu");
        html.Should().Contain("Raspored");
        html.Should().Contain("Kina");
        html.Should().Contain("Kupi ulaznicu");
    }

    private static void AssertManagementNavigationIsHidden(string html)
    {
        html.Should().NotContain("Upravljanje");
        html.Should().NotContain("href=\"/dvorana");
        html.Should().NotContain("href=\"/sjedala");
        html.Should().NotContain("href=\"/kupci");
        html.Should().NotContain("href=\"/ulaznice");
    }

    private static string ExtractBreadcrumb(string html)
    {
        var match = Regex.Match(
            html,
            "<ol[^>]*class=\"breadcrumb\"[^>]*>(.*?)</ol>",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        match.Success.Should().BeTrue("the shared layout should render a breadcrumb");
        return match.Groups[1].Value;
    }
}
