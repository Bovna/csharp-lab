using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Vjezba.DAL;
using Vjezba.Model.Entities;
using Vjezba.Web.DTOs;

namespace Vjezba.Tests;

public sealed class TicketApiControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public TicketApiControllerTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateAuthenticatedClient("Admin");
    }

    [Fact]
    public async Task GetAllTickets_ReturnsCollection()
    {
        await _factory.ClearDatabaseAsync();
        var ticket = await ApiTestData.CreateTicketAsync(_factory);

        var response = await _client.GetAsync("/api/ulaznice");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var tickets = await response.Content.ReadFromJsonAsync<List<TicketDTO>>();
        tickets.Should().NotBeNull();
        tickets.Should().ContainSingle(t => t.Id == ticket.Id && t.TicketNumber == ticket.TicketNumber);
    }

    [Fact]
    public async Task GetAllTickets_FiltersByStatus()
    {
        await _factory.ClearDatabaseAsync();
        var activeTicket = await ApiTestData.CreateTicketAsync(_factory, ticketNumber: "ACTIVE-001", status: TicketStatus.Active);
        var usedTicket = await ApiTestData.CreateTicketAsync(_factory, ticketNumber: "USED-001", status: TicketStatus.Used);

        var response = await _client.GetAsync("/api/ulaznice?status=Used");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var tickets = await response.Content.ReadFromJsonAsync<List<TicketDTO>>();
        tickets.Should().NotBeNull();
        tickets.Should().ContainSingle(t => t.Id == usedTicket.Id);
        tickets.Should().NotContain(t => t.Id == activeTicket.Id);
    }

    [Fact]
    public async Task SearchTickets_ReturnsMatchingResultsOnly()
    {
        await _factory.ClearDatabaseAsync();
        var matchingTicket = await ApiTestData.CreateTicketAsync(_factory, ticketNumber: "AURORA-001");
        var nonMatchingTicket = await ApiTestData.CreateTicketAsync(_factory, ticketNumber: "BOREALIS-001");

        var response = await _client.GetAsync("/api/ulaznice/pretraga/AURORA");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var tickets = await response.Content.ReadFromJsonAsync<List<TicketDTO>>();
        tickets.Should().NotBeNull();
        tickets.Should().ContainSingle(t => t.Id == matchingTicket.Id);
        tickets.Should().NotContain(t => t.Id == nonMatchingTicket.Id);
    }

    [Fact]
    public async Task GetTicketById_ReturnsTicket_WhenTicketExists()
    {
        await _factory.ClearDatabaseAsync();
        var ticket = await ApiTestData.CreateTicketAsync(_factory);

        var response = await _client.GetAsync($"/api/ulaznice/{ticket.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var dto = await response.Content.ReadFromJsonAsync<TicketDTO>();
        dto.Should().NotBeNull();
        dto!.Id.Should().Be(ticket.Id);
        dto.TicketNumber.Should().Be(ticket.TicketNumber);
    }

    [Fact]
    public async Task GetTicketById_ReturnsNotFound_WhenTicketDoesNotExist()
    {
        await _factory.ClearDatabaseAsync();

        var response = await _client.GetAsync("/api/ulaznice/9999");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task PostTicket_CreatesTicket_AndReturnsCreated()
    {
        await _factory.ClearDatabaseAsync();
        var setup = await CreateTicketSetupAsync();
        var request = CreateTicketWriteDto(setup.Screening.Id, setup.Seat.Id, setup.Customer.Id, "CREATED-001");

        var response = await _client.PostAsJsonAsync("/api/ulaznice", request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var dto = await response.Content.ReadFromJsonAsync<TicketDTO>();
        dto.Should().NotBeNull();
        dto!.TicketNumber.Should().Be(request.TicketNumber);
        dto.Customer.Id.Should().Be(setup.Customer.Id);
        dto.Screening.Id.Should().Be(setup.Screening.Id);
        dto.Seat!.Id.Should().Be(setup.Seat.Id);
    }

    [Fact]
    public async Task PostTicket_ReturnsBadRequest_WhenModelIsInvalid()
    {
        await _factory.ClearDatabaseAsync();

        var response = await _client.PostAsJsonAsync("/api/ulaznice", new TicketWriteDTO());

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task PostTicket_ReturnsBadRequest_WhenSeatIsAlreadyReserved()
    {
        await _factory.ClearDatabaseAsync();
        var setup = await CreateTicketSetupAsync();
        await ApiTestData.CreateTicketAsync(_factory, setup.Screening.Id, setup.Seat.Id, setup.Customer.Id);
        var anotherCustomer = await ApiTestData.CreateCustomerAsync(_factory, firstName: "Another", lastName: "Customer");
        var request = CreateTicketWriteDto(setup.Screening.Id, setup.Seat.Id, anotherCustomer.Id, "DUPLICATE-SEAT-001");

        var response = await _client.PostAsJsonAsync("/api/ulaznice", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var error = await ApiErrorTestHelper.ReadErrorMessageAsync(response);
        error.Should().Be("Odabrano sjedalo je već rezervirano za tu projekciju.");
    }

    [Fact]
    public async Task PostTicket_AllowsSeatThatWasReleasedByCancelledTicket()
    {
        await _factory.ClearDatabaseAsync();
        var setup = await CreateTicketSetupAsync();
        await ApiTestData.CreateTicketAsync(
            _factory,
            setup.Screening.Id,
            setup.Seat.Id,
            setup.Customer.Id,
            status: TicketStatus.Cancelled);
        var anotherCustomer = await ApiTestData.CreateCustomerAsync(_factory, firstName: "Available", lastName: "Seat");
        var request = CreateTicketWriteDto(setup.Screening.Id, setup.Seat.Id, anotherCustomer.Id, "RELEASED-SEAT-001");

        var response = await _client.PostAsJsonAsync("/api/ulaznice", request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task PostTicket_ReturnsBadRequest_WhenSeatDoesNotBelongToScreeningHall()
    {
        await _factory.ClearDatabaseAsync();
        var setup = await CreateTicketSetupAsync();
        var otherHall = await ApiTestData.CreateHallAsync(_factory, name: "Other Hall");
        var otherSeat = await ApiTestData.CreateSeatAsync(_factory, otherHall.Id);
        var request = CreateTicketWriteDto(setup.Screening.Id, otherSeat.Id, setup.Customer.Id, "WRONG-SEAT-001");

        var response = await _client.PostAsJsonAsync("/api/ulaznice", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var error = await ApiErrorTestHelper.ReadErrorMessageAsync(response);
        error.Should().Be("Odabrano sjedalo ne pripada dvorani projekcije.");
    }

    [Fact]
    public async Task PostTicket_ReturnsBadRequest_WhenScreeningHasEnded()
    {
        await _factory.ClearDatabaseAsync();
        var cinema = await ApiTestData.CreateCinemaAsync(_factory);
        var hall = await ApiTestData.CreateHallAsync(_factory, cinemaId: cinema.Id);
        var movie = await ApiTestData.CreateMovieAsync(_factory);
        var screening = await ApiTestData.CreateScreeningAsync(
            _factory,
            movie.Id,
            hall.Id,
            new DateTime(2026, 1, 1, 18, 0, 0),
            new DateTime(2026, 1, 1, 20, 0, 0));
        var seat = await ApiTestData.CreateSeatAsync(_factory, hall.Id);
        var customer = await ApiTestData.CreateCustomerAsync(_factory);
        var request = CreateTicketWriteDto(screening.Id, seat.Id, customer.Id, "ENDED-001");

        var response = await _client.PostAsJsonAsync("/api/ulaznice", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var error = await ApiErrorTestHelper.ReadErrorMessageAsync(response);
        error.Should().Be("Nije moguće kupiti kartu za završenu projekciju.");
    }

    [Fact]
    public async Task PutTicket_UpdatesExistingTicket()
    {
        await _factory.ClearDatabaseAsync();
        var ticket = await ApiTestData.CreateTicketAsync(_factory);
        var setup = await CreateTicketSetupAsync();
        var request = CreateTicketWriteDto(setup.Screening.Id, setup.Seat.Id, setup.Customer.Id, "UPDATED-001", TicketStatus.Used);

        var response = await _client.PutAsJsonAsync($"/api/ulaznice/{ticket.Id}", request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var dto = await response.Content.ReadFromJsonAsync<TicketDTO>();
        dto.Should().NotBeNull();
        dto!.TicketNumber.Should().Be(request.TicketNumber);
        dto.Status.Should().Be(request.Status);
        dto.Customer.Id.Should().Be(setup.Customer.Id);
    }

    [Fact]
    public async Task PutTicket_ReturnsNotFound_WhenTicketDoesNotExist()
    {
        await _factory.ClearDatabaseAsync();
        var setup = await CreateTicketSetupAsync();
        var request = CreateTicketWriteDto(setup.Screening.Id, setup.Seat.Id, setup.Customer.Id, "MISSING-001");

        var response = await _client.PutAsJsonAsync("/api/ulaznice/9999", request);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task PutTicket_ReturnsBadRequest_WhenSeatDoesNotBelongToScreeningHall()
    {
        await _factory.ClearDatabaseAsync();
        var ticket = await ApiTestData.CreateTicketAsync(_factory);
        var setup = await CreateTicketSetupAsync();
        var otherHall = await ApiTestData.CreateHallAsync(_factory, name: "Other Hall");
        var otherSeat = await ApiTestData.CreateSeatAsync(_factory, otherHall.Id);
        var request = CreateTicketWriteDto(setup.Screening.Id, otherSeat.Id, setup.Customer.Id, "WRONG-PUT-001");

        var response = await _client.PutAsJsonAsync($"/api/ulaznice/{ticket.Id}", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var error = await ApiErrorTestHelper.ReadErrorMessageAsync(response);
        error.Should().Be("Odabrano sjedalo ne pripada dvorani projekcije.");
    }

    [Fact]
    public async Task DeleteTicket_SoftDeletesExistingTicket()
    {
        await _factory.ClearDatabaseAsync();
        var ticket = await ApiTestData.CreateTicketAsync(_factory);

        var response = await _client.DeleteAsync($"/api/ulaznice/{ticket.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<CinemaDbContext>();
        var deleted = await dbContext.Tickets.FindAsync(ticket.Id);
        deleted!.DeletedAt.Should().NotBeNull();
    }

    [Fact]
    public async Task DeleteTicket_ReturnsNotFound_WhenTicketDoesNotExist()
    {
        await _factory.ClearDatabaseAsync();

        var response = await _client.DeleteAsync("/api/ulaznice/9999");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task TicketApiEndpoint_ReturnsUnauthorized_WhenUserIsNotAuthenticated()
    {
        var response = await _factory.CreateClient().GetAsync("/api/ulaznice/9999");

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    private async Task<(Customer Customer, Screening Screening, Seat Seat)> CreateTicketSetupAsync()
    {
        var cinema = await ApiTestData.CreateCinemaAsync(_factory);
        var hall = await ApiTestData.CreateHallAsync(_factory, cinemaId: cinema.Id);
        var movie = await ApiTestData.CreateMovieAsync(_factory);
        var screening = await ApiTestData.CreateScreeningAsync(
            _factory,
            movie.Id,
            hall.Id,
            new DateTime(2027, 3, 1, 18, 0, 0),
            new DateTime(2027, 3, 1, 20, 0, 0));
        var seat = await ApiTestData.CreateSeatAsync(_factory, hall.Id);
        var customer = await ApiTestData.CreateCustomerAsync(_factory);

        return (customer, screening, seat);
    }

    private static TicketWriteDTO CreateTicketWriteDto(
        int screeningId,
        int seatId,
        int customerId,
        string ticketNumber,
        TicketStatus status = TicketStatus.Active)
    {
        return new TicketWriteDTO
        {
            TicketNumber = ticketNumber,
            PurchasedAt = new DateTime(2026, 3, 2, 12, 0, 0),
            Price = 11.50m,
            Status = status,
            ScreeningId = screeningId,
            SeatId = seatId,
            CustomerId = customerId
        };
    }
}
