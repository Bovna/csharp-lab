using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using KinoKlik.DAL;
using KinoKlik.Web.DTOs;

namespace KinoKlik.Tests;

public sealed class ScreeningApiControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public ScreeningApiControllerTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateAuthenticatedClient("Admin");
    }

    [Fact]
    public async Task GetAllScreenings_ReturnsCollection()
    {
        await _factory.ClearDatabaseAsync();
        var screening = await ApiTestData.CreateScreeningAsync(_factory);

        var response = await _client.GetAsync("/api/projekcije");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var screenings = await response.Content.ReadFromJsonAsync<List<ScreeningDTO>>();
        screenings.Should().NotBeNull();
        screenings.Should().ContainSingle(s => s.Id == screening.Id);
    }

    [Fact]
    public async Task GetAllScreenings_FiltersByDayOfWeek()
    {
        await _factory.ClearDatabaseAsync();
        var movie = await ApiTestData.CreateMovieAsync(_factory);
        var hall = await ApiTestData.CreateHallAsync(_factory);
        var mondayStart = new DateTime(2027, 3, 1, 18, 0, 0);
        var tuesdayStart = new DateTime(2027, 3, 2, 18, 0, 0);
        var mondayScreening = await ApiTestData.CreateScreeningAsync(
            _factory,
            movie.Id,
            hall.Id,
            mondayStart,
            mondayStart.AddHours(2));
        var tuesdayScreening = await ApiTestData.CreateScreeningAsync(
            _factory,
            movie.Id,
            hall.Id,
            tuesdayStart,
            tuesdayStart.AddHours(2));

        var response = await _client.GetAsync($"/api/projekcije?dayOfWeek={(int)mondayStart.DayOfWeek}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var screenings = await response.Content.ReadFromJsonAsync<List<ScreeningDTO>>();
        screenings.Should().NotBeNull();
        screenings.Should().ContainSingle(s => s.Id == mondayScreening.Id);
        screenings.Should().NotContain(s => s.Id == tuesdayScreening.Id);
    }

    [Fact]
    public async Task SearchScreenings_ReturnsMatchingResultsOnly()
    {
        await _factory.ClearDatabaseAsync();
        var matchingMovie = await ApiTestData.CreateMovieAsync(_factory, "Aurora Screening Movie");
        var nonMatchingMovie = await ApiTestData.CreateMovieAsync(_factory, "Borealis Screening Movie");
        var hall = await ApiTestData.CreateHallAsync(_factory);
        var matchingScreening = await ApiTestData.CreateScreeningAsync(
            _factory,
            matchingMovie.Id,
            hall.Id,
            new DateTime(2027, 3, 1, 18, 0, 0),
            new DateTime(2027, 3, 1, 20, 0, 0));
        var nonMatchingScreening = await ApiTestData.CreateScreeningAsync(
            _factory,
            nonMatchingMovie.Id,
            hall.Id,
            new DateTime(2027, 3, 2, 18, 0, 0),
            new DateTime(2027, 3, 2, 20, 0, 0));

        var response = await _client.GetAsync("/api/projekcije/pretraga/Aurora");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var screenings = await response.Content.ReadFromJsonAsync<List<ScreeningDTO>>();
        screenings.Should().NotBeNull();
        screenings.Should().ContainSingle(s => s.Id == matchingScreening.Id);
        screenings.Should().NotContain(s => s.Id == nonMatchingScreening.Id);
    }

    [Fact]
    public async Task GetScreeningById_ReturnsScreening_WhenScreeningExists()
    {
        await _factory.ClearDatabaseAsync();
        var screening = await ApiTestData.CreateScreeningAsync(_factory);

        var response = await _client.GetAsync($"/api/projekcije/{screening.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var dto = await response.Content.ReadFromJsonAsync<ScreeningDTO>();
        dto.Should().NotBeNull();
        dto!.Id.Should().Be(screening.Id);
    }

    [Fact]
    public async Task GetScreeningById_ReturnsNotFound_WhenScreeningDoesNotExist()
    {
        await _factory.ClearDatabaseAsync();

        var response = await _client.GetAsync("/api/projekcije/9999");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task PostScreening_CreatesScreening_AndReturnsCreated()
    {
        await _factory.ClearDatabaseAsync();
        var movie = await ApiTestData.CreateMovieAsync(_factory);
        var hall = await ApiTestData.CreateHallAsync(_factory);
        var request = CreateScreeningWriteDto(movie.Id, hall.Id);

        var response = await _client.PostAsJsonAsync("/api/projekcije", request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var dto = await response.Content.ReadFromJsonAsync<ScreeningDTO>();
        dto.Should().NotBeNull();
        dto!.Movie.Id.Should().Be(movie.Id);
        dto.Hall.Id.Should().Be(hall.Id);
    }

    [Fact]
    public async Task PostScreening_ReturnsBadRequest_WhenModelIsInvalid()
    {
        await _factory.ClearDatabaseAsync();

        var response = await _client.PostAsJsonAsync("/api/projekcije", new ScreeningWriteDTO());

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task PostScreening_ReturnsBadRequest_WhenEndTimeIsNotAfterStartTime()
    {
        await _factory.ClearDatabaseAsync();
        var movie = await ApiTestData.CreateMovieAsync(_factory);
        var hall = await ApiTestData.CreateHallAsync(_factory);
        var request = CreateScreeningWriteDto(movie.Id, hall.Id);
        request.EndTime = request.StartTime;

        var response = await _client.PostAsJsonAsync("/api/projekcije", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var error = await ReadErrorMessageAsync(response);
        error.Should().Be("Vrijeme završetka mora biti nakon vremena početka.");
    }

    [Fact]
    public async Task PostScreening_ReturnsBadRequest_When3DScreeningUsesHallWithout3DSupport()
    {
        await _factory.ClearDatabaseAsync();
        var movie = await ApiTestData.CreateMovieAsync(_factory);
        var hall = await ApiTestData.CreateHallAsync(_factory, supports3D: false);
        var request = CreateScreeningWriteDto(movie.Id, hall.Id, is3D: true);

        var response = await _client.PostAsJsonAsync("/api/projekcije", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var error = await ReadErrorMessageAsync(response);
        error.Should().Be("Odabrana dvorana ne podržava 3D projekcije.");
    }

    [Fact]
    public async Task PostScreening_ReturnsBadRequest_WhenHallHasOverlappingScreening()
    {
        await _factory.ClearDatabaseAsync();
        var movie = await ApiTestData.CreateMovieAsync(_factory);
        var hall = await ApiTestData.CreateHallAsync(_factory);
        await ApiTestData.CreateScreeningAsync(_factory, movie.Id, hall.Id);
        var request = CreateScreeningWriteDto(movie.Id, hall.Id);
        request.StartTime = new DateTime(2026, 3, 1, 19, 0, 0);
        request.EndTime = new DateTime(2026, 3, 1, 21, 0, 0);

        var response = await _client.PostAsJsonAsync("/api/projekcije", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var error = await ReadErrorMessageAsync(response);
        error.Should().Be("U odabranoj dvorani već postoji projekcija u tom terminu.");
    }

    [Fact]
    public async Task PutScreening_UpdatesExistingScreening()
    {
        await _factory.ClearDatabaseAsync();
        var screening = await ApiTestData.CreateScreeningAsync(_factory);
        var movie = await ApiTestData.CreateMovieAsync(_factory, "Updated Movie");
        var hall = await ApiTestData.CreateHallAsync(_factory, "Updated Hall");
        var request = CreateScreeningWriteDto(movie.Id, hall.Id, is3D: true);

        var response = await _client.PutAsJsonAsync($"/api/projekcije/{screening.Id}", request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var dto = await response.Content.ReadFromJsonAsync<ScreeningDTO>();
        dto.Should().NotBeNull();
        dto!.Movie.Id.Should().Be(movie.Id);
        dto.Hall.Id.Should().Be(hall.Id);
        dto.Is3D.Should().BeTrue();
    }

    [Fact]
    public async Task PutScreening_ReturnsNotFound_WhenScreeningDoesNotExist()
    {
        await _factory.ClearDatabaseAsync();
        var movie = await ApiTestData.CreateMovieAsync(_factory);
        var hall = await ApiTestData.CreateHallAsync(_factory);
        var request = CreateScreeningWriteDto(movie.Id, hall.Id);

        var response = await _client.PutAsJsonAsync("/api/projekcije/9999", request);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task PutScreening_ReturnsBadRequest_WhenEndTimeIsNotAfterStartTime()
    {
        await _factory.ClearDatabaseAsync();
        var screening = await ApiTestData.CreateScreeningAsync(_factory);
        var request = CreateScreeningWriteDto(screening.MovieId, screening.HallId);
        request.EndTime = request.StartTime;

        var response = await _client.PutAsJsonAsync($"/api/projekcije/{screening.Id}", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var error = await ReadErrorMessageAsync(response);
        error.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task DeleteScreening_SoftDeletesExistingScreening()
    {
        await _factory.ClearDatabaseAsync();
        var screening = await ApiTestData.CreateScreeningAsync(_factory);

        var response = await _client.DeleteAsync($"/api/projekcije/{screening.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<CinemaDbContext>();
        var deleted = await dbContext.Screenings.FindAsync(screening.Id);
        deleted!.DeletedAt.Should().NotBeNull();
    }

    [Fact]
    public async Task DeleteScreening_ReturnsNotFound_WhenScreeningDoesNotExist()
    {
        await _factory.ClearDatabaseAsync();

        var response = await _client.DeleteAsync("/api/projekcije/9999");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task ScreeningApiEndpoint_ReturnsUnauthorized_WhenUserIsNotAuthenticated()
    {
        var response = await _factory.CreateClient().GetAsync("/api/projekcije/9999");

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    private static ScreeningWriteDTO CreateScreeningWriteDto(int movieId, int hallId, bool is3D = false)
    {
        return new ScreeningWriteDTO
        {
            StartTime = new DateTime(2026, 3, 2, 18, 0, 0),
            EndTime = new DateTime(2026, 3, 2, 20, 0, 0),
            Is3D = is3D,
            MovieId = movieId,
            HallId = hallId
        };
    }

    private static async Task<string> ReadErrorMessageAsync(HttpResponseMessage response)
    {
        var error = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
        return error?["error"] ?? string.Empty;
    }
}
