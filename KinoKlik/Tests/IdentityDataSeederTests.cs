using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using KinoKlik.Web.Identity;

namespace KinoKlik.Tests;

public sealed class IdentityDataSeederTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

    public IdentityDataSeederTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task SeedAsync_CreatesConfiguredUserWithoutOptionalIdentifiers()
    {
        await _factory.ClearDatabaseAsync();
        var configuration = CreateAdminConfiguration();

        await SeedAsync(configuration);

        using var scope = _factory.Services.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
        var user = await userManager.FindByEmailAsync("seed-admin@example.com");

        user.Should().NotBeNull();
        user!.OIB.Should().BeEmpty();
        user.JMBAG.Should().BeEmpty();
        (await userManager.IsInRoleAsync(user, "Admin")).Should().BeTrue();
    }

    [Fact]
    public async Task SeedAsync_DoesNotOverwriteExistingIdentifiersWhenTheyAreOmittedLater()
    {
        await _factory.ClearDatabaseAsync();
        var initialConfiguration = CreateAdminConfiguration(
            oib: "12345678901",
            jmbag: "1234567890123");

        await SeedAsync(initialConfiguration);
        await SeedAsync(CreateAdminConfiguration());

        using var scope = _factory.Services.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
        var user = await userManager.FindByEmailAsync("seed-admin@example.com");

        user.Should().NotBeNull();
        user!.OIB.Should().Be("12345678901");
        user.JMBAG.Should().Be("1234567890123");
    }

    private static IConfiguration CreateAdminConfiguration(string? oib = null, string? jmbag = null)
    {
        var values = new Dictionary<string, string?>
        {
            ["SeedUsers:Admin:Email"] = "seed-admin@example.com",
            ["SeedUsers:Admin:Password"] = "Test-Seed-9!Password",
            ["SeedUsers:Admin:OIB"] = oib,
            ["SeedUsers:Admin:JMBAG"] = jmbag
        };

        return new ConfigurationBuilder()
            .AddInMemoryCollection(values)
            .Build();
    }

    private async Task SeedAsync(IConfiguration configuration)
    {
        using var scope = _factory.Services.CreateScope();
        await IdentityDataSeeder.SeedAsync(scope.ServiceProvider, configuration);
    }
}
