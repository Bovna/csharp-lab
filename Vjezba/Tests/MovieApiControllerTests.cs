using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Vjezba.DAL;
using Vjezba.Model.Entities;
using Vjezba.Web.DTOs;

namespace Vjezba.Tests;

public sealed class MovieApiControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public MovieApiControllerTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateAuthenticatedClient("Admin");
    }

    [Fact]
    public async Task GetMovieById_ReturnsMovie_WhenMovieExists()
    {
        await _factory.ClearDatabaseAsync();
        var movie = await CreateTestMovieAsync(title: "Existing Movie");

        var response = await _client.GetAsync($"/api/film/{movie.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var dto = await response.Content.ReadFromJsonAsync<MovieDTO>();

        dto.Should().NotBeNull();
        dto!.Id.Should().Be(movie.Id);
        dto.Title.Should().Be(movie.Title);
        dto.Description.Should().Be(movie.Description);
        dto.DurationMinutes.Should().Be(movie.DurationMinutes);
        dto.ReleaseDate.Should().Be(movie.ReleaseDate);
        dto.Genre.Should().Be(movie.Genre);
        dto.Language.Should().Be(movie.Language);
        dto.AgeRating.Should().Be(movie.AgeRating);
    }

    [Fact]
    public async Task GetMovieById_ReturnsNotFound_WhenMovieDoesNotExist()
    {
        await _factory.ClearDatabaseAsync();
        var nonExistentId = 9999;

        var response = await _client.GetAsync($"/api/film/{nonExistentId}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetAllMovies_ReturnsListOfMovies()
    {
        await _factory.ClearDatabaseAsync();
        var movie1 = await CreateTestMovieAsync(title: "First Test Movie", language: "EN");
        var movie2 = await CreateTestMovieAsync(title: "Second Test Movie", language: "HR");

        var response = await _client.GetAsync("/api/film");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var movies = await response.Content.ReadFromJsonAsync<List<MovieDTO>>();

        movies.Should().NotBeNull();
        movies.Should().HaveCount(2);
        movies.Should().Contain(m => m.Id == movie1.Id && m.Title == movie1.Title);
        movies.Should().Contain(m => m.Id == movie2.Id && m.Title == movie2.Title);
    }

    [Fact]
    public async Task PostMovie_CreatesMovie_AndReturnsCreated()
    {
        await _factory.ClearDatabaseAsync();
        var request = CreateMovieWriteDto(title: "Created Movie");

        var response = await _client.PostAsJsonAsync("/api/film", request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Headers.Location.Should().NotBeNull();

        var createdMovie = await response.Content.ReadFromJsonAsync<MovieDTO>();

        createdMovie.Should().NotBeNull();
        createdMovie!.Id.Should().BeGreaterThan(0);
        createdMovie.Title.Should().Be(request.Title);
        createdMovie.Description.Should().Be(request.Description);

        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<CinemaDbContext>();
        dbContext.Movies.Should().Contain(movie =>
            movie.Id == createdMovie.Id &&
            movie.Title == request.Title &&
            movie.DeletedAt == null);
    }

    [Fact]
    public async Task PostMovie_ReturnsBadRequest_WhenModelIsInvalid()
    {
        await _factory.ClearDatabaseAsync();
        var request = CreateInvalidMovieWriteDto();

        var response = await _client.PostAsJsonAsync("/api/film", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task PostMovie_ReturnsBadRequest_WhenAgeRatingIsNotAllowed()
    {
        await _factory.ClearDatabaseAsync();
        var request = CreateMovieWriteDto(title: "Invalid Age", ageRating: "R");

        var response = await _client.PostAsJsonAsync("/api/film", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var error = await ApiErrorTestHelper.ReadErrorMessageAsync(response);
        error.Should().Be("Dobna oznaka nije ispravna. Dopuštene vrijednosti su U, 7+, 10+, 12+, 15+, 16+, 18+ ili format PG-13.");
    }

    [Fact]
    public async Task PostMovie_CreatesMovie_WhenAgeRatingUsesPgFormat()
    {
        await _factory.ClearDatabaseAsync();
        var request = CreateMovieWriteDto(title: "PG Movie", ageRating: "PG-13");

        var response = await _client.PostAsJsonAsync("/api/film", request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task PostMovie_ReturnsBadRequest_WhenLanguageIsNotTwoUppercaseLetters()
    {
        await _factory.ClearDatabaseAsync();
        var request = CreateMovieWriteDto(title: "Invalid Language", language: "HRV");

        var response = await _client.PostAsJsonAsync("/api/film", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task PutMovie_UpdatesExistingMovie()
    {
        await _factory.ClearDatabaseAsync();
        var movie = await CreateTestMovieAsync(title: "Old Movie");
        var request = CreateMovieWriteDto(title: "Updated Movie", language: "HR");

        var response = await _client.PutAsJsonAsync($"/api/film/{movie.Id}", request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var updatedMovie = await response.Content.ReadFromJsonAsync<MovieDTO>();

        updatedMovie.Should().NotBeNull();
        updatedMovie!.Id.Should().Be(movie.Id);
        updatedMovie.Title.Should().Be(request.Title);
        updatedMovie.Description.Should().Be(request.Description);
        updatedMovie.Language.Should().Be(request.Language);

        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<CinemaDbContext>();
        var movieFromDatabase = await dbContext.Movies.FindAsync(movie.Id);

        movieFromDatabase.Should().NotBeNull();
        movieFromDatabase!.Title.Should().Be(request.Title);
        movieFromDatabase.Language.Should().Be(request.Language);
    }

    [Fact]
    public async Task PutMovie_ReturnsNotFound_WhenMovieDoesNotExist()
    {
        await _factory.ClearDatabaseAsync();
        var request = CreateMovieWriteDto(title: "Updated Missing Movie");

        var response = await _client.PutAsJsonAsync("/api/film/9999", request);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteMovie_SoftDeletesExistingMovie()
    {
        await _factory.ClearDatabaseAsync();
        var movie = await CreateTestMovieAsync(title: "Movie To Delete");

        var response = await _client.DeleteAsync($"/api/film/{movie.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var getResponse = await _client.GetAsync($"/api/film/{movie.Id}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);

        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<CinemaDbContext>();
        var movieFromDatabase = await dbContext.Movies.FindAsync(movie.Id);

        movieFromDatabase.Should().NotBeNull();
        movieFromDatabase!.DeletedAt.Should().NotBeNull();
    }

    [Fact]
    public async Task DeleteMovie_ReturnsNotFound_WhenMovieDoesNotExist()
    {
        await _factory.ClearDatabaseAsync();

        var response = await _client.DeleteAsync("/api/film/9999");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task MovieApiEndpoint_ReturnsUnauthorized_WhenUserIsNotAuthenticated()
    {
        var anonymousClient = _factory.CreateClient();

        var response = await anonymousClient.GetAsync("/api/film/9999");

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    private async Task<Movie> CreateTestMovieAsync(
        string title,
        string description = "A movie created for testing purposes.",
        int durationMinutes = 120,
        string language = "EN",
        string ageRating = "PG-13")
    {
        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<CinemaDbContext>();

        var movie = new Movie
        {
            Title = title,
            Description = description,
            DurationMinutes = durationMinutes,
            ReleaseDate = new DateTime(2025, 1, 1),
            Genre = MovieGenre.Action,
            Language = language,
            AgeRating = ageRating
        };

        dbContext.Movies.Add(movie);
        await dbContext.SaveChangesAsync();
        return movie;
    }

    private static MovieWriteDTO CreateMovieWriteDto(
        string title,
        string description = "A valid movie description for integration testing.",
        int durationMinutes = 130,
        string language = "EN",
        string ageRating = "12+")
    {
        return new MovieWriteDTO
        {
            Title = title,
            Description = description,
            DurationMinutes = durationMinutes,
            ReleaseDate = new DateTime(2026, 2, 3),
            Genre = MovieGenre.Drama,
            Language = language,
            AgeRating = ageRating
        };
    }

    private static MovieWriteDTO CreateInvalidMovieWriteDto()
    {
        return new MovieWriteDTO
        {
            Title = string.Empty,
            Description = "Too short",
            DurationMinutes = 0,
            ReleaseDate = new DateTime(2026, 2, 3),
            Genre = MovieGenre.Drama,
            Language = "E",
            AgeRating = string.Empty
        };
    }
}
