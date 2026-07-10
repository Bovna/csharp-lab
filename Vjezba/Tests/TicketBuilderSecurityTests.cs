using System.Net;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Vjezba.DAL;
using Vjezba.Model.Entities;

namespace Vjezba.Tests;

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
}
