using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Vjezba.DAL;
using Vjezba.Model.Entities;
using Vjezba.Web.DTOs;

namespace Vjezba.Tests;

public sealed class SeatApiControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public SeatApiControllerTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateAuthenticatedClient();
    }

    [Fact]
    public async Task GetAllSeats_ReturnsCollection()
    {
        await _factory.ClearDatabaseAsync();
        var seat = await ApiTestData.CreateSeatAsync(_factory);

        var response = await _client.GetAsync("/api/sjedala");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var seats = await response.Content.ReadFromJsonAsync<List<SeatDTO>>();
        seats.Should().NotBeNull();
        seats.Should().ContainSingle(s => s.Id == seat.Id && s.RowLabel == seat.RowLabel);
    }

    [Fact]
    public async Task GetSeatById_ReturnsSeat_WhenSeatExists()
    {
        await _factory.ClearDatabaseAsync();
        var seat = await ApiTestData.CreateSeatAsync(_factory, rowLabel: "B", seatNumber: 2);

        var response = await _client.GetAsync($"/api/sjedala/{seat.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var dto = await response.Content.ReadFromJsonAsync<SeatDTO>();
        dto.Should().NotBeNull();
        dto!.Id.Should().Be(seat.Id);
        dto.RowLabel.Should().Be(seat.RowLabel);
        dto.SeatNumber.Should().Be(seat.SeatNumber);
    }

    [Fact]
    public async Task GetSeatById_ReturnsNotFound_WhenSeatDoesNotExist()
    {
        await _factory.ClearDatabaseAsync();

        var response = await _client.GetAsync("/api/sjedala/9999");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task PostSeat_CreatesSeat_AndReturnsCreated()
    {
        await _factory.ClearDatabaseAsync();
        var hall = await ApiTestData.CreateHallAsync(_factory);
        var request = CreateSeatWriteDto(hall.Id, rowLabel: "C", seatNumber: 3);

        var response = await _client.PostAsJsonAsync("/api/sjedala", request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var dto = await response.Content.ReadFromJsonAsync<SeatDTO>();
        dto.Should().NotBeNull();
        dto!.RowLabel.Should().Be(request.RowLabel);
        dto.SeatNumber.Should().Be(request.SeatNumber);
        dto.Hall.Id.Should().Be(hall.Id);
    }

    [Fact]
    public async Task PostSeat_ReturnsBadRequest_WhenModelIsInvalid()
    {
        await _factory.ClearDatabaseAsync();

        var response = await _client.PostAsJsonAsync("/api/sjedala", new SeatWriteDTO());

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task PutSeat_UpdatesExistingSeat()
    {
        await _factory.ClearDatabaseAsync();
        var hall = await ApiTestData.CreateHallAsync(_factory);
        var seat = await ApiTestData.CreateSeatAsync(_factory, hall.Id);
        var request = CreateSeatWriteDto(hall.Id, rowLabel: "D", seatNumber: 4, SeatType.Vip);

        var response = await _client.PutAsJsonAsync($"/api/sjedala/{seat.Id}", request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var dto = await response.Content.ReadFromJsonAsync<SeatDTO>();
        dto.Should().NotBeNull();
        dto!.RowLabel.Should().Be(request.RowLabel);
        dto.SeatNumber.Should().Be(request.SeatNumber);
        dto.SeatType.Should().Be(request.SeatType);
    }

    [Fact]
    public async Task PutSeat_ReturnsNotFound_WhenSeatDoesNotExist()
    {
        await _factory.ClearDatabaseAsync();
        var hall = await ApiTestData.CreateHallAsync(_factory);
        var request = CreateSeatWriteDto(hall.Id);

        var response = await _client.PutAsJsonAsync("/api/sjedala/9999", request);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteSeat_SoftDeletesExistingSeat()
    {
        await _factory.ClearDatabaseAsync();
        var seat = await ApiTestData.CreateSeatAsync(_factory);

        var response = await _client.DeleteAsync($"/api/sjedala/{seat.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<CinemaDbContext>();
        var deleted = await dbContext.Seats.FindAsync(seat.Id);
        deleted!.DeletedAt.Should().NotBeNull();
    }

    [Fact]
    public async Task DeleteSeat_ReturnsNotFound_WhenSeatDoesNotExist()
    {
        await _factory.ClearDatabaseAsync();

        var response = await _client.DeleteAsync("/api/sjedala/9999");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task SeatApiEndpoint_ReturnsUnauthorized_WhenUserIsNotAuthenticated()
    {
        var response = await _factory.CreateClient().GetAsync("/api/sjedala");

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    private static SeatWriteDTO CreateSeatWriteDto(
        int hallId,
        string rowLabel = "A",
        int seatNumber = 1,
        SeatType seatType = SeatType.Standard)
    {
        return new SeatWriteDTO
        {
            RowLabel = rowLabel,
            SeatNumber = seatNumber,
            SeatType = seatType,
            HallId = hallId
        };
    }
}
