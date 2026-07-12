using System.Globalization;
using System.Reflection;
using Microsoft.AspNetCore.Localization;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Models;
using KinoKlik.DAL;
using KinoKlik.Web.Identity;
using KinoKlik.Web.Options;
using KinoKlik.Web.Services;

var builder = WebApplication.CreateBuilder(args);
var applicationInsightsConnectionString =
    builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]
    ?? builder.Configuration["ApplicationInsights:ConnectionString"];

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "KinoKlik API",
        Version = "v1",
        Description = "REST API for browsing and managing KinoKlik cinema data."
    });
    options.DocInclusionPredicate((_, apiDescription) =>
        apiDescription.ActionDescriptor.EndpointMetadata
            .OfType<Microsoft.AspNetCore.Mvc.ApiControllerAttribute>()
            .Any());
});

if (!string.IsNullOrWhiteSpace(applicationInsightsConnectionString))
{
    builder.Services.AddApplicationInsightsTelemetry();
}
builder.Services.Configure<UploadStorageOptions>(builder.Configuration.GetSection("UploadStorage"));
builder.Services.AddSingleton<IUploadStorage, UploadStorage>();

var cinemaConnectionString = builder.Configuration.GetConnectionString("CinemaDbContext")
    ?? throw new InvalidOperationException("Connection string 'CinemaDbContext' is not configured.");
var cinemaConnectionStringBuilder = new SqlConnectionStringBuilder(cinemaConnectionString);

if (cinemaConnectionStringBuilder.ConnectTimeout < 90)
{
    cinemaConnectionStringBuilder.ConnectTimeout = 90;
}

builder.Services.AddDbContext<CinemaDbContext>(options =>
    options.UseSqlServer(
        cinemaConnectionStringBuilder.ConnectionString,
        sql =>
        {
            sql.MigrationsAssembly("KinoKlik.DAL");
            sql.EnableRetryOnFailure();
        }));

builder.Services
    .AddDefaultIdentity<AppUser>(options => { options.SignIn.RequireConfirmedAccount = false; })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<CinemaDbContext>();

var authenticationBuilder = builder.Services.AddAuthentication();
var googleClientId = builder.Configuration["Authentication:Google:ClientId"];
var googleClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];

if (!string.IsNullOrWhiteSpace(googleClientId) && !string.IsNullOrWhiteSpace(googleClientSecret))
{
    authenticationBuilder.AddGoogle(options =>
    {
        options.ClientId = googleClientId;
        options.ClientSecret = googleClientSecret;
    });
}

builder.Services.AddRazorPages();
builder.Services
    .AddHealthChecks()
    .AddDbContextCheck<CinemaDbContext>(
        "database",
        customTestQuery: async (dbContext, cancellationToken) =>
        {
            if (!await dbContext.Database.CanConnectAsync(cancellationToken))
            {
                return false;
            }

            if (!dbContext.Database.IsRelational())
            {
                return true;
            }

            var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync(cancellationToken);
            return !pendingMigrations.Any();
        });

var app = builder.Build();
var applicationVersion =
    typeof(Program).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion
    ?? typeof(Program).Assembly.GetName().Version?.ToString()
    ?? "unknown";
var uploadStorage = app.Services.GetRequiredService<IUploadStorage>();

app.Logger.LogInformation(
    "Application Insights telemetry enabled={ApplicationInsightsEnabled}",
    !string.IsNullOrWhiteSpace(applicationInsightsConnectionString));

try
{
    uploadStorage.EnsureRootExists();

    app.Logger.LogInformation(
        "Upload storage is ready. RequestPath={RequestPath}",
        uploadStorage.RequestPath);
}
catch (Exception ex)
{
    app.Logger.LogError(
        ex,
        "Upload storage initialization failed. RequestPath={RequestPath}",
        uploadStorage.RequestPath);
}

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();

    try
    {
        await IdentityDataSeeder.SeedAsync(
            scope.ServiceProvider,
            app.Configuration);

        app.Logger.LogInformation(
            "Development identity data seeded. Environment={EnvironmentName}",
            app.Environment.EnvironmentName);
    }
    catch (Exception ex)
    {
        app.Logger.LogError(
            ex,
            "Development identity seed failed. Environment={EnvironmentName}",
            app.Environment.EnvironmentName);
    }
}
else
{
    app.Logger.LogInformation(
        "Skipping identity seed during startup. Environment={EnvironmentName}",
        app.Environment.EnvironmentName);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (Exception ex)
    {
        app.Logger.LogError(
            ex,
            "Unhandled request exception. Method={Method}, Path={Path}, TraceIdentifier={TraceIdentifier}",
            context.Request.Method,
            context.Request.Path.Value,
            context.TraceIdentifier);

        throw;
    }

    if (context.Response.StatusCode >= StatusCodes.Status500InternalServerError)
    {
        app.Logger.LogError(
            "Request completed with server error. Method={Method}, Path={Path}, StatusCode={StatusCode}, TraceIdentifier={TraceIdentifier}",
            context.Request.Method,
            context.Request.Path.Value,
            context.Response.StatusCode,
            context.TraceIdentifier);
    }
});

app.UseHttpsRedirection();
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "KinoKlik API v1");
    options.RoutePrefix = "swagger";
});
app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(uploadStorage.RootPath),
    RequestPath = uploadStorage.RequestPath
});

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

var supportedCultures = new[]
{
    new CultureInfo("hr"),
    new CultureInfo("en-US")
};
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("hr"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();
app.MapHealthChecks("/health");
app.MapGet("/health/live", () => Results.Text("Healthy"));

app.Logger.LogInformation(
    "Application configured. Environment={EnvironmentName}, Version={Version}",
    app.Environment.EnvironmentName,
    applicationVersion);

app.Run();

public partial class Program
{
}
