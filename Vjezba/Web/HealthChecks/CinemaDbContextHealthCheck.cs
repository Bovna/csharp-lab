using Microsoft.Extensions.Diagnostics.HealthChecks;
using Vjezba.DAL;

namespace Vjezba.Web.HealthChecks;

public sealed class CinemaDbContextHealthCheck : IHealthCheck
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<CinemaDbContextHealthCheck> _logger;

    public CinemaDbContextHealthCheck(
        IServiceScopeFactory serviceScopeFactory,
        ILogger<CinemaDbContextHealthCheck> logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<CinemaDbContext>();

            if (await dbContext.Database.CanConnectAsync(cancellationToken))
            {
                return HealthCheckResult.Healthy("Database connection is available.");
            }

            _logger.LogWarning("Database health check failed because database connection is not available.");
            return HealthCheckResult.Unhealthy("Database connection is not available.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Database health check failed with exception.");
            return HealthCheckResult.Unhealthy("Database health check failed.", ex);
        }
    }
}
