using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KinoKlik.DAL;
using KinoKlik.Model.Entities;
using KinoKlik.Web.ViewModels;

namespace KinoKlik.Web.Controllers;

[AutoValidateAntiforgeryToken]
[Route("projekcije")]
[Authorize]
public class ScreeningController : BaseController
{
    private readonly CinemaDbContext _dbContext;
    private readonly ILogger<ScreeningController> _logger;

    public ScreeningController(
        CinemaDbContext dbContext,
        UserManager<AppUser> userManager,
        ILogger<ScreeningController> logger)
        : base(userManager)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    [Route("pretraga")]
    [AllowAnonymous]
    public IActionResult Index(int? dayOfWeek, bool management = false, bool partial = false)
    {
        var screenings = ActiveScreeningsQuery()
            .OrderBy(screening => screening.Id)
            .ToList();

        if (dayOfWeek.HasValue)
        {
            var targetDayOfWeek = (DayOfWeek)(dayOfWeek.Value % 7);
            screenings = screenings
                .Where(screening => screening.StartTime.DayOfWeek == targetDayOfWeek)
                .ToList();
        }

        ViewBag.SelectedDayOfWeek = dayOfWeek;
        ViewBag.Search = null;
        ViewBag.Management = management;

        if (partial)
        {
            return PartialView("_IndexResults", screenings);
        }

        return View(screenings);
    }

    [AllowAnonymous]
    public IActionResult Search(string? query, int? dayOfWeek, bool management = false, bool partial = false)
    {
        var normalizedQuery = (query ?? string.Empty).Trim();
        var screeningsQuery = ActiveScreeningsQuery();

        if (!string.IsNullOrWhiteSpace(normalizedQuery))
        {
            screeningsQuery = screeningsQuery.Where(screening =>
                screening.Movie.Title.Contains(normalizedQuery) ||
                screening.Hall.Name.Contains(normalizedQuery) ||
                screening.Hall.Cinema.Name.Contains(normalizedQuery));
        }

        var screenings = screeningsQuery
            .OrderBy(screening => screening.Id)
            .ToList();

        if (dayOfWeek.HasValue)
        {
            var targetDayOfWeek = (DayOfWeek)(dayOfWeek.Value % 7);
            screenings = screenings
                .Where(screening => screening.StartTime.DayOfWeek == targetDayOfWeek)
                .ToList();
        }

        ViewBag.SelectedDayOfWeek = dayOfWeek;
        ViewBag.Search = query;
        ViewBag.Management = management;

        if (partial)
        {
            return PartialView("_IndexResults", screenings);
        }

        return View(nameof(Index), screenings);
    }

    [HttpGet("dvorane/autocomplete")]
    [Authorize(Roles = "Admin,Manager")]
    public IActionResult HallAutocomplete(string? query)
    {
        var normalizedQuery = (query ?? string.Empty).Trim();

        var halls = _dbContext.Halls
            .Include(hall => hall.Cinema)
            .Where(hall => hall.DeletedAt == null && hall.Cinema.DeletedAt == null)
            .Where(hall => string.IsNullOrEmpty(normalizedQuery)
                || hall.Name.Contains(normalizedQuery)
                || hall.Cinema.Name.Contains(normalizedQuery))
            .OrderBy(hall => hall.Cinema.Name)
            .ThenBy(hall => hall.Name)
            .Take(12)
            .Select(hall => new
            {
                value = hall.Id,
                text = hall.Cinema.Name + " - " + hall.Name
            })
            .ToList();

        return Json(halls);
    }

    [HttpGet("autocomplete")]
    [AllowAnonymous]
    public IActionResult ScreeningAutocomplete(string? query)
    {
        var normalizedQuery = (query ?? string.Empty).Trim();

        var screenings = ActiveScreeningsQuery()
            .Where(screening => string.IsNullOrEmpty(normalizedQuery)
                || screening.Movie.Title.Contains(normalizedQuery)
                || screening.Hall.Name.Contains(normalizedQuery)
                || screening.Hall.Cinema.Name.Contains(normalizedQuery))
            .OrderBy(screening => screening.StartTime)
            .Take(12)
            .AsEnumerable()
            .Select(screening => new
            {
                value = screening.Id,
                text = screening.Movie.Title + " - " + screening.Hall.Cinema.Name + " / " + screening.Hall.Name + " - " + screening.StartTime.ToString("dd.MM.yyyy HH:mm")
            })
            .ToList();

        return Json(screenings);
    }

    [Route("detalji/{id}")]
    [Authorize]
    public IActionResult Details(int id)
    {
        var screening = ActiveScreeningsQuery()
            .FirstOrDefault(s => s.Id == id);

        if (screening is null)
        {
            return NotFound();
        }

        var tickets = _dbContext.Tickets
            .Where(t => t.ScreeningId == screening.Id
                && t.DeletedAt == null
                && t.Customer.DeletedAt == null
                && (t.Seat == null || t.Seat.DeletedAt == null))
            .Include(t => t.Customer)
            .Include(t => t.Seat)
            .OrderByDescending(t => t.PurchasedAt)
            .ToList();

        var viewModel = new ScreeningDetailsViewModel
        {
            Screening = screening,
            Tickets = tickets
        };

        return View(viewModel);
    }

    [Route("dodaj")]
    [Authorize(Roles = "Admin,Manager")]
    public IActionResult Create()
    {
        var model = new ScreeningFormViewModel
        {
            StartTime = DateTime.Now,
            EndTime = DateTime.Now.AddHours(2)
        };

        PrepareScreeningForm(model);
        return View(model);
    }

    [HttpPost("dodaj")]
    [Authorize(Roles = "Admin,Manager")]
    public IActionResult Create(ScreeningFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            PrepareScreeningForm(model);
            return View(model);
        }

        ValidateScreeningBusinessRules(model);

        if (!ModelState.IsValid)
        {
            PrepareScreeningForm(model);
            return View(model);
        }

        var screening = new Screening
        {
            StartTime = model.StartTime,
            EndTime = model.EndTime,
            Is3D = model.Is3D,
            MovieId = model.MovieId!.Value,
            HallId = model.HallId!.Value
        };

        _dbContext.Screenings.Add(screening);
        _dbContext.SaveChanges();

        _logger.LogInformation(
            "Screening created by MVC. ScreeningId={ScreeningId}, MovieId={MovieId}, HallId={HallId}, StartTime={StartTime}, EndTime={EndTime}, Is3D={Is3D}, UserId={UserId}",
            screening.Id,
            screening.MovieId,
            screening.HallId,
            screening.StartTime,
            screening.EndTime,
            screening.Is3D,
            UserId ?? "unknown");

        return RedirectToAction(nameof(Index));
    }

    [Route("uredi/{id}")]
    [ActionName("Edit")]
    [Authorize(Roles = "Admin,Manager")]
    public IActionResult EditGet(int id)
    {
        var screening = _dbContext.Screenings.FirstOrDefault(s => s.Id == id && s.DeletedAt == null);

        if (screening is null)
        {
            return NotFound();
        }

        var model = new ScreeningFormViewModel
        {
            Id = screening.Id,
            StartTime = screening.StartTime,
            EndTime = screening.EndTime,
            Is3D = screening.Is3D,
            MovieId = screening.MovieId,
            HallId = screening.HallId
        };

        PrepareScreeningForm(model);
        return View(model);
    }

    [HttpPost("uredi/{id}")]
    [ActionName("Edit")]
    [Authorize(Roles = "Admin,Manager")]
    public IActionResult EditPost(int id, ScreeningFormViewModel model)
    {
        var screening = _dbContext.Screenings.FirstOrDefault(s => s.Id == id && s.DeletedAt == null);

        if (screening is null)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            model.Id = id;
            PrepareScreeningForm(model);
            return View(model);
        }

        ValidateScreeningBusinessRules(model, id);

        if (!ModelState.IsValid)
        {
            model.Id = id;
            PrepareScreeningForm(model);
            return View(model);
        }

        screening.StartTime = model.StartTime;
        screening.EndTime = model.EndTime;
        screening.Is3D = model.Is3D;
        screening.MovieId = model.MovieId!.Value;
        screening.HallId = model.HallId!.Value;

        _dbContext.SaveChanges();

        _logger.LogInformation(
            "Screening updated by MVC. ScreeningId={ScreeningId}, MovieId={MovieId}, HallId={HallId}, StartTime={StartTime}, EndTime={EndTime}, Is3D={Is3D}, UserId={UserId}",
            screening.Id,
            screening.MovieId,
            screening.HallId,
            screening.StartTime,
            screening.EndTime,
            screening.Is3D,
            UserId ?? "unknown");

        return RedirectToAction(nameof(Index));
    }

    [HttpPost("obrisi/{id}")]
    [Authorize(Roles = "Admin")]
    public IActionResult Delete(int id)
    {
        var screening = _dbContext.Screenings.FirstOrDefault(s => s.Id == id && s.DeletedAt == null);

        if (screening is null)
        {
            return NotFound();
        }

        var deletedTicketCount = SoftDeleteScreening(screening);
        _dbContext.SaveChanges();

        _logger.LogInformation(
            "Screening soft deleted by MVC. ScreeningId={ScreeningId}, MovieId={MovieId}, HallId={HallId}, DeletedTicketCount={DeletedTicketCount}, UserId={UserId}",
            screening.Id,
            screening.MovieId,
            screening.HallId,
            deletedTicketCount,
            UserId ?? "unknown");

        return RedirectToAction(nameof(Index));
    }

    private IQueryable<Screening> ActiveScreeningsQuery()
    {
        return _dbContext.Screenings
            .Where(screening => screening.DeletedAt == null
                && screening.Movie.DeletedAt == null
                && screening.Hall.DeletedAt == null
                && screening.Hall.Cinema.DeletedAt == null)
            .Include(screening => screening.Movie)
            .Include(screening => screening.Hall)
                .ThenInclude(hall => hall.Cinema);
    }

    private int SoftDeleteScreening(Screening screening)
    {
        var deletedAt = screening.DeletedAt ?? DateTime.UtcNow;
        screening.DeletedAt = deletedAt;

        var tickets = _dbContext.Tickets
            .Where(ticket => ticket.ScreeningId == screening.Id && ticket.DeletedAt == null)
            .ToList();

        foreach (var ticket in tickets)
        {
            ticket.DeletedAt = deletedAt;
        }

        return tickets.Count;
    }

    private void PrepareScreeningForm(ScreeningFormViewModel model)
    {
        model.MovieSelector = new AutocompleteViewModel
        {
            InputName = nameof(model.MovieId),
            Label = "Film",
            Endpoint = Url.Action(nameof(MovieController.Autocomplete), "Movie") ?? "/filmovi/autocomplete",
            SearchPlaceholder = "Pretražite film po naslovu",
            RequiredMessage = "Film je obavezan.",
            EnableRemoteSearch = true,
            Items = BuildSelectedMovieItems(model.MovieId)
        };

        model.HallSelector = new AutocompleteViewModel
        {
            InputName = nameof(model.HallId),
            Label = "Dvorana",
            Endpoint = Url.Action(nameof(HallAutocomplete), "Screening") ?? "/projekcije/dvorane/autocomplete",
            SearchPlaceholder = "Pretražite dvoranu po kinu ili nazivu",
            RequiredMessage = "Dvorana je obavezna.",
            EnableRemoteSearch = true,
            Items = BuildSelectedHallItems(model.HallId)
        };
    }

    private void ValidateScreeningBusinessRules(ScreeningFormViewModel model, int? currentScreeningId = null)
    {
        if (model.EndTime <= model.StartTime)
        {
            ModelState.AddModelError(nameof(model.EndTime), "Vrijeme završetka mora biti nakon vremena početka.");
        }

        if (!model.HallId.HasValue)
        {
            return;
        }

        var hall = _dbContext.Halls
            .Include(hall => hall.Cinema)
            .FirstOrDefault(hall => hall.Id == model.HallId.Value
                && hall.DeletedAt == null
                && hall.Cinema.DeletedAt == null);

        if (model.Is3D && hall is not null && !hall.Supports3D)
        {
            ModelState.AddModelError(nameof(model.HallId), "Odabrana dvorana ne podržava 3D projekcije.");
        }

        if (model.EndTime <= model.StartTime)
        {
            return;
        }

        var hasOverlappingScreening = _dbContext.Screenings.Any(screening =>
            screening.DeletedAt == null
            && screening.HallId == model.HallId.Value
            && (!currentScreeningId.HasValue || screening.Id != currentScreeningId.Value)
            && screening.StartTime < model.EndTime
            && model.StartTime < screening.EndTime);

        if (hasOverlappingScreening)
        {
            ModelState.AddModelError(nameof(model.HallId), "U odabranoj dvorani već postoji projekcija u tom terminu.");
        }
    }

    private List<SelectListItem> BuildSelectedMovieItems(int? selectedMovieId)
    {
        if (!selectedMovieId.HasValue)
        {
            return new List<SelectListItem>();
        }

        return _dbContext.Movies
            .Where(movie => movie.DeletedAt == null && movie.Id == selectedMovieId.Value)
            .Select(movie => new SelectListItem
            {
                Value = movie.Id.ToString(),
                Text = movie.Title,
                Selected = true
            })
            .ToList();
    }

    private List<SelectListItem> BuildSelectedHallItems(int? selectedHallId)
    {
        if (!selectedHallId.HasValue)
        {
            return new List<SelectListItem>();
        }

        return _dbContext.Halls
            .Include(hall => hall.Cinema)
            .Where(hall => hall.DeletedAt == null
                && hall.Cinema.DeletedAt == null
                && hall.Id == selectedHallId.Value)
            .Select(hall => new SelectListItem
            {
                Value = hall.Id.ToString(),
                Text = hall.Cinema.Name + " - " + hall.Name,
                Selected = true
            })
            .ToList();
    }
}
