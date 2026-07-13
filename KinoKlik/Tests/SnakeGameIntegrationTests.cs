using System.Net;
using FluentAssertions;

namespace KinoKlik.Tests;

public sealed class SnakeGameIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

    public SnakeGameIntegrationTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task SharedLayout_LoadsSnakeGameAssets()
    {
        var response = await _factory.CreateClient().GetAsync("/");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var html = await response.Content.ReadAsStringAsync();
        html.Should().Contain("/css/components/snake-game.css");
        html.Should().Contain("/js/ui/snakeGame.js");
        html.Should().Contain("/js/ui/globalSearch.js");
    }

    [Fact]
    public async Task SnakeGameAssets_AreServed_WithSecretSearchContract()
    {
        using var client = _factory.CreateClient();
        using var searchResponse = await client.GetAsync("/js/ui/globalSearch.js");
        using var gameResponse = await client.GetAsync("/js/ui/snakeGame.js");
        using var styleResponse = await client.GetAsync("/css/components/snake-game.css");

        searchResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        gameResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        styleResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var searchScript = await searchResponse.Content.ReadAsStringAsync();
        var gameScript = await gameResponse.Content.ReadAsStringAsync();
        var styles = await styleResponse.Content.ReadAsStringAsync();

        searchScript.Should().Contain("const SECRET_QUERY = \"bruh\"");
        searchScript.Should().Contain("window.KinoKlikSnake.open()");
        gameScript.Should().Contain("window.KinoKlikSnake");
        gameScript.Should().Contain("aria-modal");
        styles.Should().Contain("body > .snake-game.snake-game");
    }
}
