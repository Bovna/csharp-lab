using FluentAssertions;

namespace Vjezba.Tests;

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

        html.Should().NotContain("href=\"/kupci");
        html.Should().NotContain("href=\"/ulaznice");
    }

    [Fact]
    public async Task MainNavigation_HidesSensitiveLinks_WhenUserHasNoRole()
    {
        var response = await _factory.CreateAuthenticatedClient().GetAsync("/");

        var html = await response.Content.ReadAsStringAsync();

        html.Should().NotContain("href=\"/kupci");
        html.Should().NotContain("href=\"/ulaznice");
    }

    [Fact]
    public async Task MainNavigation_ShowsSensitiveLinks_WhenUserIsManager()
    {
        var response = await _factory.CreateAuthenticatedClient("Manager").GetAsync("/");

        var html = await response.Content.ReadAsStringAsync();

        html.Should().Contain("href=\"/kupci");
        html.Should().Contain("href=\"/ulaznice");
    }
}
