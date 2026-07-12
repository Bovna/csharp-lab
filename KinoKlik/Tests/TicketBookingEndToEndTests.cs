using System.Net;
using System.Text.RegularExpressions;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using KinoKlik.DAL;

namespace KinoKlik.Tests;

public sealed class TicketBookingEndToEndTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

    public TicketBookingEndToEndTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Checkout_PostsBookingAndDisplaysConfirmation()
    {
        await _factory.ClearDatabaseAsync();

        var cinema = await ApiTestData.CreateCinemaAsync(_factory, name: "E2E Cinema");
        var hall = await ApiTestData.CreateHallAsync(_factory, name: "E2E Hall", cinemaId: cinema.Id);
        var movie = await ApiTestData.CreateMovieAsync(_factory, title: "E2E Movie");
        var screening = await ApiTestData.CreateScreeningAsync(
            _factory,
            movie.Id,
            hall.Id,
            DateTime.Now.AddDays(2),
            DateTime.Now.AddDays(2).AddHours(2));
        var seat = await ApiTestData.CreateSeatAsync(_factory, hall.Id, rowLabel: "C", seatNumber: 7);

        var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false,
            HandleCookies = true
        });
        var checkoutPath = $"/TicketBuilder/checkout/{cinema.Id}/{movie.Id}/{screening.Id}/{seat.Id}";
        var checkoutResponse = await client.GetAsync(checkoutPath);

        checkoutResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var checkoutHtml = await checkoutResponse.Content.ReadAsStringAsync();
        var antiForgeryToken = ExtractAntiForgeryToken(checkoutHtml);

        var purchaseResponse = await client.PostAsync(
            "/TicketBuilder/Purchase",
            new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["__RequestVerificationToken"] = antiForgeryToken,
                ["Input.CinemaId"] = cinema.Id.ToString(),
                ["Input.MovieId"] = movie.Id.ToString(),
                ["Input.ScreeningId"] = screening.Id.ToString(),
                ["Input.SeatId"] = seat.Id.ToString(),
                ["Input.FirstName"] = "Demo",
                ["Input.LastName"] = "Kupac",
                ["Input.Email"] = "demo.kupac@example.com",
                ["Input.Phone"] = "+385 91 555 0101",
                ["Input.City"] = "Zagreb",
                ["Input.Street"] = "Filmska ulica",
                ["Input.HouseNumber"] = "10",
                ["Input.PostalCode"] = "10000",
                ["Input.IsLoyaltyMember"] = "true"
            }));

        purchaseResponse.StatusCode.Should().Be(HttpStatusCode.Redirect);
        purchaseResponse.Headers.Location.Should().NotBeNull();
        purchaseResponse.Headers.Location!.OriginalString
            .Should().MatchRegex("^/TicketBuilder/success/[0-9a-fA-F-]{36}$");

        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<CinemaDbContext>();
        var ticket = await dbContext.Tickets
            .Include(existing => existing.Customer)
            .SingleAsync(existing => existing.ScreeningId == screening.Id && existing.SeatId == seat.Id);

        ticket.Customer.Email.Should().Be("demo.kupac@example.com");

        var confirmationResponse = await client.GetAsync(purchaseResponse.Headers.Location);
        confirmationResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        (await confirmationResponse.Content.ReadAsStringAsync()).Should().Contain(ticket.TicketNumber);
    }

    private static string ExtractAntiForgeryToken(string html)
    {
        var match = Regex.Match(
            html,
            "name=\"__RequestVerificationToken\"[^>]*value=\"([^\"]+)\"",
            RegexOptions.CultureInvariant);

        match.Success.Should().BeTrue("the checkout form should contain an antiforgery token");
        return WebUtility.HtmlDecode(match.Groups[1].Value);
    }
}
