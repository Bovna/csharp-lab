using System.Net;
using FluentAssertions;

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
}
