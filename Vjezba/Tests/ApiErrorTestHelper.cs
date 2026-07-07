using System.Text.Json;

namespace Vjezba.Tests;

internal static class ApiErrorTestHelper
{
    public static async Task<string> ReadErrorMessageAsync(HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();
        using var json = JsonDocument.Parse(content);

        return json.RootElement.TryGetProperty("error", out var error)
            ? error.GetString() ?? string.Empty
            : string.Empty;
    }
}
