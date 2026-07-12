using FluentAssertions;

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

        var html = await response.Content.ReadAsStringAsync();

        AssertPublicNavigation(html);
        AssertManagementNavigationIsHidden(html);
    }

    [Fact]
    public async Task MainNavigation_HidesSensitiveLinks_WhenUserHasNoRole()
    {
        var response = await _factory.CreateAuthenticatedClient().GetAsync("/");

        var html = await response.Content.ReadAsStringAsync();

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
}
