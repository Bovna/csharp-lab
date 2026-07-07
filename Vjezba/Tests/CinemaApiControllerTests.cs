using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Vjezba.DAL;
using Vjezba.Web.DTOs;

namespace Vjezba.Tests;

public sealed class CinemaApiControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public CinemaApiControllerTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateAuthenticatedClient("Admin");
    }

    [Fact]
    public async Task GetAllCinemas_ReturnsCollection()
    {
        await _factory.ClearDatabaseAsync();
        var cinema = await ApiTestData.CreateCinemaAsync(_factory, name: "Cinema One");

        var response = await _client.GetAsync("/api/kina");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var cinemas = await response.Content.ReadFromJsonAsync<List<CinemaDTO>>();
        cinemas.Should().NotBeNull();
        cinemas.Should().ContainSingle(c => c.Id == cinema.Id && c.Name == cinema.Name);
    }

    [Fact]
    public async Task GetCinemaById_ReturnsCinema_WhenCinemaExists()
    {
        await _factory.ClearDatabaseAsync();
        var cinema = await ApiTestData.CreateCinemaAsync(_factory, name: "Existing Cinema");

        var response = await _client.GetAsync($"/api/kina/{cinema.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var dto = await response.Content.ReadFromJsonAsync<CinemaDTO>();
        dto.Should().NotBeNull();
        dto!.Id.Should().Be(cinema.Id);
        dto.Name.Should().Be(cinema.Name);
    }

    [Fact]
    public async Task GetCinemaById_ReturnsNotFound_WhenCinemaDoesNotExist()
    {
        await _factory.ClearDatabaseAsync();

        var response = await _client.GetAsync("/api/kina/9999");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task PostCinema_CreatesCinema_AndReturnsCreated()
    {
        await _factory.ClearDatabaseAsync();
        var request = CreateCinemaWriteDto("Created Cinema");

        var response = await _client.PostAsJsonAsync("/api/kina", request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var dto = await response.Content.ReadFromJsonAsync<CinemaDTO>();
        dto.Should().NotBeNull();
        dto!.Name.Should().Be(request.Name);
    }

    [Fact]
    public async Task PostCinema_ReturnsBadRequest_WhenModelIsInvalid()
    {
        await _factory.ClearDatabaseAsync();

        var response = await _client.PostAsJsonAsync("/api/kina", new CinemaWriteDTO());

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task PostCinema_ReturnsBadRequest_WhenEmailAlreadyExists()
    {
        await _factory.ClearDatabaseAsync();
        await ApiTestData.CreateCinemaAsync(_factory);
        var request = CreateCinemaWriteDto("Other Cinema");

        var response = await _client.PostAsJsonAsync("/api/kina", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var error = await ApiErrorTestHelper.ReadErrorMessageAsync(response);
        error.Should().Be("Kino s tom email adresom već postoji.");
    }

    [Fact]
    public async Task PostCinema_ReturnsBadRequest_WhenNameAlreadyExistsInCity()
    {
        await _factory.ClearDatabaseAsync();
        await ApiTestData.CreateCinemaAsync(_factory, name: "Kino Test", city: "Zagreb");
        var request = CreateCinemaWriteDto("Kino Test");
        request.Email = "unique-cinema@example.com";

        var response = await _client.PostAsJsonAsync("/api/kina", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var error = await ApiErrorTestHelper.ReadErrorMessageAsync(response);
        error.Should().Be("Kino s tim nazivom već postoji u odabranom gradu.");
    }

    [Fact]
    public async Task PutCinema_UpdatesExistingCinema()
    {
        await _factory.ClearDatabaseAsync();
        var cinema = await ApiTestData.CreateCinemaAsync(_factory);
        var request = CreateCinemaWriteDto("Updated Cinema");

        var response = await _client.PutAsJsonAsync($"/api/kina/{cinema.Id}", request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var dto = await response.Content.ReadFromJsonAsync<CinemaDTO>();
        dto.Should().NotBeNull();
        dto!.Name.Should().Be(request.Name);
    }

    [Fact]
    public async Task PutCinema_ReturnsNotFound_WhenCinemaDoesNotExist()
    {
        await _factory.ClearDatabaseAsync();
        var request = CreateCinemaWriteDto("Missing Cinema");

        var response = await _client.PutAsJsonAsync("/api/kina/9999", request);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteCinema_SoftDeletesExistingCinema()
    {
        await _factory.ClearDatabaseAsync();
        var cinema = await ApiTestData.CreateCinemaAsync(_factory);

        var response = await _client.DeleteAsync($"/api/kina/{cinema.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<CinemaDbContext>();
        var deleted = await dbContext.Cinemas.FindAsync(cinema.Id);
        deleted!.DeletedAt.Should().NotBeNull();
    }

    [Fact]
    public async Task DeleteCinema_ReturnsNotFound_WhenCinemaDoesNotExist()
    {
        await _factory.ClearDatabaseAsync();

        var response = await _client.DeleteAsync("/api/kina/9999");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task CinemaApiEndpoint_ReturnsUnauthorized_WhenUserIsNotAuthenticated()
    {
        var response = await _factory.CreateClient().GetAsync("/api/kina/9999");

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    private static CinemaWriteDTO CreateCinemaWriteDto(string name)
    {
        return new CinemaWriteDTO
        {
            Name = name,
            City = "Zagreb",
            Street = "Test Street",
            HouseNumber = "1",
            PostalCode = "10000",
            Email = "cinema@example.com",
            Phone = "+385 1 234 567"
        };
    }
}
