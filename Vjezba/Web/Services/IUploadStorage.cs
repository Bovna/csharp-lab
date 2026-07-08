namespace Vjezba.Web.Services;

public interface IUploadStorage
{
    string RootPath { get; }
    string RequestPath { get; }

    void EnsureRootExists();
    string GetMovieDirectory(int movieId);
    string GetMoviePublicPath(int movieId, string fileName);
    string GetPhysicalPath(string publicPath);
}
