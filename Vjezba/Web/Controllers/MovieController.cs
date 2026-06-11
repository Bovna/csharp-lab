using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vjezba.DAL;
using Vjezba.Model.Entities;
using Vjezba.Web.ViewModels;

namespace Vjezba.Web.Controllers;

[Route("filmovi")]
[Authorize]
public class MovieController : BaseController
{
    private readonly CinemaDbContext _dbContext;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public MovieController(
        CinemaDbContext dbContext,
        UserManager<AppUser> userManager,
        IWebHostEnvironment webHostEnvironment)
        : base(userManager)
    {
        _dbContext = dbContext;
        _webHostEnvironment = webHostEnvironment;
    }

    [Route("pretraga")]
    [AllowAnonymous]
    public IActionResult Index(string? language, bool partial = false)
    {
        var query = _dbContext.Movies.Where(m => m.DeletedAt == null).AsQueryable();

        ViewBag.Languages = _dbContext.Movies
            .Where(m => m.DeletedAt == null)
            .Select(m => m.Language)
            .Where(l => l != null && l != "")
            .Distinct()
            .OrderBy(l => l)
            .ToList();

        ViewBag.SelectedLanguage = language;
        ViewBag.Search = null;

        if (!string.IsNullOrWhiteSpace(language))
        {
            query = query.Where(m => m.Language == language);
        }

        var movies = query.ToList();
        movies = movies.OrderBy(movie => movie.Id).ToList();

        if (partial)
        {
            return PartialView("_IndexResults", movies);
        }

        return View(movies);
    }

    [AllowAnonymous]
    public IActionResult Search(string? query, string? language, bool partial = false)
    {
        var normalizedQuery = (query ?? string.Empty).Trim();

        ViewBag.Languages = _dbContext.Movies
            .Where(m => m.DeletedAt == null)
            .Select(m => m.Language)
            .Where(l => l != null && l != "")
            .Distinct()
            .OrderBy(l => l)
            .ToList();

        ViewBag.SelectedLanguage = null;
        ViewBag.Search = query;

        var moviesQuery = _dbContext.Movies.Where(movie => movie.DeletedAt == null).AsQueryable();

        if (!string.IsNullOrWhiteSpace(language))
        {
            moviesQuery = moviesQuery.Where(movie => movie.Language == language);
            ViewBag.SelectedLanguage = language;
        }

        if (!string.IsNullOrWhiteSpace(normalizedQuery))
        {
            moviesQuery = moviesQuery.Where(movie =>
                movie.Title.Contains(normalizedQuery) ||
                movie.Description.Contains(normalizedQuery));
        }

        var movies = moviesQuery.OrderBy(movie => movie.Id).ToList();

        if (partial)
        {
            return PartialView("_IndexResults", movies);
        }

        return View(nameof(Index), movies);
    }

    [HttpGet("autocomplete")]
    [AllowAnonymous]
    public IActionResult Autocomplete(string? query)
    {
        var normalizedQuery = (query ?? string.Empty).Trim();

        var movies = _dbContext.Movies
            .Where(movie => movie.DeletedAt == null)
            .Where(movie => string.IsNullOrEmpty(normalizedQuery)
                || movie.Title.Contains(normalizedQuery))
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
    [Authorize]
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
    [Authorize(Roles = "Admin,Manager")]
    public IActionResult Create()
    {
        return View(new MovieFormViewModel());
    }

    [HttpPost("dodaj")]
    [Authorize(Roles = "Admin,Manager")]
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
    [Authorize(Roles = "Admin,Manager")]
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
    [Authorize(Roles = "Admin,Manager")]
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
    [Authorize(Roles = "Admin")]
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

    [HttpPost("uredi/{movieId}/datoteke/objavi")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> UploadAttachment(int movieId, IFormFile? file)
    {
        var movieExists = await _dbContext.Movies.AnyAsync(movie => movie.Id == movieId && movie.DeletedAt == null);

        if (!movieExists)
        {
            return NotFound();
        }

        if (file is null || file.Length == 0)
        {
            return BadRequest();
        }

        var uploadsPath = Path.Combine(
            Directory.GetCurrentDirectory(),
            "wwwroot",
            "uploads",
            "movies",
            movieId.ToString()
        );

        Directory.CreateDirectory(uploadsPath);

        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
        var filePath = Path.Combine(uploadsPath, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        var attachment = new Attachment
        {
            MovieId = movieId,
            FileName = file.FileName,
            FilePath = "/uploads/movies/" + movieId + "/" + fileName,
            ContentType = file.ContentType,
            FileSize = file.Length,
            CreatedAt = DateTime.UtcNow
        };

        _dbContext.Attachments.Add(attachment);
        await _dbContext.SaveChangesAsync();

        return Json(new { success = true });
    }

    [HttpGet("uredi/{movieId}/datoteke")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> GetAttachments(int movieId)
    {
        var movieExists = await _dbContext.Movies.AnyAsync(movie => movie.Id == movieId && movie.DeletedAt == null);

        if (!movieExists)
        {
            return NotFound();
        }

        var attachments = await _dbContext.Attachments
            .Where(attachment => attachment.MovieId == movieId)
            .OrderByDescending(attachment => attachment.CreatedAt)
            .ToListAsync();

        return PartialView("_AttachmentList", attachments);
    }

    [HttpPost("datoteke/obrisi")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> DeleteAttachment(int id)
    {
        var attachment = await _dbContext.Attachments.FirstOrDefaultAsync(attachment => attachment.Id == id);

        if (attachment is null)
        {
            return NotFound();
        }

        var physicalPath = Path.Combine(
            Directory.GetCurrentDirectory(),
            "wwwroot",
            attachment.FilePath.TrimStart('/'));

        if (System.IO.File.Exists(physicalPath))
        {
            System.IO.File.Delete(physicalPath);
        }

        _dbContext.Attachments.Remove(attachment);
        await _dbContext.SaveChangesAsync();

        return Json(new { success = true });
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
