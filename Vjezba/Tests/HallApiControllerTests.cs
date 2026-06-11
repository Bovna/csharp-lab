using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Vjezba.DAL;
using Vjezba.Web.DTOs;

namespace Vjezba.Tests;

public sealed class HallApiControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public HallApiControllerTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateAuthenticatedClient();
    }

    [Fact]
    public async Task GetAllHalls_ReturnsCollection()
    {
        await _factory.ClearDatabaseAsync();
        var hall = await ApiTestData.CreateHallAsync(_factory, name: "Hall One");

        var response = await _client.GetAsync("/api/dvorane");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var halls = await response.Content.ReadFromJsonAsync<List<HallDTO>>();
        halls.Should().NotBeNull();
        halls.Should().ContainSingle(h => h.Id == hall.Id && h.Name == hall.Name);
    }

    [Fact]
    public async Task GetHallById_ReturnsHall_WhenHallExists()
    {
        await _factory.ClearDatabaseAsync();
        var hall = await ApiTestData.CreateHallAsync(_factory, name: "Existing Hall");

        var response = await _client.GetAsync($"/api/dvorane/{hall.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var dto = await response.Content.ReadFromJsonAsync<HallDTO>();
        dto.Should().NotBeNull();
        dto!.Id.Should().Be(hall.Id);
        dto.Name.Should().Be(hall.Name);
    }

    [Fact]
    public async Task GetHallById_ReturnsNotFound_WhenHallDoesNotExist()
    {
        await _factory.ClearDatabaseAsync();

        var response = await _client.GetAsync("/api/dvorane/9999");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task PostHall_CreatesHall_AndReturnsCreated()
    {
        await _factory.ClearDatabaseAsync();
        var cinema = await ApiTestData.CreateCinemaAsync(_factory);
        var request = CreateHallWriteDto("Created Hall", cinema.Id);

        var response = await _client.PostAsJsonAsync("/api/dvorane", request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var dto = await response.Content.ReadFromJsonAsync<HallDTO>();
        dto.Should().NotBeNull();
        dto!.Name.Should().Be(request.Name);
        dto.CinemaId.Should().Be(cinema.Id);
    }

    [Fact]
    public async Task PostHall_ReturnsBadRequest_WhenModelIsInvalid()
    {
        await _factory.ClearDatabaseAsync();

        var response = await _client.PostAsJsonAsync("/api/dvorane", new HallWriteDTO());

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task PutHall_UpdatesExistingHall()
    {
        await _factory.ClearDatabaseAsync();
        var cinema = await ApiTestData.CreateCinemaAsync(_factory);
        var hall = await ApiTestData.CreateHallAsync(_factory, cinemaId: cinema.Id);
        var request = CreateHallWriteDto("Updated Hall", cinema.Id);

        var response = await _client.PutAsJsonAsync($"/api/dvorane/{hall.Id}", request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var dto = await response.Content.ReadFromJsonAsync<HallDTO>();
        dto.Should().NotBeNull();
        dto!.Name.Should().Be(request.Name);
    }

    [Fact]
    public async Task PutHall_ReturnsNotFound_WhenHallDoesNotExist()
    {
        await _factory.ClearDatabaseAsync();
        var cinema = await ApiTestData.CreateCinemaAsync(_factory);
        var request = CreateHallWriteDto("Missing Hall", cinema.Id);

        var response = await _client.PutAsJsonAsync("/api/dvorane/9999", request);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteHall_SoftDeletesExistingHall()
    {
        await _factory.ClearDatabaseAsync();
        var hall = await ApiTestData.CreateHallAsync(_factory);

        var response = await _client.DeleteAsync($"/api/dvorane/{hall.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<CinemaDbContext>();
        var deleted = await dbContext.Halls.FindAsync(hall.Id);
        deleted!.DeletedAt.Should().NotBeNull();
    }

    [Fact]
    public async Task DeleteHall_ReturnsNotFound_WhenHallDoesNotExist()
    {
        await _factory.ClearDatabaseAsync();

        var response = await _client.DeleteAsync("/api/dvorane/9999");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task HallApiEndpoint_ReturnsUnauthorized_WhenUserIsNotAuthenticated()
    {
        var response = await _factory.CreateClient().GetAsync("/api/dvorane");

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    private static HallWriteDTO CreateHallWriteDto(string name, int cinemaId)
    {
        return new HallWriteDTO
        {
            Name = name,
            Capacity = 100,
            Supports3D = true,
            CinemaId = cinemaId
        };
    }
}
