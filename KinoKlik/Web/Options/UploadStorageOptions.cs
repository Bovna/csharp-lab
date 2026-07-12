namespace KinoKlik.Web.Options;

public sealed class UploadStorageOptions
{
    public string? RootPath { get; set; }
    public string RequestPath { get; set; } = "/uploads";
}
