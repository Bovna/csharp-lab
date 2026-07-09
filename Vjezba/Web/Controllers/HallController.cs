using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Vjezba.DAL;
using Vjezba.Model.Entities;
using Vjezba.Web.ViewModels;

namespace Vjezba.Web.Controllers;

[AutoValidateAntiforgeryToken]
[Route("dvorana")]
[Authorize]
public class HallController : Controller
{
    private readonly CinemaDbContext _dbContext;
    private readonly ILogger<HallController> _logger;

    public HallController(CinemaDbContext dbContext, ILogger<HallController> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    [Route("")]
    [Route("pretraga")]
    [AllowAnonymous]
    public IActionResult Index(bool? supports3D, bool partial = false)
    {
        var hallsQuery = ActiveHallsQuery();

        if (supports3D.HasValue)
        {
            hallsQuery = hallsQuery.Where(hall => hall.Supports3D == supports3D.Value);
        }

        ViewBag.Supports3D = supports3D;
        ViewBag.Search = null;

        var halls = hallsQuery.OrderBy(hall => hall.Id).ToList();

        if (partial)
        {
            return PartialView("_IndexResults", halls);
        }

        return View(halls);
    }

    [HttpGet("rezultati")]
    [AllowAnonymous]
    public IActionResult Search(string? query, bool? supports3D, bool partial = false)
    {
        var normalizedQuery = (query ?? string.Empty).Trim();
        var hallsQuery = ActiveHallsQuery();

        if (supports3D.HasValue)
        {
            hallsQuery = hallsQuery.Where(hall => hall.Supports3D == supports3D.Value);
        }

        if (!string.IsNullOrWhiteSpace(normalizedQuery))
        {
            hallsQuery = hallsQuery.Where(hall =>
                hall.Name.Contains(normalizedQuery) ||
                hall.Cinema.Name.Contains(normalizedQuery));
        }

        ViewBag.Supports3D = supports3D;
        ViewBag.Search = query;

        var halls = hallsQuery.OrderBy(hall => hall.Id).ToList();

        if (partial)
        {
            return PartialView("_IndexResults", halls);
        }

        return View(nameof(Index), halls);
    }

    [HttpGet("autocomplete")]
    [AllowAnonymous]
    public IActionResult Autocomplete(string? query)
    {
        var normalizedQuery = (query ?? string.Empty).Trim();

        var halls = ActiveHallsQuery()
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

    [Route("detalji/{id}")]
    [Authorize]
    public IActionResult Details(int id)
    {
        var hall = ActiveHallsQuery().FirstOrDefault(hall => hall.Id == id);

        if (hall is null)
        {
            return NotFound();
        }

        var seats = _dbContext.Seats
            .Where(seat => seat.HallId == hall.Id && seat.DeletedAt == null)
            .OrderBy(seat => seat.RowLabel)
            .ThenBy(seat => seat.SeatNumber)
            .ToList();

        var screenings = _dbContext.Screenings
            .Where(screening => screening.HallId == hall.Id
                && screening.DeletedAt == null
                && screening.Movie.DeletedAt == null)
            .Include(screening => screening.Movie)
            .OrderBy(screening => screening.StartTime)
            .ToList();

        return View(new HallDetailsViewModel
        {
            Hall = hall,
            Seats = seats,
            Screenings = screenings
        });
    }

    [Route("dodaj")]
    [Authorize(Roles = "Admin,Manager")]
    public IActionResult Create()
    {
        var model = new HallFormViewModel();
        PrepareHallForm(model);
        return View(model);
    }

    [HttpPost("dodaj")]
    [Authorize(Roles = "Admin,Manager")]
    public IActionResult Create(HallFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            PrepareHallForm(model);
            return View(model);
        }

        ValidateHallBusinessRules(model);

        if (!ModelState.IsValid)
        {
            PrepareHallForm(model);
            return View(model);
        }

        var hall = new Hall();
        MapHallForm(model, hall);

        _dbContext.Halls.Add(hall);
        _dbContext.SaveChanges();

        _logger.LogInformation(
            "Hall created by MVC. HallId={HallId}, CinemaId={CinemaId}, Capacity={Capacity}, Supports3D={Supports3D}, UserId={UserId}",
            hall.Id,
            hall.CinemaId,
            hall.Capacity,
            hall.Supports3D,
            GetCurrentUserId());

        return RedirectToAction(nameof(Index));
    }

    [Route("uredi/{id}")]
    [ActionName("Edit")]
    [Authorize(Roles = "Admin,Manager")]
    public IActionResult EditGet(int id)
    {
        var hall = ActiveHallsQuery().FirstOrDefault(hall => hall.Id == id);

        if (hall is null)
        {
            return NotFound();
        }

        var model = ToHallForm(hall);
        PrepareHallForm(model);
        return View(model);
    }

    [HttpPost("uredi/{id}")]
    [ActionName("Edit")]
    [Authorize(Roles = "Admin,Manager")]
    public IActionResult EditPost(int id, HallFormViewModel model)
    {
        var hall = ActiveHallsQuery().FirstOrDefault(hall => hall.Id == id);

        if (hall is null)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            model.Id = id;
            PrepareHallForm(model);
            return View(model);
        }

        ValidateHallBusinessRules(model, id);

        if (!ModelState.IsValid)
        {
            model.Id = id;
            PrepareHallForm(model);
            return View(model);
        }

        MapHallForm(model, hall);
        _dbContext.SaveChanges();

        _logger.LogInformation(
            "Hall updated by MVC. HallId={HallId}, CinemaId={CinemaId}, Capacity={Capacity}, Supports3D={Supports3D}, UserId={UserId}",
            hall.Id,
            hall.CinemaId,
            hall.Capacity,
            hall.Supports3D,
            GetCurrentUserId());

        return RedirectToAction(nameof(Index));
    }

    [HttpPost("obrisi/{id}")]
    [Authorize(Roles = "Admin")]
    public IActionResult Delete(int id)
    {
        var hall = ActiveHallsQuery().FirstOrDefault(hall => hall.Id == id);

        if (hall is null)
        {
            return NotFound();
        }

        var deleteSummary = SoftDeleteHall(hall);
        _dbContext.SaveChanges();

        _logger.LogInformation(
            "Hall soft deleted by MVC. HallId={HallId}, CinemaId={CinemaId}, DeletedSeatCount={DeletedSeatCount}, DeletedScreeningCount={DeletedScreeningCount}, DeletedTicketCount={DeletedTicketCount}, UserId={UserId}",
            hall.Id,
            hall.CinemaId,
            deleteSummary.DeletedSeatCount,
            deleteSummary.DeletedScreeningCount,
            deleteSummary.DeletedTicketCount,
            GetCurrentUserId());

        return RedirectToAction(nameof(Index));
    }

    private string GetCurrentUserId()
    {
        return User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "unknown";
    }

    private IQueryable<Hall> ActiveHallsQuery()
    {
        return _dbContext.Halls
            .Include(hall => hall.Cinema)
            .Where(hall => hall.DeletedAt == null && hall.Cinema.DeletedAt == null);
    }

    private void PrepareHallForm(HallFormViewModel model)
    {
        model.CinemaSelector = new AutocompleteViewModel
        {
            InputName = nameof(model.CinemaId),
            Label = "Kino",
            Endpoint = Url.Action(nameof(CinemaController.Autocomplete), "Cinema") ?? "/kina/autocomplete",
            SearchPlaceholder = "Pretrazite kino po nazivu",
            RequiredMessage = "Kino je obavezno.",
            EnableRemoteSearch = true,
            Items = BuildSelectedCinemaItems(model.CinemaId)
        };
    }

    private List<SelectListItem> BuildSelectedCinemaItems(int? selectedCinemaId)
    {
        if (!selectedCinemaId.HasValue)
        {
            return new List<SelectListItem>();
        }

        return _dbContext.Cinemas
            .Where(cinema => cinema.DeletedAt == null && cinema.Id == selectedCinemaId.Value)
            .Select(cinema => new SelectListItem
            {
                Value = cinema.Id.ToString(),
                Text = cinema.Name,
                Selected = true
            })
            .ToList();
    }

    private static HallFormViewModel ToHallForm(Hall hall)
    {
        return new HallFormViewModel
        {
            Id = hall.Id,
            Name = hall.Name,
            Capacity = hall.Capacity,
            Supports3D = hall.Supports3D,
            CinemaId = hall.CinemaId
        };
    }

    private static void MapHallForm(HallFormViewModel model, Hall hall)
    {
        hall.Name = model.Name.Trim();
        hall.Capacity = model.Capacity;
        hall.Supports3D = model.Supports3D;
        hall.CinemaId = model.CinemaId!.Value;
    }

    private void ValidateHallBusinessRules(HallFormViewModel model, int? currentHallId = null)
    {
        model.Name = (model.Name ?? string.Empty).Trim();

        if (!model.CinemaId.HasValue || string.IsNullOrWhiteSpace(model.Name))
        {
            return;
        }

        var normalizedName = model.Name.ToLower();
        var hallExists = _dbContext.Halls.Any(hall =>
            hall.DeletedAt == null
            && hall.CinemaId == model.CinemaId.Value
            && hall.Name.ToLower() == normalizedName
            && (!currentHallId.HasValue || hall.Id != currentHallId.Value));

        if (hallExists)
        {
            ModelState.AddModelError(nameof(model.Name), "Dvorana s tim nazivom već postoji u odabranom kinu.");
        }
    }

    private (int DeletedSeatCount, int DeletedScreeningCount, int DeletedTicketCount) SoftDeleteHall(Hall hall)
    {
        var deletedAt = DateTime.UtcNow;
        hall.DeletedAt = deletedAt;

        var seats = _dbContext.Seats
            .Where(seat => seat.HallId == hall.Id && seat.DeletedAt == null)
            .ToList();

        foreach (var seat in seats)
        {
            seat.DeletedAt = deletedAt;
        }

        var screenings = _dbContext.Screenings
            .Where(screening => screening.HallId == hall.Id && screening.DeletedAt == null)
            .ToList();

        foreach (var screening in screenings)
        {
            screening.DeletedAt = deletedAt;
        }

        var screeningIds = screenings.Select(screening => screening.Id).ToList();
        var seatIds = seats.Select(seat => seat.Id).ToList();

        var tickets = _dbContext.Tickets
            .Where(ticket => ticket.DeletedAt == null
                && (screeningIds.Contains(ticket.ScreeningId)
                    || (ticket.SeatId.HasValue && seatIds.Contains(ticket.SeatId.Value))))
            .ToList();

        foreach (var ticket in tickets)
        {
            ticket.DeletedAt = deletedAt;
        }

        return (seats.Count, screenings.Count, tickets.Count);
    }
}
