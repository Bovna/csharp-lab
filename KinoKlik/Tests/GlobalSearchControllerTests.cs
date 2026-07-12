using System.Net;
using System.Text.Json;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using KinoKlik.DAL;
using KinoKlik.Model.Entities;

namespace KinoKlik.Tests;

public sealed class GlobalSearchControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

    public GlobalSearchControllerTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GlobalSearch_ReturnsPublicPagesAndPublicData_WhenUserIsAnonymous()
    {
        await _factory.ClearDatabaseAsync();
        await CreatePublicSearchGraphAsync("Aurora");
        await CreateCustomerAsync("Aurora", "Korisnik");

        var response = await _factory.CreateClient().GetAsync("/global-search?query=Aurora");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var results = await ReadResultsAsync(response);
        var titles = results.Select(result => GetString(result, "title")).ToList();
        var categories = results.Select(result => GetString(result, "category")).ToList();

        categories.Should().Contain("Filmovi");
        categories.Should().Contain("Kina");
        categories.Should().Contain("Projekcije");
        titles.Should().Contain("Aurora Film");
        titles.Should().Contain("Aurora Kino");
        titles.Should().NotContain("Aurora Korisnik");
        categories.Should().NotContain("Kupci");
        categories.Should().NotContain("Ulaznice");
    }

    [Fact]
    public async Task GlobalSearch_UsesPublicUrls_ForAnonymousDataResults()
    {
        await _factory.ClearDatabaseAsync();
        await CreatePublicSearchGraphAsync("PublicLink");

        var response = await _factory.CreateClient().GetAsync("/global-search?query=PublicLink");
        var results = await ReadResultsAsync(response);

        var dataUrls = results
            .Where(result => GetString(result, "kind") == "data")
            .Select(result => GetString(result, "url"))
            .ToList();

        dataUrls.Should().NotBeEmpty();
        dataUrls.Should().OnlyContain(url => !url.Contains("/detalji/", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public async Task GlobalSearch_DoesNotReturnManagementPages_WhenUserHasNoRole()
    {
        await _factory.ClearDatabaseAsync();

        var response = await _factory.CreateAuthenticatedClient()
            .GetAsync("/global-search?query=kupci");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var results = await ReadResultsAsync(response);
        var titles = results.Select(result => GetString(result, "title")).ToList();
        var categories = results.Select(result => GetString(result, "category")).ToList();

        titles.Should().NotContain("Kupci");
        titles.Should().NotContain("Ulaznice");
        categories.Should().NotContain("Kupci");
        categories.Should().NotContain("Ulaznice");
    }

    [Theory]
    [InlineData("Admin")]
    [InlineData("Manager")]
    public async Task GlobalSearch_ReturnsManagementPages_WhenUserHasManagementRole(string role)
    {
        await _factory.ClearDatabaseAsync();

        var response = await _factory.CreateAuthenticatedClient(role)
            .GetAsync("/global-search?query=kupci");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var results = await ReadResultsAsync(response);

        results.Should().Contain(result =>
            GetString(result, "title") == "Kupci" &&
            GetString(result, "kind") == "page");
    }

    [Theory]
    [InlineData("Admin")]
    [InlineData("Manager")]
    public async Task GlobalSearch_ReturnsProtectedData_OnlyForManagementRoles(string role)
    {
        await _factory.ClearDatabaseAsync();
        await CreateManagementSearchGraphAsync("RoleSearch");

        var managementResponse = await _factory.CreateAuthenticatedClient(role)
            .GetAsync("/global-search?query=RoleSearch");
        var anonymousResponse = await _factory.CreateClient()
            .GetAsync("/global-search?query=RoleSearch");
        var regularUserResponse = await _factory.CreateAuthenticatedClient()
            .GetAsync("/global-search?query=RoleSearch");

        var managementResults = await ReadResultsAsync(managementResponse);
        var anonymousResults = await ReadResultsAsync(anonymousResponse);
        var regularUserResults = await ReadResultsAsync(regularUserResponse);

        managementResults.Should().Contain(result => GetString(result, "category") == "Kupci");
        managementResults.Should().Contain(result => GetString(result, "category") == "Ulaznice");
        anonymousResults.Should().NotContain(result =>
            GetString(result, "category") == "Kupci" ||
            GetString(result, "category") == "Ulaznice");
        regularUserResults.Should().NotContain(result =>
            GetString(result, "category") == "Kupci" ||
            GetString(result, "category") == "Ulaznice");
    }

    [Fact]
    public async Task GlobalSearch_ReturnsOnlyNavigationPages_ForHallsAndSeats()
    {
        await _factory.ClearDatabaseAsync();
        await CreateScreeningWithHallNameAsync("Dvorana Posebna");

        var response = await _factory.CreateAuthenticatedClient("Manager")
            .GetAsync("/global-search?query=dvor");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var results = await ReadResultsAsync(response);

        results.Should().Contain(result =>
            GetString(result, "title") == "Dvorane" &&
            GetString(result, "kind") == "page" &&
            GetString(result, "badge") == "Stranica");

        results.Should().OnlyContain(result => GetString(result, "kind") == "page");
    }

    [Fact]
    public async Task GlobalSearch_DoesNotReturnManagementNavigation_ForAnonymousUser()
    {
        await _factory.ClearDatabaseAsync();

        var response = await _factory.CreateClient().GetAsync("/global-search?query=dvorane");
        var results = await ReadResultsAsync(response);

        results.Should().NotContain(result => GetString(result, "title") == "Dvorane");
    }

    [Fact]
    public async Task GlobalSearch_MatchesPageDescriptions()
    {
        await _factory.ClearDatabaseAsync();

        var response = await _factory.CreateClient().GetAsync("/global-search?query=sustava");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var results = await ReadResultsAsync(response);

        results.Should().Contain(result =>
            GetString(result, "kind") == "page" &&
            GetString(result, "title") == "Početna");
    }

    [Fact]
    public async Task GlobalSearch_DoesNotMatchPageCategoryOrBadge()
    {
        await _factory.ClearDatabaseAsync();

        var response = await _factory.CreateClient().GetAsync("/global-search?query=stranica");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var results = await ReadResultsAsync(response);

        results.Should().NotContain(result => GetString(result, "kind") == "page");
    }

    [Fact]
    public async Task GlobalSearch_PageResultsDoNotExposeNavigationMeta()
    {
        await _factory.ClearDatabaseAsync();

        var response = await _factory.CreateClient().GetAsync("/global-search?query=film");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var results = await ReadResultsAsync(response);
        var pageResult = results.Should()
            .ContainSingle(result =>
                GetString(result, "kind") == "page" &&
                GetString(result, "title") == "Filmovi")
            .Subject;

        GetString(pageResult, "meta").Should().BeEmpty();
    }

    [Fact]
    public async Task GlobalSearch_MatchesDataWithoutDiacritics()
    {
        await _factory.ClearDatabaseAsync();

        using (var scope = _factory.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<CinemaDbContext>();

            dbContext.Movies.Add(new Movie
            {
                Title = "Žar Film",
                Description = "Film s hrvatskim znakovima.",
                DurationMinutes = 100,
                ReleaseDate = new DateTime(2026, 1, 1),
                Genre = MovieGenre.Drama,
                Language = "HR",
                AgeRating = "12+"
            });

            dbContext.Cinemas.Add(new Cinema
            {
                Name = "Kino Čakovec",
                City = "Čakovec",
                Street = "Županijska",
                HouseNumber = "1",
                PostalCode = "40000",
                Email = "cakovec@example.test",
                Phone = "+385 40 111 222"
            });

            await dbContext.SaveChangesAsync();
        }

        var client = _factory.CreateClient();
        var movieResponse = await client.GetAsync("/global-search?query=Zar");
        var cinemaResponse = await client.GetAsync("/global-search?query=Cakovec");

        movieResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        cinemaResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var movieResults = await ReadResultsAsync(movieResponse);
        var cinemaResults = await ReadResultsAsync(cinemaResponse);
        var movieTitles = movieResults.Select(result => GetString(result, "title")).ToList();
        var cinemaTitles = cinemaResults.Select(result => GetString(result, "title")).ToList();

        movieTitles.Should().Contain("Žar Film");
        cinemaTitles.Should().Contain("Kino Čakovec");
    }

    [Fact]
    public async Task GlobalSearchResultsPage_ReturnsDetailedResults()
    {
        await _factory.ClearDatabaseAsync();
        await CreatePublicSearchGraphAsync("Detaljni");

        var response = await _factory.CreateClient().GetAsync("/global-search/rezultati?query=Detaljni");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var html = await response.Content.ReadAsStringAsync();
        html.Should().Contain("Rezultati pretrage");
        html.Should().Contain("Početna");
        html.Should().Contain("Pretraga");
        html.Should().NotContain("GlobalSearch");
        html.Should().NotContain("aria-current=\"page\">Results");
        html.Should().Contain("data-ajax-search-form");
        html.Should().Contain("data-ajax-sync-url=\"true\"");
        html.Should().Contain("global-search-page-results");
        ExtractFormMarkup(html, "global-search-page__form").Should().NotContain("<button");
        html.Should().Contain("Detaljni Film");
        html.Should().Contain("Detaljni Kino");
        html.Should().NotContain("global-search-page__badge");
    }

    [Fact]
    public async Task GlobalSearchResultsPage_ReturnsPartialResults_ForLiveSearch()
    {
        await _factory.ClearDatabaseAsync();
        await CreatePublicSearchGraphAsync("Uzivo");

        var response = await _factory.CreateClient().GetAsync("/global-search/rezultati?query=Uzivo&partial=true");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var html = await response.Content.ReadAsStringAsync();
        html.Should().NotContain("<!DOCTYPE html>");
        html.Should().Contain("Uzivo Film");
        html.Should().Contain("Uzivo Kino");
        html.Should().NotContain("global-search-page__badge");
    }

    [Fact]
    public async Task GlobalSearch_DoesNotReturnSoftDeletedData()
    {
        await _factory.ClearDatabaseAsync();

        using (var scope = _factory.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<CinemaDbContext>();

            dbContext.Movies.Add(new Movie
            {
                Title = "Ghost Film",
                Description = "Soft deleted movie.",
                DurationMinutes = 100,
                ReleaseDate = new DateTime(2026, 1, 1),
                Genre = MovieGenre.Drama,
                Language = "HR",
                AgeRating = "12+",
                DeletedAt = DateTime.UtcNow
            });

            dbContext.Cinemas.Add(new Cinema
            {
                Name = "Ghost Kino",
                City = "Zagreb",
                Street = "Skrivena",
                HouseNumber = "1",
                PostalCode = "10000",
                Email = "ghost@example.test",
                Phone = "+385 1 000 000",
                DeletedAt = DateTime.UtcNow
            });

            var activeCinema = new Cinema
            {
                Name = "Active Kino",
                City = "Zagreb",
                Street = "Ilica",
                HouseNumber = "1",
                PostalCode = "10000",
                Email = "active@example.test",
                Phone = "+385 1 111 111"
            };
            var activeMovie = new Movie
            {
                Title = "Active Film",
                Description = "Visible movie.",
                DurationMinutes = 110,
                ReleaseDate = new DateTime(2026, 1, 1),
                Genre = MovieGenre.Action,
                Language = "EN",
                AgeRating = "12+"
            };
            var hall = new Hall
            {
                Name = "Ghost Hall",
                Capacity = 40,
                Supports3D = false,
                Cinema = activeCinema
            };

            dbContext.Screenings.Add(new Screening
            {
                Movie = activeMovie,
                Hall = hall,
                StartTime = new DateTime(2026, 9, 1, 18, 0, 0),
                EndTime = new DateTime(2026, 9, 1, 20, 0, 0),
                Is3D = false,
                DeletedAt = DateTime.UtcNow
            });

            await dbContext.SaveChangesAsync();
        }

        var movieResponse = await _factory.CreateClient().GetAsync("/global-search?query=Ghost");
        var movieResults = await ReadResultsAsync(movieResponse);

        movieResults.Should().NotContain(result => GetString(result, "kind") == "data");
    }

    [Fact]
    public async Task GlobalSearch_DoesNotReturnSoftDeletedProtectedData()
    {
        await _factory.ClearDatabaseAsync();

        using (var scope = _factory.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<CinemaDbContext>();
            var deletedAt = DateTime.UtcNow;
            var customer = new Customer
            {
                FirstName = "DeletedSensitive",
                LastName = "Kupac",
                City = "Zagreb",
                Street = "Skrivena",
                HouseNumber = "1",
                PostalCode = "10000",
                Email = "deleted.sensitive@example.test",
                Phone = "+385 99 000 000",
                RegisteredAt = new DateTime(2026, 1, 1),
                DeletedAt = deletedAt
            };

            dbContext.Customers.Add(customer);
            await dbContext.SaveChangesAsync();
        }

        var response = await _factory.CreateAuthenticatedClient("Manager")
            .GetAsync("/global-search?query=DeletedSensitive");
        var results = await ReadResultsAsync(response);

        results.Should().NotContain(result => GetString(result, "category") == "Kupci");
        results.Should().NotContain(result => GetString(result, "category") == "Ulaznice");
    }

    [Fact]
    public async Task GlobalSearch_RequiresTwoCharacters()
    {
        await _factory.ClearDatabaseAsync();
        await CreatePublicSearchGraphAsync("Astra");

        var response = await _factory.CreateClient().GetAsync("/global-search?query=A");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var results = await ReadResultsAsync(response);

        results.Should().BeEmpty();
    }

    [Fact]
    public async Task GlobalSearch_LimitsMovieResultsPerCategory()
    {
        await _factory.ClearDatabaseAsync();

        using (var scope = _factory.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<CinemaDbContext>();

            for (var i = 1; i <= 8; i++)
            {
                dbContext.Movies.Add(new Movie
                {
                    Title = $"Limit Film {i}",
                    Description = "Movie used to verify global search limits.",
                    DurationMinutes = 90 + i,
                    ReleaseDate = new DateTime(2026, 1, i),
                    Genre = MovieGenre.Action,
                    Language = "EN",
                    AgeRating = "12+"
                });
            }

            await dbContext.SaveChangesAsync();
        }

        var response = await _factory.CreateClient().GetAsync("/global-search?query=Limit");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var results = await ReadResultsAsync(response);
        var movieResults = results
            .Where(result => GetString(result, "category") == "Filmovi")
            .ToList();

        movieResults.Should().HaveCountLessThanOrEqualTo(5);
    }

    private async Task CreatePublicSearchGraphAsync(string prefix)
    {
        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<CinemaDbContext>();

        var cinema = new Cinema
        {
            Name = $"{prefix} Kino",
            City = "Zagreb",
            Street = "Testna",
            HouseNumber = "10",
            PostalCode = "10000",
            Email = $"{prefix.ToLowerInvariant()}@example.test",
            Phone = "+385 1 123 456"
        };
        var hall = new Hall
        {
            Name = $"{prefix} Dvorana",
            Capacity = 80,
            Supports3D = true,
            Cinema = cinema
        };
        var movie = new Movie
        {
            Title = $"{prefix} Film",
            Description = $"{prefix} opis filma.",
            DurationMinutes = 120,
            ReleaseDate = new DateTime(2026, 1, 1),
            Genre = MovieGenre.SciFi,
            Language = "EN",
            AgeRating = "12+"
        };

        dbContext.Screenings.Add(new Screening
        {
            Movie = movie,
            Hall = hall,
            StartTime = new DateTime(2026, 9, 1, 18, 0, 0),
            EndTime = new DateTime(2026, 9, 1, 20, 0, 0),
            Is3D = true
        });

        await dbContext.SaveChangesAsync();
    }

    private async Task CreateCustomerAsync(string firstName, string lastName)
    {
        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<CinemaDbContext>();

        dbContext.Customers.Add(new Customer
        {
            FirstName = firstName,
            LastName = lastName,
            City = "Zagreb",
            Street = "Testna",
            HouseNumber = "1",
            PostalCode = "10000",
            Email = $"{firstName.ToLowerInvariant()}.{lastName.ToLowerInvariant()}@example.test",
            Phone = "+385 99 111 222",
            RegisteredAt = new DateTime(2026, 1, 1),
            IsLoyaltyMember = false,
            LoyaltyPoints = 0
        });

        await dbContext.SaveChangesAsync();
    }

    private async Task CreateScreeningWithHallNameAsync(string hallName)
    {
        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<CinemaDbContext>();

        var cinema = new Cinema
        {
            Name = "Neutral Kino",
            City = "Zagreb",
            Street = "Neutralna",
            HouseNumber = "2",
            PostalCode = "10000",
            Email = "neutral@example.test",
            Phone = "+385 1 222 333"
        };
        var hall = new Hall
        {
            Name = hallName,
            Capacity = 60,
            Supports3D = false,
            Cinema = cinema
        };
        var movie = new Movie
        {
            Title = "Neutral Film",
            Description = "Movie whose title does not match the hall query.",
            DurationMinutes = 100,
            ReleaseDate = new DateTime(2026, 1, 1),
            Genre = MovieGenre.Drama,
            Language = "HR",
            AgeRating = "12+"
        };

        dbContext.Screenings.Add(new Screening
        {
            Movie = movie,
            Hall = hall,
            StartTime = new DateTime(2026, 9, 1, 19, 0, 0),
            EndTime = new DateTime(2026, 9, 1, 21, 0, 0),
            Is3D = false
        });

        await dbContext.SaveChangesAsync();
    }

    private async Task CreateManagementSearchGraphAsync(string prefix)
    {
        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<CinemaDbContext>();

        var cinema = new Cinema
        {
            Name = "Management Kino",
            City = "Zagreb",
            Street = "Upraviteljska",
            HouseNumber = "1",
            PostalCode = "10000",
            Email = "management@example.test",
            Phone = "+385 1 555 000"
        };
        var hall = new Hall
        {
            Name = "Management Dvorana",
            Capacity = 40,
            Supports3D = false,
            Cinema = cinema
        };
        var movie = new Movie
        {
            Title = "Management Film",
            Description = "Film za autorizacijski test.",
            DurationMinutes = 95,
            ReleaseDate = new DateTime(2026, 1, 1),
            Genre = MovieGenre.Drama,
            Language = "HR",
            AgeRating = "12+"
        };
        var screening = new Screening
        {
            Movie = movie,
            Hall = hall,
            StartTime = new DateTime(2026, 9, 1, 18, 0, 0),
            EndTime = new DateTime(2026, 9, 1, 20, 0, 0),
            Is3D = false
        };
        var customer = new Customer
        {
            FirstName = prefix,
            LastName = "Kupac",
            City = "Zagreb",
            Street = "Testna",
            HouseNumber = "1",
            PostalCode = "10000",
            Email = $"{prefix.ToLowerInvariant()}@example.test",
            Phone = "+385 99 555 000",
            RegisteredAt = new DateTime(2026, 1, 1)
        };

        dbContext.Tickets.Add(new Ticket
        {
            TicketNumber = $"{prefix}-001",
            PurchasedAt = new DateTime(2026, 1, 2),
            Price = 10,
            Status = TicketStatus.Active,
            Screening = screening,
            Customer = customer
        });

        await dbContext.SaveChangesAsync();
    }

    private static async Task<List<JsonElement>> ReadResultsAsync(HttpResponseMessage response)
    {
        var json = await response.Content.ReadAsStringAsync();
        using var document = JsonDocument.Parse(json);

        var results = document.RootElement.TryGetProperty("results", out var camelResults)
            ? camelResults
            : document.RootElement.GetProperty("Results");

        return results
            .EnumerateArray()
            .Select(result => result.Clone())
            .ToList();
    }

    private static string ExtractFormMarkup(string html, string formClass)
    {
        var classIndex = html.IndexOf(formClass, StringComparison.Ordinal);
        classIndex.Should().BeGreaterThanOrEqualTo(0);

        var formStart = html.LastIndexOf("<form", classIndex, StringComparison.Ordinal);
        var formEnd = html.IndexOf("</form>", classIndex, StringComparison.Ordinal);

        formStart.Should().BeGreaterThanOrEqualTo(0);
        formEnd.Should().BeGreaterThan(formStart);

        return html[formStart..(formEnd + "</form>".Length)];
    }

    private static string GetString(JsonElement element, string propertyName)
    {
        if (element.TryGetProperty(propertyName, out var property))
        {
            return property.GetString() ?? string.Empty;
        }

        var pascalPropertyName = char.ToUpperInvariant(propertyName[0]) + propertyName[1..];
        return element.TryGetProperty(pascalPropertyName, out property)
            ? property.GetString() ?? string.Empty
            : string.Empty;
    }
}
