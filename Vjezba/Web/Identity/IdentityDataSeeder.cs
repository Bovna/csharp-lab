using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace Vjezba.Web.Identity;

public static class IdentityDataSeeder
{
    private const string AdminRole = "Admin";
    private const string ManagerRole = "Manager";

    public static async Task SeedAsync(
        IServiceProvider serviceProvider,
        IConfiguration configuration,
        bool seedDemoUsers)
    {
        await SeedRoles(serviceProvider);
        await SeedConfiguredUser(serviceProvider, configuration.GetSection("SeedUsers:Admin"), AdminRole);
        await SeedConfiguredUser(serviceProvider, configuration.GetSection("SeedUsers:Manager"), ManagerRole);

        if (seedDemoUsers)
        {
            await SeedDemoUsers(serviceProvider);
        }
    }

    private static async Task SeedRoles(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        string[] roles = { AdminRole, ManagerRole };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }

    private static async Task SeedDemoUsers(IServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();

        await EnsureUser(
            userManager,
            email: "admin@example.com",
            password: "Admin123!",
            oib: "11111111111",
            jmbag: "1111111111111",
            role: AdminRole,
            userSource: "demo");

        await EnsureUser(
            userManager,
            email: "manager@example.com",
            password: "Manager123!",
            oib: "22222222222",
            jmbag: "2222222222222",
            role: ManagerRole,
            userSource: "demo");
    }

    private static async Task SeedConfiguredUser(
        IServiceProvider serviceProvider,
        IConfigurationSection userSection,
        string role)
    {
        var email = userSection["Email"]?.Trim();

        if (string.IsNullOrWhiteSpace(email))
        {
            return;
        }

        var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
        var user = await userManager.FindByEmailAsync(email);

        if (user is null)
        {
            await EnsureUser(
                userManager,
                email,
                password: GetRequiredSeedValue(userSection, "Password", role),
                oib: GetRequiredSeedValue(userSection, "OIB", role),
                jmbag: GetRequiredSeedValue(userSection, "JMBAG", role),
                role,
                userSource: "configured");

            return;
        }

        await EnsureUserRole(userManager, user, role, "configured");
    }

    private static async Task EnsureUser(
        UserManager<AppUser> userManager,
        string email,
        string password,
        string oib,
        string jmbag,
        string role,
        string userSource)
    {
        var user = await userManager.FindByEmailAsync(email);

        if (user == null)
        {
            user = new AppUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true,
                OIB = oib,
                JMBAG = jmbag
            };

            var createResult = await userManager.CreateAsync(user, password);
            if (!createResult.Succeeded)
            {
                throw new InvalidOperationException(
                    $"Could not create {userSource} user '{email}': {string.Join(", ", createResult.Errors.Select(e => e.Description))}");
            }
        }

        await EnsureUserRole(userManager, user, role, userSource);
    }

    private static async Task EnsureUserRole(
        UserManager<AppUser> userManager,
        AppUser user,
        string role,
        string userSource)
    {
        if (!await userManager.IsInRoleAsync(user, role))
        {
            var roleResult = await userManager.AddToRoleAsync(user, role);
            if (!roleResult.Succeeded)
            {
                throw new InvalidOperationException(
                    $"Could not add {userSource} user '{user.Email}' to role '{role}': {string.Join(", ", roleResult.Errors.Select(e => e.Description))}");
            }
        }
    }

    private static string GetRequiredSeedValue(IConfigurationSection section, string key, string role)
    {
        var value = section[key]?.Trim();

        if (string.IsNullOrWhiteSpace(value))
        {
            throw new InvalidOperationException(
                $"SeedUsers:{role}:{key} must be configured when SeedUsers:{role}:Email creates a new user.");
        }

        return value;
    }
}
