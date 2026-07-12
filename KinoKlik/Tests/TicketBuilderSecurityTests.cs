using System.Net;
using System.Text.RegularExpressions;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using KinoKlik.DAL;
using KinoKlik.Model.Entities;

namespace KinoKlik.Tests;

public sealed class TicketBuilderSecurityTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

    public TicketBuilderSecurityTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Success_UsesConfirmationCode_AndDisablesCaching()
    {
        await _factory.ClearDatabaseAsync();
        var ticket = await ApiTestData.CreateTicketAsync(_factory, ticketNumber: "CONFIRM-001");

        var response = await _factory.CreateClient()
            .GetAsync($"/TicketBuilder/success/{ticket.ConfirmationCode}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Headers.CacheControl!.NoStore.Should().BeTrue();

        var html = await response.Content.ReadAsStringAsync();
        html.Should().Contain(ticket.TicketNumber);
    }

    [Fact]
    public async Task Success_DoesNotExposeNumericTicketIdRoute()
    {
        await _factory.ClearDatabaseAsync();
        var ticket = await ApiTestData.CreateTicketAsync(_factory);

        var response = await _factory.CreateClient()
            .GetAsync($"/TicketBuilder/success/{ticket.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public void TicketModel_UsesUniqueFilteredIndex_ForActiveSeatReservations()
    {
        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<CinemaDbContext>();
        var ticketType = dbContext.Model.FindEntityType(typeof(Ticket));

        var index = ticketType!.GetIndexes()
            .Single(existing => existing.Properties.Select(property => property.Name)
                .SequenceEqual(new[] { nameof(Ticket.ScreeningId), nameof(Ticket.SeatId) }));

        index.IsUnique.Should().BeTrue();
        index.GetFilter().Should().Contain("[DeletedAt] IS NULL");
        index.GetFilter().Should().Contain("[Status] IN (0, 2)");
    }

    [Fact]
    public async Task Purchase_AcceptsEmptyOptionalContactAndAddressFields()
    {
        await _factory.ClearDatabaseAsync();
        var cinema = await ApiTestData.CreateCinemaAsync(_factory);
        var hall = await ApiTestData.CreateHallAsync(_factory, cinemaId: cinema.Id);
        var movie = await ApiTestData.CreateMovieAsync(_factory);
        var screening = await ApiTestData.CreateScreeningAsync(
            _factory,
            movieId: movie.Id,
            hallId: hall.Id,
            startTime: DateTime.Now.AddDays(1),
            endTime: DateTime.Now.AddDays(1).AddHours(2));
        var seat = await ApiTestData.CreateSeatAsync(_factory, hallId: hall.Id);
        var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });

        var checkoutResponse = await client.GetAsync(
            $"/TicketBuilder/checkout/{cinema.Id}/{movie.Id}/{screening.Id}/{seat.Id}");
        checkoutResponse.EnsureSuccessStatusCode();
        var checkoutHtml = await checkoutResponse.Content.ReadAsStringAsync();
        var tokenMatch = Regex.Match(
            checkoutHtml,
            "name=\"__RequestVerificationToken\"[^>]*value=\"([^\"]+)\"");
        tokenMatch.Success.Should().BeTrue();

        var response = await client.PostAsync(
            "/TicketBuilder/Purchase",
            new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["__RequestVerificationToken"] = WebUtility.HtmlDecode(tokenMatch.Groups[1].Value),
                ["Input.CinemaId"] = cinema.Id.ToString(),
                ["Input.MovieId"] = movie.Id.ToString(),
                ["Input.ScreeningId"] = screening.Id.ToString(),
                ["Input.SeatId"] = seat.Id.ToString(),
                ["Input.FirstName"] = "Ana",
                ["Input.LastName"] = "Anić",
                ["Input.Email"] = "ana@example.com",
                ["Input.Phone"] = string.Empty,
                ["Input.City"] = string.Empty,
                ["Input.PostalCode"] = string.Empty,
                ["Input.Street"] = string.Empty,
                ["Input.HouseNumber"] = string.Empty
            }));

        response.StatusCode.Should().Be(HttpStatusCode.Redirect);
        response.Headers.Location!.ToString().Should().Contain("/TicketBuilder/success/");

        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<CinemaDbContext>();
        var customer = await dbContext.Customers.SingleAsync();
        customer.Phone.Should().BeEmpty();
        customer.City.Should().BeEmpty();
        customer.PostalCode.Should().BeEmpty();
        customer.Street.Should().BeEmpty();
        customer.HouseNumber.Should().BeEmpty();
    }
}
