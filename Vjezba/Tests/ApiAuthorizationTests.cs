using System.Net;
using System.Net.Http.Json;
using FluentAssertions;

namespace Vjezba.Tests;

public sealed class ApiAuthorizationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

    public ApiAuthorizationTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }

    public static IEnumerable<object[]> PublicListEndpoints()
    {
        yield return new object[] { "/api/kina" };
        yield return new object[] { "/api/kupci" };
        yield return new object[] { "/api/dvorane" };
        yield return new object[] { "/api/film" };
        yield return new object[] { "/api/projekcije" };
        yield return new object[] { "/api/sjedala" };
        yield return new object[] { "/api/ulaznice" };
    }

    public static IEnumerable<object[]> AuthenticatedDetailEndpoints()
    {
        yield return new object[] { "/api/kina/9999" };
        yield return new object[] { "/api/kupci/9999" };
        yield return new object[] { "/api/dvorane/9999" };
        yield return new object[] { "/api/film/9999" };
        yield return new object[] { "/api/projekcije/9999" };
        yield return new object[] { "/api/sjedala/9999" };
        yield return new object[] { "/api/ulaznice/9999" };
    }

    public static IEnumerable<object[]> ManagerMutatingEndpoints()
    {
        yield return new object[] { HttpMethod.Post, "/api/kina" };
        yield return new object[] { HttpMethod.Put, "/api/kina/9999" };
        yield return new object[] { HttpMethod.Post, "/api/kupci" };
        yield return new object[] { HttpMethod.Put, "/api/kupci/9999" };
        yield return new object[] { HttpMethod.Post, "/api/dvorane" };
        yield return new object[] { HttpMethod.Put, "/api/dvorane/9999" };
        yield return new object[] { HttpMethod.Post, "/api/film" };
        yield return new object[] { HttpMethod.Put, "/api/film/9999" };
        yield return new object[] { HttpMethod.Post, "/api/projekcije" };
        yield return new object[] { HttpMethod.Put, "/api/projekcije/9999" };
        yield return new object[] { HttpMethod.Post, "/api/sjedala" };
        yield return new object[] { HttpMethod.Put, "/api/sjedala/9999" };
        yield return new object[] { HttpMethod.Post, "/api/ulaznice" };
        yield return new object[] { HttpMethod.Put, "/api/ulaznice/9999" };
    }

    public static IEnumerable<object[]> AdminOnlyDeleteEndpoints()
    {
        yield return new object[] { "/api/kina/9999" };
        yield return new object[] { "/api/kupci/9999" };
        yield return new object[] { "/api/dvorane/9999" };
        yield return new object[] { "/api/film/9999" };
        yield return new object[] { "/api/projekcije/9999" };
        yield return new object[] { "/api/sjedala/9999" };
        yield return new object[] { "/api/ulaznice/9999" };
    }

    [Theory]
    [MemberData(nameof(PublicListEndpoints))]
    public async Task ListEndpoints_ReturnOk_WhenUserIsNotAuthenticated(string endpoint)
    {
        await _factory.ClearDatabaseAsync();

        var response = await _factory.CreateClient().GetAsync(endpoint);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Theory]
    [MemberData(nameof(AuthenticatedDetailEndpoints))]
    public async Task DetailEndpoints_ReturnUnauthorized_WhenUserIsNotAuthenticated(string endpoint)
    {
        var response = await _factory.CreateClient().GetAsync(endpoint);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Theory]
    [MemberData(nameof(ManagerMutatingEndpoints))]
    public async Task ManagerMutatingEndpoints_ReturnForbidden_WhenUserHasNoRole(HttpMethod method, string endpoint)
    {
        var client = _factory.CreateAuthenticatedClient();

        var response = await client.SendAsync(CreateJsonRequest(method, endpoint));

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Theory]
    [MemberData(nameof(AdminOnlyDeleteEndpoints))]
    public async Task DeleteEndpoints_ReturnForbidden_WhenUserIsManager(string endpoint)
    {
        var client = _factory.CreateAuthenticatedClient("Manager");

        var response = await client.DeleteAsync(endpoint);

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    private static HttpRequestMessage CreateJsonRequest(HttpMethod method, string endpoint)
    {
        return new HttpRequestMessage(method, endpoint)
        {
            Content = JsonContent.Create(new { })
        };
    }
}
