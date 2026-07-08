using Microsoft.Extensions.Diagnostics.HealthChecks;
using Vjezba.Web.Services;

namespace Vjezba.Web.HealthChecks;

public sealed class UploadStorageHealthCheck : IHealthCheck
{
    private readonly IUploadStorage _uploadStorage;

    public UploadStorageHealthCheck(IUploadStorage uploadStorage)
    {
        _uploadStorage = uploadStorage;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        var probePath = Path.Combine(_uploadStorage.RootPath, $".health-{Guid.NewGuid():N}.tmp");

        try
        {
            _uploadStorage.EnsureRootExists();
            await File.WriteAllTextAsync(probePath, "ok", cancellationToken);
            File.Delete(probePath);

            return HealthCheckResult.Healthy("Upload storage is writable.");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("Upload storage is not writable.", ex);
        }
    }
}
