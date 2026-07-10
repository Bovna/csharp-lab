using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Vjezba.DAL;
using Vjezba.Web.DTOs;

namespace Vjezba.Tests;

public sealed class CustomerApiControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public CustomerApiControllerTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateAuthenticatedClient("Admin");
    }

    [Fact]
    public async Task GetAllCustomers_ReturnsCollection()
    {
        await _factory.ClearDatabaseAsync();
        var customer = await ApiTestData.CreateCustomerAsync(_factory, firstName: "Ana");

        var response = await _client.GetAsync("/api/kupci");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var customers = await response.Content.ReadFromJsonAsync<List<CustomerDTO>>();
        customers.Should().NotBeNull();
        customers.Should().ContainSingle(c => c.Id == customer.Id && c.FirstName == customer.FirstName);
    }

    [Fact]
    public async Task GetAllCustomers_FiltersByLoyaltyMembership()
    {
        await _factory.ClearDatabaseAsync();
        var loyaltyCustomer = await ApiTestData.CreateCustomerAsync(_factory, firstName: "Loyal", isLoyaltyMember: true);
        var regularCustomer = await ApiTestData.CreateCustomerAsync(
            _factory,
            firstName: "Regular",
            isLoyaltyMember: false,
            loyaltyPoints: 0);

        var response = await _client.GetAsync("/api/kupci?loyaltyMember=false");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var customers = await response.Content.ReadFromJsonAsync<List<CustomerDTO>>();
        customers.Should().NotBeNull();
        customers.Should().ContainSingle(c => c.Id == regularCustomer.Id);
        customers.Should().NotContain(c => c.Id == loyaltyCustomer.Id);
    }

    [Fact]
    public async Task SearchCustomers_ReturnsMatchingResultsOnly()
    {
        await _factory.ClearDatabaseAsync();
        var matchingCustomer = await ApiTestData.CreateCustomerAsync(_factory, firstName: "Aurora", lastName: "Buyer");
        var nonMatchingCustomer = await ApiTestData.CreateCustomerAsync(_factory, firstName: "Marko", lastName: "Buyer");

        var response = await _client.GetAsync("/api/kupci/pretraga/Aurora");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var customers = await response.Content.ReadFromJsonAsync<List<CustomerDTO>>();
        customers.Should().NotBeNull();
        customers.Should().ContainSingle(c => c.Id == matchingCustomer.Id);
        customers.Should().NotContain(c => c.Id == nonMatchingCustomer.Id);
    }

    [Fact]
    public async Task GetCustomerById_ReturnsCustomer_WhenCustomerExists()
    {
        await _factory.ClearDatabaseAsync();
        var customer = await ApiTestData.CreateCustomerAsync(_factory, firstName: "Marko");

        var response = await _client.GetAsync($"/api/kupci/{customer.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var dto = await response.Content.ReadFromJsonAsync<CustomerDTO>();
        dto.Should().NotBeNull();
        dto!.Id.Should().Be(customer.Id);
        dto.FirstName.Should().Be(customer.FirstName);
    }

    [Fact]
    public async Task GetCustomerById_ReturnsNotFound_WhenCustomerDoesNotExist()
    {
        await _factory.ClearDatabaseAsync();

        var response = await _client.GetAsync("/api/kupci/9999");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task PostCustomer_CreatesCustomer_AndReturnsCreated()
    {
        await _factory.ClearDatabaseAsync();
        var request = CreateCustomerWriteDto("Created");

        var response = await _client.PostAsJsonAsync("/api/kupci", request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var dto = await response.Content.ReadFromJsonAsync<CustomerDTO>();
        dto.Should().NotBeNull();
        dto!.FirstName.Should().Be(request.FirstName);
    }

    [Fact]
    public async Task PostCustomer_ReturnsBadRequest_WhenModelIsInvalid()
    {
        await _factory.ClearDatabaseAsync();

        var response = await _client.PostAsJsonAsync("/api/kupci", new CustomerWriteDTO());

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task PostCustomer_ReturnsBadRequest_WhenEmailAlreadyExists()
    {
        await _factory.ClearDatabaseAsync();
        await ApiTestData.CreateCustomerAsync(_factory);
        var request = CreateCustomerWriteDto("Other");
        request.Email = "test.customer@example.com";

        var response = await _client.PostAsJsonAsync("/api/kupci", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var error = await ApiErrorTestHelper.ReadErrorMessageAsync(response);
        error.Should().Be("Kupac s tom email adresom već postoji.");
    }

    [Fact]
    public async Task PostCustomer_ReturnsBadRequest_WhenLoyaltyPointsAreNegative()
    {
        await _factory.ClearDatabaseAsync();
        var request = CreateCustomerWriteDto("Negative");
        request.LoyaltyPoints = -1;

        var response = await _client.PostAsJsonAsync("/api/kupci", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task PutCustomer_UpdatesExistingCustomer()
    {
        await _factory.ClearDatabaseAsync();
        var customer = await ApiTestData.CreateCustomerAsync(_factory);
        var request = CreateCustomerWriteDto("Updated");

        var response = await _client.PutAsJsonAsync($"/api/kupci/{customer.Id}", request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var dto = await response.Content.ReadFromJsonAsync<CustomerDTO>();
        dto.Should().NotBeNull();
        dto!.FirstName.Should().Be(request.FirstName);
    }

    [Fact]
    public async Task PutCustomer_ReturnsNotFound_WhenCustomerDoesNotExist()
    {
        await _factory.ClearDatabaseAsync();
        var request = CreateCustomerWriteDto("Missing");

        var response = await _client.PutAsJsonAsync("/api/kupci/9999", request);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task PutCustomer_ReturnsBadRequest_WhenEmailAlreadyExists()
    {
        await _factory.ClearDatabaseAsync();
        var existingCustomer = await ApiTestData.CreateCustomerAsync(
            _factory,
            firstName: "Existing",
            email: "existing-customer@example.com");
        var customerToUpdate = await ApiTestData.CreateCustomerAsync(
            _factory,
            firstName: "Update",
            email: "update-customer@example.com");
        var request = CreateCustomerWriteDto("Updated");
        request.Email = existingCustomer.Email;

        var response = await _client.PutAsJsonAsync($"/api/kupci/{customerToUpdate.Id}", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var error = await ApiErrorTestHelper.ReadErrorMessageAsync(response);
        error.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task DeleteCustomer_SoftDeletesExistingCustomer()
    {
        await _factory.ClearDatabaseAsync();
        var customer = await ApiTestData.CreateCustomerAsync(_factory);

        var response = await _client.DeleteAsync($"/api/kupci/{customer.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<CinemaDbContext>();
        var deleted = await dbContext.Customers.FindAsync(customer.Id);
        deleted!.DeletedAt.Should().NotBeNull();
    }

    [Fact]
    public async Task DeleteCustomer_ReturnsNotFound_WhenCustomerDoesNotExist()
    {
        await _factory.ClearDatabaseAsync();

        var response = await _client.DeleteAsync("/api/kupci/9999");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task CustomerApiEndpoint_ReturnsUnauthorized_WhenUserIsNotAuthenticated()
    {
        var response = await _factory.CreateClient().GetAsync("/api/kupci/9999");

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    private static CustomerWriteDTO CreateCustomerWriteDto(string firstName)
    {
        return new CustomerWriteDTO
        {
            FirstName = firstName,
            LastName = "Customer",
            City = "Zagreb",
            Street = "Customer Street",
            HouseNumber = "2",
            PostalCode = "10000",
            Email = "customer@example.com",
            Phone = "+385 91 234 5678",
            RegisteredAt = new DateTime(2026, 1, 1),
            IsLoyaltyMember = true,
            LoyaltyPoints = 20
        };
    }
}
