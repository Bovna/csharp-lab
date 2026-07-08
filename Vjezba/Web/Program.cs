using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.FileProviders;
using Vjezba.DAL;
using Vjezba.Web.HealthChecks;
using Vjezba.Web.Identity;
using Vjezba.Web.Options;
using Vjezba.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.Configure<UploadStorageOptions>(builder.Configuration.GetSection("UploadStorage"));
builder.Services.AddSingleton<IUploadStorage, UploadStorage>();

builder.Services.AddDbContext<CinemaDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("CinemaDbContext"),
        sql => sql.MigrationsAssembly("Vjezba.DAL")));

builder.Services
    .AddDefaultIdentity<AppUser>(options => { options.SignIn.RequireConfirmedAccount = false; })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<CinemaDbContext>();

var authenticationBuilder = builder.Services.AddAuthentication();
var googleClientId = builder.Configuration["Authentication:Google:ClientId"];
var googleClientSecret = ***REMOVED***"Authentication:Google:ClientSecret"];

if (!string.IsNullOrWhiteSpace(googleClientId) && !string.IsNullOrWhiteSpace(googleClientSecret))
{
    authenticationBuilder.AddGoogle(options =>
    {
        options.ClientId = googleClientId;
        options.ClientSecret = ***REMOVED***;
    });
}

builder.Services.AddRazorPages();
builder.Services
    .AddHealthChecks()
    .AddCheck<CinemaDbContextHealthCheck>("database")
    .AddCheck<UploadStorageHealthCheck>("upload_storage");

var app = builder.Build();
var uploadStorage = app.Services.GetRequiredService<IUploadStorage>();
uploadStorage.EnsureRootExists();

using (var scope = app.Services.CreateScope())
{
    await IdentityDataSeeder.SeedAsync(
        scope.ServiceProvider,
        app.Configuration,
        seedDemoUsers: app.Environment.IsDevelopment());
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
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

app.Run();

public partial class Program
{
}
