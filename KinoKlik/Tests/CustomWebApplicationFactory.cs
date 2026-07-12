using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using KinoKlik.DAL;

namespace KinoKlik.Tests;

public sealed class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly string _databaseName = $"CinemaTestDb-{Guid.NewGuid()}";
    private readonly string _uploadRootPath = Path.Combine(Path.GetTempPath(), $"KinoKlikTestUploads-{Guid.NewGuid():N}");

    public HttpClient CreateAuthenticatedClient(params string[] roles)
    {
        var client = CreateClient();
        client.DefaultRequestHeaders.Add(TestAuthHandler.UserIdHeader, "test-user");

        if (roles.Length > 0)
        {
            client.DefaultRequestHeaders.Add(TestAuthHandler.RolesHeader, roles);
        }

        return client;
    }

    public async Task ClearDatabaseAsync()
    {
        using var scope = Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<CinemaDbContext>();

        dbContext.Tickets.RemoveRange(dbContext.Tickets);
        dbContext.CustomerFavoriteMovies.RemoveRange(dbContext.CustomerFavoriteMovies);
        dbContext.Screenings.RemoveRange(dbContext.Screenings);
        dbContext.Seats.RemoveRange(dbContext.Seats);
        dbContext.Attachments.RemoveRange(dbContext.Attachments);
        dbContext.Halls.RemoveRange(dbContext.Halls);
        dbContext.Cinemas.RemoveRange(dbContext.Cinemas);
        dbContext.Customers.RemoveRange(dbContext.Customers);
        dbContext.Movies.RemoveRange(dbContext.Movies);

        dbContext.UserClaims.RemoveRange(dbContext.UserClaims);
        dbContext.UserLogins.RemoveRange(dbContext.UserLogins);
        dbContext.UserRoles.RemoveRange(dbContext.UserRoles);
        dbContext.UserTokens.RemoveRange(dbContext.UserTokens);
        dbContext.RoleClaims.RemoveRange(dbContext.RoleClaims);
        dbContext.Roles.RemoveRange(dbContext.Roles);
        dbContext.Users.RemoveRange(dbContext.Users);

        await dbContext.SaveChangesAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<CinemaDbContext>));

            if (descriptor is not null)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<CinemaDbContext>(options =>
            {
                options.UseInMemoryDatabase(_databaseName);
            });

            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = TestAuthHandler.AuthenticationScheme;
                    options.DefaultChallengeScheme = TestAuthHandler.AuthenticationScheme;
                })
                .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                    TestAuthHandler.AuthenticationScheme,
                    options => { });

            using var serviceProvider = services.BuildServiceProvider();
            using var scope = serviceProvider.CreateScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<CinemaDbContext>();

            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();
        });

        builder.ConfigureAppConfiguration((context, config) =>
        {
            var testSettings = new Dictionary<string, string?>
            {
                ["ConnectionStrings:DefaultConnection"] = "TestConnection",
                ["Authentication:Google:ClientId"] = "test-client-id",
                ["Authentication:Google:ClientSecret"] = "test-client-secret",
                ["UploadStorage:RootPath"] = _uploadRootPath
            };

            config.AddInMemoryCollection(testSettings);
        });
    }

}
