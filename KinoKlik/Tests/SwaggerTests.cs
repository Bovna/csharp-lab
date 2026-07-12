using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;

namespace KinoKlik.Tests;

public sealed class SwaggerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public SwaggerTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task OpenApiDocument_ContainsOnlyApiControllerRoutes()
    {
        var response = await _client.GetAsync("/swagger/v1/swagger.json");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        using var document = await response.Content.ReadFromJsonAsync<JsonDocument>();
        document.Should().NotBeNull();

        var root = document!.RootElement;
        root.GetProperty("info").GetProperty("title").GetString().Should().Be("KinoKlik API");
        root.GetProperty("info").GetProperty("description").GetString().Should().NotBeNullOrWhiteSpace();

        var paths = root
            .GetProperty("paths")
            .EnumerateObject()
            .Select(path => path.Name)
            .ToList();

        paths.Should().Contain("/api/kina");
        paths.Should().Contain("/api/ulaznice");
        paths.Should().OnlyContain(path =>
            path.StartsWith("/api/", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public async Task SwaggerUi_IsPubliclyAvailable()
    {
        var response = await _client.GetAsync("/swagger/index.html");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        (await response.Content.ReadAsStringAsync()).Should().Contain("KinoKlik API");
    }
}
