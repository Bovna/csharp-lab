using System.Net;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using KinoKlik.DAL;
using KinoKlik.Web.Services;

namespace KinoKlik.Tests;

public sealed class MovieAttachmentUploadTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public MovieAttachmentUploadTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateAuthenticatedClient("Manager");
    }

    [Fact]
    public async Task UploadAttachment_CreatesAttachment_WhenPosterImageIsValid()
    {
        await _factory.ClearDatabaseAsync();
        var movie = await ApiTestData.CreateMovieAsync(_factory);

        var response = await UploadFileAsync(movie.Id, "C:\\fakepath\\poster.JPG", "image/jpeg", [1, 2, 3]);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<CinemaDbContext>();
        var attachment = await dbContext.Attachments.SingleAsync();

        attachment.MovieId.Should().Be(movie.Id);
        attachment.FileName.Should().Be("poster.JPG");
        attachment.ContentType.Should().Be("image/jpeg");
        attachment.FileSize.Should().Be(3);
        attachment.FilePath.Should().StartWith($"/uploads/movies/{movie.Id}/");
        attachment.FilePath.Should().EndWith(".jpg");
        attachment.FilePath.Should().NotContain("poster");

        DeleteUploadedFile(attachment.FilePath);
    }

    [Fact]
    public async Task UploadAttachment_ReturnsBadRequest_WhenExtensionIsNotAllowed()
    {
        await _factory.ClearDatabaseAsync();
        var movie = await ApiTestData.CreateMovieAsync(_factory);

        var response = await UploadFileAsync(movie.Id, "poster.txt", "text/plain", [1, 2, 3]);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        await AssertNoAttachmentWasSavedAsync();
    }

    [Fact]
    public async Task UploadAttachment_ReturnsBadRequest_WhenContentTypeDoesNotMatchExtension()
    {
        await _factory.ClearDatabaseAsync();
        var movie = await ApiTestData.CreateMovieAsync(_factory);

        var response = await UploadFileAsync(movie.Id, "poster.jpg", "text/plain", [1, 2, 3]);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        await AssertNoAttachmentWasSavedAsync();
    }

    [Fact]
    public async Task UploadAttachment_ReturnsBadRequest_WhenFileIsTooLarge()
    {
        await _factory.ClearDatabaseAsync();
        var movie = await ApiTestData.CreateMovieAsync(_factory);
        var oversizedFile = new byte[(5 * 1024 * 1024) + 1];

        var response = await UploadFileAsync(movie.Id, "poster.png", "image/png", oversizedFile);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        await AssertNoAttachmentWasSavedAsync();
    }

    private async Task<HttpResponseMessage> UploadFileAsync(
        int movieId,
        string fileName,
        string contentType,
        byte[] content)
    {
        var token = await GetAntiForgeryTokenAsync(movieId);
        using var multipart = new MultipartFormDataContent();
        using var fileContent = new ByteArrayContent(content);
        fileContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);
        multipart.Add(new StringContent(token), "__RequestVerificationToken");
        multipart.Add(fileContent, "file", fileName);

        return await _client.PostAsync($"/filmovi/uredi/{movieId}/datoteke/objavi", multipart);
    }

    private async Task<string> GetAntiForgeryTokenAsync(int movieId)
    {
        var response = await _client.GetAsync($"/filmovi/uredi/{movieId}");
        response.EnsureSuccessStatusCode();

        var html = await response.Content.ReadAsStringAsync();
        var match = Regex.Match(
            html,
            "<input[^>]+name=\"__RequestVerificationToken\"[^>]+value=\"([^\"]+)\"",
            RegexOptions.IgnoreCase);

        match.Success.Should().BeTrue("the movie edit form should render an antiforgery token");
        return match.Groups[1].Value;
    }

    private async Task AssertNoAttachmentWasSavedAsync()
    {
        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<CinemaDbContext>();

        (await dbContext.Attachments.AnyAsync()).Should().BeFalse();
    }

    private void DeleteUploadedFile(string filePath)
    {
        using var scope = _factory.Services.CreateScope();
        var uploadStorage = scope.ServiceProvider.GetRequiredService<IUploadStorage>();
        var physicalPath = uploadStorage.GetPhysicalPath(filePath);

        if (File.Exists(physicalPath))
        {
            File.Delete(physicalPath);
        }
    }
}
