using System.Net;
using System.Net.Http.Headers;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Vjezba.DAL;

namespace Vjezba.Tests;

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
        using var multipart = new MultipartFormDataContent();
        using var fileContent = new ByteArrayContent(content);
        fileContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);
        multipart.Add(fileContent, "file", fileName);

        return await _client.PostAsync($"/filmovi/uredi/{movieId}/datoteke/objavi", multipart);
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
        var webHostEnvironment = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();
        var webRootPath = webHostEnvironment.WebRootPath
            ?? Path.Combine(webHostEnvironment.ContentRootPath, "wwwroot");
        var physicalPath = Path.Combine(
            webRootPath,
            filePath.TrimStart('/'));

        if (File.Exists(physicalPath))
        {
            File.Delete(physicalPath);
        }
    }
}
