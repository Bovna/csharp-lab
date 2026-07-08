using System.Globalization;
using Microsoft.Extensions.Options;
using Vjezba.Web.Options;

namespace Vjezba.Web.Services;

public sealed class UploadStorage : IUploadStorage
{
    public UploadStorage(IOptions<UploadStorageOptions> options, IWebHostEnvironment webHostEnvironment)
    {
        RequestPath = NormalizeRequestPath(options.Value.RequestPath);
        RootPath = ResolveRootPath(options.Value.RootPath, webHostEnvironment);
    }

    public string RootPath { get; }
    public string RequestPath { get; }

    public void EnsureRootExists()
    {
        Directory.CreateDirectory(RootPath);
    }

    public string GetMovieDirectory(int movieId)
    {
        return Path.Combine(RootPath, "movies", movieId.ToString(CultureInfo.InvariantCulture));
    }

    public string GetMoviePublicPath(int movieId, string fileName)
    {
        return $"{RequestPath}/movies/{movieId.ToString(CultureInfo.InvariantCulture)}/{fileName}";
    }

    public string GetPhysicalPath(string publicPath)
    {
        var normalizedPublicPath = "/" + publicPath.TrimStart('/').Replace('\\', '/');
        var normalizedRequestPath = RequestPath.TrimEnd('/');

        if (!normalizedPublicPath.StartsWith($"{normalizedRequestPath}/", StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException($"Upload path '{publicPath}' is outside configured upload request path '{RequestPath}'.");
        }

        var relativePath = normalizedPublicPath[(normalizedRequestPath.Length + 1)..]
            .Replace('/', Path.DirectorySeparatorChar);

        return Path.Combine(RootPath, relativePath);
    }

    private static string ResolveRootPath(string? configuredRootPath, IWebHostEnvironment webHostEnvironment)
    {
        if (!string.IsNullOrWhiteSpace(configuredRootPath))
        {
            return Path.GetFullPath(Path.IsPathRooted(configuredRootPath)
                ? configuredRootPath
                : Path.Combine(webHostEnvironment.ContentRootPath, configuredRootPath));
        }

        var webRootPath = webHostEnvironment.WebRootPath
            ?? Path.Combine(webHostEnvironment.ContentRootPath, "wwwroot");

        return Path.GetFullPath(Path.Combine(webRootPath, "uploads"));
    }

    private static string NormalizeRequestPath(string requestPath)
    {
        var normalizedPath = "/" + (string.IsNullOrWhiteSpace(requestPath) ? "uploads" : requestPath).Trim('/');
        return normalizedPath.Equals("/", StringComparison.Ordinal) ? "/uploads" : normalizedPath;
    }
}
