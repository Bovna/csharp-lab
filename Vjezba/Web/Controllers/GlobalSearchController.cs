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
    private const string AdminRole = "Admin";
    private const string ManagerRole = "Manager";

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
        var canViewManagementResults = CanViewManagementResults();

        results.AddRange(SearchPages(query, perCategoryLimit, canViewManagementResults));
        results.AddRange(SearchMovies(query, perCategoryLimit));
        results.AddRange(SearchCinemas(query, perCategoryLimit));
        results.AddRange(SearchScreenings(query, perCategoryLimit));

        if (canViewManagementResults)
        {
            results.AddRange(SearchCustomers(query, perCategoryLimit));
            results.AddRange(SearchTickets(query, perCategoryLimit));
        }

        return results
            .Take(maxTotalResults)
            .ToList();
    }

    private IEnumerable<GlobalSearchResultViewModel> SearchPages(
        string query,
        int limit,
        bool canViewManagementResults)
    {
        var comparableQuery = NormalizeForCompare(query);

        return BuildPageDefinitions(canViewManagementResults)
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
                Url = BuildAccessibleDataUrl(
                    nameof(MovieController.Details),
                    "Movie",
                    movie.Id,
                    nameof(MovieController.Search),
                    movie.Title,
                    "/filmovi/pretraga")
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
                Url = BuildAccessibleDataUrl(
                    nameof(CinemaController.Details),
                    "Cinema",
                    cinema.Id,
                    nameof(CinemaController.Search),
                    cinema.Name,
                    "/kina")
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
                Url = BuildAccessibleDataUrl(
                    nameof(ScreeningController.Details),
                    "Screening",
                    screening.Id,
                    nameof(ScreeningController.Search),
                    screening.MovieTitle,
                    "/projekcije/pretraga")
            })
            .ToList();
    }

    private List<GlobalSearchResultViewModel> SearchCustomers(string query, int limit)
    {
        var comparableQuery = NormalizeForCompare(query);

        var customers = _dbContext.Customers
            .AsNoTracking()
            .Where(customer => customer.DeletedAt == null)
            .Select(customer => new
            {
                customer.Id,
                customer.FirstName,
                customer.LastName,
                customer.City,
                customer.Email,
                customer.IsLoyaltyMember
            })
            .ToList()
            .Where(customer => MatchesQuery(
                comparableQuery,
                $"{customer.FirstName} {customer.LastName}",
                customer.City,
                customer.Email))
            .OrderBy(customer => customer.LastName)
            .ThenBy(customer => customer.FirstName)
            .Take(limit)
            .ToList();

        return customers
            .Select(customer => new GlobalSearchResultViewModel
            {
                Category = "Kupci",
                Kind = "data",
                Badge = "Kupac",
                Title = $"{customer.FirstName} {customer.LastName}",
                Description = $"{customer.Email} - {customer.City}",
                Meta = customer.IsLoyaltyMember ? "Loyalty član" : "Standardni kupac",
                Url = Url.Action(nameof(CustomerController.Details), "Customer", new { id = customer.Id })
                    ?? "/kupci"
            })
            .ToList();
    }

    private List<GlobalSearchResultViewModel> SearchTickets(string query, int limit)
    {
        var comparableQuery = NormalizeForCompare(query);

        var tickets = _dbContext.Tickets
            .AsNoTracking()
            .Where(ticket => ticket.DeletedAt == null
                && ticket.Customer.DeletedAt == null
                && ticket.Screening.DeletedAt == null
                && ticket.Screening.Movie.DeletedAt == null
                && ticket.Screening.Hall.DeletedAt == null
                && ticket.Screening.Hall.Cinema.DeletedAt == null
                && (ticket.Seat == null
                    || (ticket.Seat.DeletedAt == null
                        && ticket.Seat.Hall.DeletedAt == null
                        && ticket.Seat.Hall.Cinema.DeletedAt == null)))
            .Select(ticket => new
            {
                ticket.Id,
                ticket.TicketNumber,
                ticket.Status,
                ticket.PurchasedAt,
                CustomerName = ticket.Customer.FirstName + " " + ticket.Customer.LastName,
                MovieTitle = ticket.Screening.Movie.Title,
                HallName = ticket.Screening.Hall.Name,
                CinemaName = ticket.Screening.Hall.Cinema.Name
            })
            .ToList()
            .Where(ticket => MatchesQuery(
                comparableQuery,
                ticket.TicketNumber,
                ticket.CustomerName,
                ticket.MovieTitle,
                ticket.HallName,
                ticket.CinemaName))
            .OrderByDescending(ticket => ticket.PurchasedAt)
            .Take(limit)
            .ToList();

        return tickets
            .Select(ticket => new GlobalSearchResultViewModel
            {
                Category = "Ulaznice",
                Kind = "data",
                Badge = "Ulaznica",
                Title = ticket.TicketNumber,
                Description = $"{ticket.CustomerName} - {ticket.MovieTitle}",
                Meta = $"{ticket.CinemaName} / {ticket.HallName} - {ticket.Status}",
                Url = Url.Action(nameof(TicketController.Details), "Ticket", new { id = ticket.Id })
                    ?? "/ulaznice"
            })
            .ToList();
    }

    private string BuildAccessibleDataUrl(
        string detailsAction,
        string controller,
        int id,
        string searchAction,
        string query,
        string fallbackUrl)
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            return Url.Action(detailsAction, controller, new { id })
                ?? fallbackUrl;
        }

        return Url.Action(searchAction, controller, new { query })
            ?? fallbackUrl;
    }

    private bool CanViewManagementResults()
    {
        return User.IsInRole(AdminRole) || User.IsInRole(ManagerRole);
    }

    private static IReadOnlyList<PageSearchDefinition> BuildPageDefinitions(bool includeManagementPages)
    {
        var pages = new List<PageSearchDefinition>
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
                "Brza kupnja",
                "Vodič kroz odabir kina, filma, termina i sjedala.",
                "TicketBuilder",
                "Index",
                "/TicketBuilder")
        };

        if (includeManagementPages)
        {
            pages.AddRange(new[]
            {
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
                    "Kupci",
                    "Pregled i upravljanje kupcima.",
                    "Customer",
                    "Index",
                    "/kupci"),
                new PageSearchDefinition(
                    "Ulaznice",
                    "Pregled i upravljanje ulaznicama.",
                    "Ticket",
                    "Index",
                    "/ulaznice")
            });
        }

        return pages;
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
