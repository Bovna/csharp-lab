using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vjezba.DAL;
using Vjezba.Model.Entities;
using Vjezba.Web.ViewModels;

namespace Vjezba.Web.Controllers;

[Route("filmovi")]
public class MovieController : Controller
{
    private readonly CinemaDbContext _dbContext;

    public MovieController(CinemaDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [Route("pretraga")]
    public IActionResult Index(string? language, string? search, bool partial = false)
    {
        var normalizedSearch = (search ?? string.Empty).Trim();
        var query = _dbContext.Movies.Where(m => m.DeletedAt == null).AsQueryable();

        ViewBag.Languages = _dbContext.Movies
            .Where(m => m.DeletedAt == null)
            .Select(m => m.Language)
            .Where(l => l != null && l != "")
            .Distinct()
            .OrderBy(l => l)
            .ToList();

        ViewBag.SelectedLanguage = language;
        ViewBag.Search = search;

        if (!string.IsNullOrWhiteSpace(language))
        {
            query = query.Where(m => m.Language == language);
        }

        if (!string.IsNullOrWhiteSpace(normalizedSearch))
        {
            query = query.Where(movie => EF.Functions.Like(movie.Title, $"%{normalizedSearch}%"));
        }

        var movies = query.ToList();

        if (partial)
        {
            return PartialView("_IndexResults", movies);
        }

        return View(movies);
    }

    public IActionResult Search(string? query)
    {
        var normalizedQuery = (query ?? string.Empty).Trim();

        var movies = _dbContext.Movies
            .Where(movie => movie.DeletedAt == null)
            .Where(movie => string.IsNullOrEmpty(normalizedQuery)
                || EF.Functions.Like(movie.Title, $"%{normalizedQuery}%"))
            .OrderBy(movie => movie.Title)
            .Take(12)
            .Select(movie => new
            {
                value = movie.Id,
                text = movie.Title
            })
            .ToList();

        return Json(movies);
    }

    [Route("detalji/{id}")]
    public IActionResult Details(int id)
    {
        var movie = _dbContext.Movies.FirstOrDefault(m => m.Id == id && m.DeletedAt == null);

        if (movie is null)
        {
            return NotFound();
        }

        return View(movie);
    }

    [Route("dodaj")]
    public IActionResult Create()
    {
        return View(new MovieFormViewModel());
    }

    [HttpPost("dodaj")]
    public IActionResult Create(MovieFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var movie = new Movie
        {
            Title = model.Title,
            Description = model.Description,
            DurationMinutes = model.DurationMinutes,
            ReleaseDate = model.ReleaseDate,
            Genre = model.Genre,
            Language = model.Language,
            AgeRating = model.AgeRating
        };

        _dbContext.Movies.Add(movie);
        _dbContext.SaveChanges();

        return RedirectToAction(nameof(Index));
    }

    [Route("uredi/{id}")]
    [ActionName("Edit")]
    public IActionResult EditGet(int id)
    {
        var movie = _dbContext.Movies.FirstOrDefault(m => m.Id == id && m.DeletedAt == null);
        if (movie is null)
        {
            return NotFound();
        }

        var model = new MovieFormViewModel
        {
            Id = movie.Id,
            Title = movie.Title,
            Description = movie.Description,
            DurationMinutes = movie.DurationMinutes,
            ReleaseDate = movie.ReleaseDate,
            Genre = movie.Genre,
            Language = movie.Language,
            AgeRating = movie.AgeRating
        };

        return View(model);
    }

    [HttpPost("uredi/{id}")]
    [ActionName("Edit")]
    public async Task<IActionResult> EditPost(int id)
    {
        var movie = _dbContext.Movies.FirstOrDefault(m => m.Id == id && m.DeletedAt == null);

        if (movie is null)
        {
            return NotFound();
        }

        var ok = await TryUpdateModelAsync(movie, "", m => m.Title, m => m.Description, m => m.DurationMinutes,
            m => m.ReleaseDate, m => m.Genre, m => m.Language, m => m.AgeRating);

        if (ok && ModelState.IsValid)
        {
            _dbContext.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        var model = new MovieFormViewModel
        {
            Id = movie.Id,
            Title = movie.Title,
            Description = movie.Description,
            DurationMinutes = movie.DurationMinutes,
            ReleaseDate = movie.ReleaseDate,
            Genre = movie.Genre,
            Language = movie.Language,
            AgeRating = movie.AgeRating
        };

        return View(model);
    }

    [HttpPost("obrisi/{id}")]
    public IActionResult Delete(int id)
    {
        var movie = _dbContext.Movies.FirstOrDefault(m => m.Id == id && m.DeletedAt == null);
        if (movie is null)
        {
            return NotFound();
        }

        movie.DeletedAt = DateTime.UtcNow;

        SoftDeleteMovie(movie);
        _dbContext.SaveChanges();

        return RedirectToAction(nameof(Index));
    }

    private void SoftDeleteMovie(Movie movie)
    {
        var deletedAt = movie.DeletedAt ?? DateTime.UtcNow;
        movie.DeletedAt = deletedAt;

        var screenings = _dbContext.Screenings
            .Where(screening => screening.MovieId == movie.Id && screening.DeletedAt == null)
            .ToList();

        foreach (var screening in screenings)
        {
            screening.DeletedAt = deletedAt;
        }

        var screeningIds = screenings.Select(screening => screening.Id).ToList();

        var tickets = _dbContext.Tickets
            .Where(ticket => ticket.DeletedAt == null && screeningIds.Contains(ticket.ScreeningId))
            .ToList();

        foreach (var ticket in tickets)
        {
            ticket.DeletedAt = deletedAt;
        }

        var favorites = _dbContext.CustomerFavoriteMovies
            .Where(favorite => favorite.MovieId == movie.Id && favorite.DeletedAt == null)
            .ToList();

        foreach (var favorite in favorites)
        {
            favorite.DeletedAt = deletedAt;
        }
    }
}
