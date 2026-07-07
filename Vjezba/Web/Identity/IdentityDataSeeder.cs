using Microsoft.AspNetCore.Identity;

namespace Vjezba.Web.Identity;

public static class IdentityDataSeeder
{
    private const string AdminRole = "Admin";
    private const string ManagerRole = "Manager";

    public static async Task SeedAsync(IServiceProvider serviceProvider, bool seedDemoUsers)
    {
        await SeedRoles(serviceProvider);

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

        await EnsureDemoUser(
            userManager,
            email: "admin@example.com",
            password: "Admin123!",
            oib: "11111111111",
            jmbag: "1111111111111",
            role: AdminRole);

        await EnsureDemoUser(
            userManager,
            email: "manager@example.com",
            password: "Manager123!",
            oib: "22222222222",
            jmbag: "2222222222222",
            role: ManagerRole);
    }

    private static async Task EnsureDemoUser(
        UserManager<AppUser> userManager,
        string email,
        string password,
        string oib,
        string jmbag,
        string role)
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
                    $"Could not create demo user '{email}': {string.Join(", ", createResult.Errors.Select(e => e.Description))}");
            }
        }

        if (!await userManager.IsInRoleAsync(user, role))
        {
            var roleResult = await userManager.AddToRoleAsync(user, role);
            if (!roleResult.Succeeded)
            {
                throw new InvalidOperationException(
                    $"Could not add demo user '{email}' to role '{role}': {string.Join(", ", roleResult.Errors.Select(e => e.Description))}");
            }
        }
    }
}
