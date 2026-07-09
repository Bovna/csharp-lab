using System.Globalization;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vjezba.DAL;
using Vjezba.Web.ViewModels;

namespace Vjezba.Web.Controllers;

[Route("global-search")]
[AllowAnonymous]
public sealed class GlobalSearchController : Controller
{
    private const int MinQueryLength = 2;
    private const int PerCategoryLimit = 5;
    private const int MaxTotalResults = 20;
    private const int ResultsPageCategoryLimit = 25;
    private const int ResultsPageMaxTotal = 100;

    private readonly CinemaDbContext _dbContext;

    public GlobalSearchController(CinemaDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet("")]
    public IActionResult Index(string? query)
    {
        var normalizedQuery = (query ?? string.Empty).Trim();

        if (normalizedQuery.Length < MinQueryLength)
        {
            return Json(new
            {
                query = normalizedQuery,
                minQueryLength = MinQueryLength,
                total = 0,
                results = Array.Empty<GlobalSearchResultViewModel>()
            });
        }

        var limitedResults = BuildResults(normalizedQuery, PerCategoryLimit, MaxTotalResults);

        return Json(new
        {
            query = normalizedQuery,
            minQueryLength = MinQueryLength,
            total = limitedResults.Count,
            results = limitedResults
        });
    }

    [HttpGet("rezultati")]
    public IActionResult Results(string? query, bool partial = false)
    {
        var model = BuildPageViewModel(query);

        return partial
            ? PartialView("_ResultsContent", model)
            : View(model);
    }

    private GlobalSearchPageViewModel BuildPageViewModel(string? query)
    {
        var normalizedQuery = (query ?? string.Empty).Trim();
        IReadOnlyList<GlobalSearchResultViewModel> results = normalizedQuery.Length < MinQueryLength
            ? Array.Empty<GlobalSearchResultViewModel>()
            : BuildResults(normalizedQuery, ResultsPageCategoryLimit, ResultsPageMaxTotal);

        return new GlobalSearchPageViewModel
        {
            Query = normalizedQuery,
            MinQueryLength = MinQueryLength,
            Results = results
        };
    }

    private List<GlobalSearchResultViewModel> BuildResults(
        string query,
        int perCategoryLimit,
        int maxTotalResults)
    {
        var results = new List<GlobalSearchResultViewModel>();

        results.AddRange(SearchPages(query, perCategoryLimit));
        results.AddRange(SearchMovies(query, perCategoryLimit));
        results.AddRange(SearchCinemas(query, perCategoryLimit));
        results.AddRange(SearchScreenings(query, perCategoryLimit));

        return results
            .Take(maxTotalResults)
            .ToList();
    }

    private IEnumerable<GlobalSearchResultViewModel> SearchPages(string query, int limit)
    {
        var comparableQuery = NormalizeForCompare(query);

        return BuildPageDefinitions()
            .Where(page => MatchesQuery(comparableQuery, page.Title, page.Description))
            .Take(limit)
            .Select(page => new GlobalSearchResultViewModel
            {
                Category = "Stranice",
                Kind = "page",
                Badge = "Stranica",
                Title = page.Title,
                Description = page.Description,
                Meta = string.Empty,
                Url = Url.Action(page.Action, page.Controller) ?? page.FallbackUrl
            });
    }

    private List<GlobalSearchResultViewModel> SearchMovies(string query, int limit)
    {
        var comparableQuery = NormalizeForCompare(query);

        var movies = _dbContext.Movies
            .AsNoTracking()
            .Where(movie => movie.DeletedAt == null)
            .Select(movie => new
            {
                movie.Id,
                movie.Title,
                movie.Description,
                movie.Language,
                movie.DurationMinutes
            })
            .ToList()
            .Where(movie => MatchesQuery(
                comparableQuery,
                movie.Title,
                movie.Description,
                movie.Language))
            .OrderBy(movie => movie.Title)
            .Take(limit)
            .ToList();

        return movies
            .Select(movie => new GlobalSearchResultViewModel
            {
                Category = "Filmovi",
                Kind = "data",
                Badge = "Film",
                Title = movie.Title,
                Description = movie.Description,
                Meta = $"{movie.Language} - {movie.DurationMinutes} min",
                Url = Url.Action(nameof(MovieController.Details), "Movie", new { id = movie.Id })
                    ?? Url.Action(nameof(MovieController.Search), "Movie", new { query = movie.Title })
                    ?? "/filmovi/pretraga"
            })
            .ToList();
    }

    private List<GlobalSearchResultViewModel> SearchCinemas(string query, int limit)
    {
        var comparableQuery = NormalizeForCompare(query);

        var cinemas = _dbContext.Cinemas
            .AsNoTracking()
            .Where(cinema => cinema.DeletedAt == null)
            .Select(cinema => new
            {
                cinema.Id,
                cinema.Name,
                cinema.City,
                cinema.Street,
                cinema.HouseNumber
            })
            .ToList()
            .Where(cinema => MatchesQuery(
                comparableQuery,
                cinema.Name,
                cinema.City,
                cinema.Street))
            .OrderBy(cinema => cinema.Name)
            .Take(limit)
            .ToList();

        return cinemas
            .Select(cinema => new GlobalSearchResultViewModel
            {
                Category = "Kina",
                Kind = "data",
                Badge = "Kino",
                Title = cinema.Name,
                Description = $"{cinema.Street} {cinema.HouseNumber}, {cinema.City}",
                Meta = cinema.City,
                Url = Url.Action(nameof(CinemaController.Details), "Cinema", new { id = cinema.Id })
                    ?? Url.Action(nameof(CinemaController.Search), "Cinema", new { query = cinema.Name })
                    ?? "/kina"
            })
            .ToList();
    }

    private List<GlobalSearchResultViewModel> SearchScreenings(string query, int limit)
    {
        var comparableQuery = NormalizeForCompare(query);

        var screenings = _dbContext.Screenings
            .AsNoTracking()
            .Where(screening => screening.DeletedAt == null
                && screening.Movie.DeletedAt == null
                && screening.Hall.DeletedAt == null
                && screening.Hall.Cinema.DeletedAt == null)
            .Select(screening => new
            {
                screening.Id,
                screening.StartTime,
                screening.Is3D,
                MovieTitle = screening.Movie.Title,
                HallName = screening.Hall.Name,
                CinemaName = screening.Hall.Cinema.Name
            })
            .ToList()
            .Where(screening => MatchesQuery(
                comparableQuery,
                screening.MovieTitle,
                screening.CinemaName))
            .OrderBy(screening => screening.StartTime)
            .Take(limit)
            .ToList();

        return screenings
            .Select(screening => new GlobalSearchResultViewModel
            {
                Category = "Projekcije",
                Kind = "data",
                Badge = "Projekcija",
                Title = screening.MovieTitle,
                Description = $"{screening.CinemaName} / {screening.HallName}",
                Meta = $"{screening.StartTime:dd.MM.yyyy HH:mm} - {(screening.Is3D ? "3D" : "2D")}",
                Url = Url.Action(nameof(ScreeningController.Details), "Screening", new { id = screening.Id })
                    ?? Url.Action(nameof(ScreeningController.Search), "Screening", new { query = screening.MovieTitle })
                    ?? "/projekcije/pretraga"
            })
            .ToList();
    }

    private static IReadOnlyList<PageSearchDefinition> BuildPageDefinitions()
    {
        return new[]
        {
            new PageSearchDefinition(
                "Početna",
                "Naslovnica kino sustava.",
                "Home",
                "Index",
                "/"),
            new PageSearchDefinition(
                "Filmovi",
                "Pregled dostupnih filmova.",
                "Movie",
                "Index",
                "/filmovi/pretraga"),
            new PageSearchDefinition(
                "Projekcije",
                "Pregled termina i rasporeda projekcija.",
                "Screening",
                "Index",
                "/projekcije/pretraga"),
            new PageSearchDefinition(
                "Kina",
                "Pregled kino lokacija.",
                "Cinema",
                "Index",
                "/kina"),
            new PageSearchDefinition(
                "Dvorane",
                "Pregled dvorana po kinima.",
                "Hall",
                "Index",
                "/dvorana"),
            new PageSearchDefinition(
                "Sjedala",
                "Pregled sjedala i njihovih oznaka.",
                "Seat",
                "Index",
                "/sjedala"),
            new PageSearchDefinition(
                "Brza kupnja",
                "Vodič kroz odabir kina, filma, termina i sjedala.",
                "TicketBuilder",
                "Index",
                "/TicketBuilder")
        };
    }

    private static string NormalizeForCompare(string value)
    {
        var normalized = value.Trim().ToLowerInvariant().Normalize(NormalizationForm.FormD);
        var builder = new StringBuilder(normalized.Length);

        foreach (var character in normalized)
        {
            if (CharUnicodeInfo.GetUnicodeCategory(character) != UnicodeCategory.NonSpacingMark)
            {
                builder.Append(character);
            }
        }

        return builder.ToString().Normalize(NormalizationForm.FormC);
    }

    private static bool MatchesQuery(string comparableQuery, params string?[] values)
    {
        return values
            .Where(value => !string.IsNullOrWhiteSpace(value))
            .Select(value => NormalizeForCompare(value!))
            .Any(value => value.Contains(comparableQuery));
    }

    private sealed class PageSearchDefinition
    {
        public PageSearchDefinition(
            string title,
            string description,
            string controller,
            string action,
            string fallbackUrl)
        {
            Title = title;
            Description = description;
            Controller = controller;
            Action = action;
            FallbackUrl = fallbackUrl;
        }

        public string Title { get; }
        public string Description { get; }
        public string Controller { get; }
        public string Action { get; }
        public string FallbackUrl { get; }
    }

}
