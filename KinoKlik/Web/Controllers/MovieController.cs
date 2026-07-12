using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KinoKlik.DAL;
using KinoKlik.Model.Entities;
using KinoKlik.Web.Services;
using KinoKlik.Web.ViewModels;

namespace KinoKlik.Web.Controllers;

[AutoValidateAntiforgeryToken]
[Route("filmovi")]
[Authorize]
public class MovieController : BaseController
{
    private static readonly string[] AllowedAgeRatings = { "U", "7+", "10+", "12+", "15+", "16+", "18+" };
    private const string AgeRatingErrorMessage = "Dobna oznaka nije ispravna. Dopuštene vrijednosti su U, 7+, 10+, 12+, 15+, 16+, 18+ ili format PG-13.";
    private const long MaxPosterFileSizeInBytes = 5 * 1024 * 1024;
    private static readonly IReadOnlyDictionary<string, string[]> AllowedPosterContentTypesByExtension =
        new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase)
        {
            [".jpg"] = new[] { "image/jpeg" },
            [".jpeg"] = new[] { "image/jpeg" },
            [".png"] = new[] { "image/png" },
            [".webp"] = new[] { "image/webp" }
        };

    private readonly CinemaDbContext _dbContext;
    private readonly IUploadStorage _uploadStorage;
    private readonly ILogger<MovieController> _logger;

    public MovieController(
        CinemaDbContext dbContext,
        UserManager<AppUser> userManager,
        IUploadStorage uploadStorage,
        ILogger<MovieController> logger)
        : base(userManager)
    {
        _dbContext = dbContext;
        _uploadStorage = uploadStorage;
        _logger = logger;
    }

    [Route("pretraga")]
    [AllowAnonymous]
    public IActionResult Index(string? language, bool management = false, bool partial = false)
    {
        var query = _dbContext.Movies
            .Include(m => m.Attachments)
            .Where(m => m.DeletedAt == null)
            .AsQueryable();

        ViewBag.Languages = _dbContext.Movies
            .Where(m => m.DeletedAt == null)
            .Select(m => m.Language)
            .Where(l => l != null && l != "")
            .Distinct()
            .OrderBy(l => l)
            .ToList();

        ViewBag.SelectedLanguage = language;
        ViewBag.Search = null;
        ViewBag.Management = management;

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
    public IActionResult Search(string? query, string? language, bool management = false, bool partial = false)
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
        ViewBag.Management = management;

        var moviesQuery = _dbContext.Movies
            .Include(movie => movie.Attachments)
            .Where(movie => movie.DeletedAt == null)
            .AsQueryable();

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

        ValidateMovieBusinessRules(model);

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

        _logger.LogInformation(
            "Movie created by MVC. MovieId={MovieId}, Genre={Genre}, Language={Language}, AgeRating={AgeRating}, DurationMinutes={DurationMinutes}, UserId={UserId}",
            movie.Id,
            movie.Genre,
            movie.Language,
            movie.AgeRating,
            movie.DurationMinutes,
            UserId ?? "unknown");

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

        ValidateMovieBusinessRules(model);

        if (ok && ModelState.IsValid)
        {
            movie.Language = model.Language;
            movie.AgeRating = model.AgeRating;
            _dbContext.SaveChanges();

            _logger.LogInformation(
                "Movie updated by MVC. MovieId={MovieId}, Genre={Genre}, Language={Language}, AgeRating={AgeRating}, DurationMinutes={DurationMinutes}, UserId={UserId}",
                movie.Id,
                movie.Genre,
                movie.Language,
                movie.AgeRating,
                movie.DurationMinutes,
                UserId ?? "unknown");

            return RedirectToAction(nameof(Index));
        }

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

        var deleteSummary = SoftDeleteMovie(movie);
        _dbContext.SaveChanges();

        _logger.LogInformation(
            "Movie soft deleted by MVC. MovieId={MovieId}, DeletedScreeningCount={DeletedScreeningCount}, DeletedTicketCount={DeletedTicketCount}, DeletedFavoriteCount={DeletedFavoriteCount}, UserId={UserId}",
            movie.Id,
            deleteSummary.DeletedScreeningCount,
            deleteSummary.DeletedTicketCount,
            deleteSummary.DeletedFavoriteCount,
            UserId ?? "unknown");

        return RedirectToAction(nameof(Index));
    }

    [HttpPost("uredi/{movieId}/datoteke/objavi")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> UploadAttachment(int movieId, IFormFile? file)
    {
        var movieExists = await _dbContext.Movies.AnyAsync(movie => movie.Id == movieId && movie.DeletedAt == null);

        if (!movieExists)
        {
            _logger.LogWarning(
                "Attachment upload requested for missing movie. MovieId={MovieId}, UserId={UserId}",
                movieId,
                UserId ?? "unknown");

            return NotFound();
        }

        if (!TryValidatePosterFile(file, out var originalFileName, out var extension, out var validationError))
        {
            _logger.LogWarning(
                "Attachment upload rejected. MovieId={MovieId}, ContentType={ContentType}, FileSize={FileSize}, Reason={Reason}, UserId={UserId}",
                movieId,
                file?.ContentType ?? string.Empty,
                file?.Length ?? 0,
                validationError,
                UserId ?? "unknown");

            return BadRequest(validationError);
        }

        var uploadsPath = _uploadStorage.GetMovieDirectory(movieId);

        Directory.CreateDirectory(uploadsPath);

        var fileName = $"{Guid.NewGuid():N}{extension.ToLowerInvariant()}";
        var filePath = Path.Combine(uploadsPath, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file!.CopyToAsync(stream);
        }

        var attachment = new Attachment
        {
            MovieId = movieId,
            FileName = originalFileName,
            FilePath = _uploadStorage.GetMoviePublicPath(movieId, fileName),
            ContentType = file.ContentType,
            FileSize = file.Length,
            CreatedAt = DateTime.UtcNow
        };

        _dbContext.Attachments.Add(attachment);
        await _dbContext.SaveChangesAsync();

        _logger.LogInformation(
            "Attachment uploaded. AttachmentId={AttachmentId}, MovieId={MovieId}, ContentType={ContentType}, FileSize={FileSize}, UserId={UserId}",
            attachment.Id,
            attachment.MovieId,
            attachment.ContentType,
            attachment.FileSize,
            UserId ?? "unknown");

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
            _logger.LogWarning(
                "Attachment delete requested for missing attachment. AttachmentId={AttachmentId}, UserId={UserId}",
                id,
                UserId ?? "unknown");

            return NotFound();
        }

        var physicalPath = _uploadStorage.GetPhysicalPath(attachment.FilePath);

        if (System.IO.File.Exists(physicalPath))
        {
            try
            {
                System.IO.File.Delete(physicalPath);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Attachment file delete failed. AttachmentId={AttachmentId}, MovieId={MovieId}, UserId={UserId}",
                    attachment.Id,
                    attachment.MovieId,
                    UserId ?? "unknown");

                throw;
            }
        }
        else
        {
            _logger.LogWarning(
                "Attachment file was missing during delete. AttachmentId={AttachmentId}, MovieId={MovieId}, UserId={UserId}",
                attachment.Id,
                attachment.MovieId,
                UserId ?? "unknown");
        }

        _dbContext.Attachments.Remove(attachment);
        await _dbContext.SaveChangesAsync();

        _logger.LogInformation(
            "Attachment deleted. AttachmentId={AttachmentId}, MovieId={MovieId}, ContentType={ContentType}, FileSize={FileSize}, UserId={UserId}",
            attachment.Id,
            attachment.MovieId,
            attachment.ContentType,
            attachment.FileSize,
            UserId ?? "unknown");

        return Json(new { success = true });
    }

    private (int DeletedScreeningCount, int DeletedTicketCount, int DeletedFavoriteCount) SoftDeleteMovie(Movie movie)
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

        return (screenings.Count, tickets.Count, favorites.Count);
    }

    private void ValidateMovieBusinessRules(MovieFormViewModel model)
    {
        model.Language = (model.Language ?? string.Empty).Trim().ToUpperInvariant();
        model.AgeRating = (model.AgeRating ?? string.Empty).Trim().ToUpperInvariant();

        if (!AllowedAgeRatings.Contains(model.AgeRating) && !System.Text.RegularExpressions.Regex.IsMatch(model.AgeRating, "^PG-[0-9]{1,2}$"))
        {
            ModelState.AddModelError(nameof(model.AgeRating), AgeRatingErrorMessage);
        }
    }

    private static bool TryValidatePosterFile(
        IFormFile? file,
        out string originalFileName,
        out string extension,
        out string errorMessage)
    {
        originalFileName = string.Empty;
        extension = string.Empty;
        errorMessage = string.Empty;

        if (file is null || file.Length == 0)
        {
            errorMessage = "Datoteka nije poslana.";
            return false;
        }

        if (file.Length > MaxPosterFileSizeInBytes)
        {
            errorMessage = "Maksimalna velicina poster slike je 5 MB.";
            return false;
        }

        var normalizedClientFileName = file.FileName.Replace('\\', '/');
        originalFileName = Path.GetFileName(normalizedClientFileName);

        if (string.IsNullOrWhiteSpace(originalFileName))
        {
            errorMessage = "Naziv datoteke nije ispravan.";
            return false;
        }

        if (originalFileName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
        {
            errorMessage = "Naziv datoteke sadrzi nedopustene znakove.";
            return false;
        }

        extension = Path.GetExtension(originalFileName);

        if (!AllowedPosterContentTypesByExtension.TryGetValue(extension, out var allowedContentTypes))
        {
            errorMessage = "Dopustene su samo JPG, PNG i WEBP slike.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(file.ContentType) ||
            !allowedContentTypes.Contains(file.ContentType, StringComparer.OrdinalIgnoreCase))
        {
            errorMessage = "MIME type datoteke ne odgovara dopustenim poster slikama.";
            return false;
        }

        return true;
    }

}
