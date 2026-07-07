using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Vjezba.DAL;
using Vjezba.Model.Entities;
using Vjezba.Web.ViewModels;

namespace Vjezba.Web.Controllers;

[Route("sjedala")]
[Authorize]
public class SeatController : Controller
{
    private readonly CinemaDbContext _dbContext;

    public SeatController(CinemaDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [Route("")]
    [Route("pretraga")]
    [AllowAnonymous]
    public IActionResult Index(SeatType? seatType, bool partial = false)
    {
        var seatsQuery = ActiveSeatsQuery();

        if (seatType.HasValue)
        {
            seatsQuery = seatsQuery.Where(seat => seat.SeatType == seatType.Value);
        }

        ViewBag.SelectedSeatType = seatType?.ToString();
        ViewBag.Search = null;

        var seats = seatsQuery.OrderBy(seat => seat.Id).ToList();

        if (partial)
        {
            return PartialView("_IndexResults", seats);
        }

        return View(seats);
    }

    [HttpGet("rezultati")]
    [AllowAnonymous]
    public IActionResult Search(string? query, SeatType? seatType, bool partial = false)
    {
        var normalizedQuery = (query ?? string.Empty).Trim();
        var seatsQuery = ActiveSeatsQuery();

        if (seatType.HasValue)
        {
            seatsQuery = seatsQuery.Where(seat => seat.SeatType == seatType.Value);
        }

        if (!string.IsNullOrWhiteSpace(normalizedQuery))
        {
            if (int.TryParse(normalizedQuery, out var seatNumber))
            {
                seatsQuery = seatsQuery.Where(seat => seat.SeatNumber == seatNumber);
            }
            else
            {
                seatsQuery = seatsQuery.Where(seat =>
                    seat.RowLabel.Contains(normalizedQuery) ||
                    seat.Hall.Name.Contains(normalizedQuery) ||
                    seat.Hall.Cinema.Name.Contains(normalizedQuery));
            }
        }

        ViewBag.SelectedSeatType = seatType?.ToString();
        ViewBag.Search = query;

        var seats = seatsQuery.OrderBy(seat => seat.Id).ToList();

        if (partial)
        {
            return PartialView("_IndexResults", seats);
        }

        return View(nameof(Index), seats);
    }

    [HttpGet("autocomplete")]
    [AllowAnonymous]
    public IActionResult Autocomplete(string? query)
    {
        var normalizedQuery = (query ?? string.Empty).Trim();

        var seats = ActiveSeatsQuery()
            .Where(seat => string.IsNullOrEmpty(normalizedQuery)
                || seat.RowLabel.Contains(normalizedQuery)
                || seat.Hall.Name.Contains(normalizedQuery)
                || seat.Hall.Cinema.Name.Contains(normalizedQuery))
            .OrderBy(seat => seat.Hall.Cinema.Name)
            .ThenBy(seat => seat.Hall.Name)
            .ThenBy(seat => seat.RowLabel)
            .ThenBy(seat => seat.SeatNumber)
            .Take(12)
            .Select(seat => new
            {
                value = seat.Id,
                text = seat.Hall.Cinema.Name + " - " + seat.Hall.Name + " - " + seat.RowLabel + seat.SeatNumber
            })
            .ToList();

        return Json(seats);
    }

    [Route("detalji/{id}")]
    [Authorize]
    public IActionResult Details(int id)
    {
        var seat = ActiveSeatsQuery().FirstOrDefault(seat => seat.Id == id);

        if (seat is null)
        {
            return NotFound();
        }

        return View(seat);
    }

    [Route("dodaj")]
    [Authorize(Roles = "Admin,Manager")]
    public IActionResult Create()
    {
        var model = new SeatFormViewModel();
        PrepareSeatForm(model);
        return View(model);
    }

    [HttpPost("dodaj")]
    [Authorize(Roles = "Admin,Manager")]
    public IActionResult Create(SeatFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            PrepareSeatForm(model);
            return View(model);
        }

        ValidateSeatBusinessRules(model);

        if (!ModelState.IsValid)
        {
            PrepareSeatForm(model);
            return View(model);
        }

        var seat = new Seat();
        MapSeatForm(model, seat);

        _dbContext.Seats.Add(seat);
        _dbContext.SaveChanges();

        return RedirectToAction(nameof(Index));
    }

    [Route("uredi/{id}")]
    [ActionName("Edit")]
    [Authorize(Roles = "Admin,Manager")]
    public IActionResult EditGet(int id)
    {
        var seat = ActiveSeatsQuery().FirstOrDefault(seat => seat.Id == id);

        if (seat is null)
        {
            return NotFound();
        }

        var model = ToSeatForm(seat);
        PrepareSeatForm(model);
        return View(model);
    }

    [HttpPost("uredi/{id}")]
    [ActionName("Edit")]
    [Authorize(Roles = "Admin,Manager")]
    public IActionResult EditPost(int id, SeatFormViewModel model)
    {
        var seat = ActiveSeatsQuery().FirstOrDefault(seat => seat.Id == id);

        if (seat is null)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            model.Id = id;
            PrepareSeatForm(model);
            return View(model);
        }

        ValidateSeatBusinessRules(model, id);

        if (!ModelState.IsValid)
        {
            model.Id = id;
            PrepareSeatForm(model);
            return View(model);
        }

        MapSeatForm(model, seat);
        _dbContext.SaveChanges();

        return RedirectToAction(nameof(Index));
    }

    [HttpPost("obrisi/{id}")]
    [Authorize(Roles = "Admin")]
    public IActionResult Delete(int id)
    {
        var seat = ActiveSeatsQuery().FirstOrDefault(seat => seat.Id == id);

        if (seat is null)
        {
            return NotFound();
        }

        SoftDeleteSeat(seat);
        _dbContext.SaveChanges();

        return RedirectToAction(nameof(Index));
    }

    private IQueryable<Seat> ActiveSeatsQuery()
    {
        return _dbContext.Seats
            .Include(seat => seat.Hall)
                .ThenInclude(hall => hall.Cinema)
            .Where(seat => seat.DeletedAt == null
                && seat.Hall.DeletedAt == null
                && seat.Hall.Cinema.DeletedAt == null);
    }

    private void PrepareSeatForm(SeatFormViewModel model)
    {
        model.HallSelector = new AutocompleteViewModel
        {
            InputName = nameof(model.HallId),
            Label = "Dvorana",
            Endpoint = Url.Action(nameof(HallController.Autocomplete), "Hall") ?? "/dvorana/autocomplete",
            SearchPlaceholder = "Pretrazite dvoranu po kinu ili nazivu",
            RequiredMessage = "Dvorana je obavezna.",
            EnableRemoteSearch = true,
            Items = BuildSelectedHallItems(model.HallId)
        };
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

    private static SeatFormViewModel ToSeatForm(Seat seat)
    {
        return new SeatFormViewModel
        {
            Id = seat.Id,
            RowLabel = seat.RowLabel,
            SeatNumber = seat.SeatNumber,
            SeatType = seat.SeatType,
            HallId = seat.HallId
        };
    }

    private static void MapSeatForm(SeatFormViewModel model, Seat seat)
    {
        seat.RowLabel = model.RowLabel.Trim().ToUpperInvariant();
        seat.SeatNumber = model.SeatNumber;
        seat.SeatType = model.SeatType;
        seat.HallId = model.HallId!.Value;
    }

    private void ValidateSeatBusinessRules(SeatFormViewModel model, int? currentSeatId = null)
    {
        model.RowLabel = (model.RowLabel ?? string.Empty).Trim().ToUpperInvariant();

        if (!model.HallId.HasValue || string.IsNullOrWhiteSpace(model.RowLabel))
        {
            return;
        }

        var seatExists = _dbContext.Seats.Any(seat =>
            seat.DeletedAt == null
            && seat.HallId == model.HallId.Value
            && seat.RowLabel.ToLower() == model.RowLabel.ToLower()
            && seat.SeatNumber == model.SeatNumber
            && (!currentSeatId.HasValue || seat.Id != currentSeatId.Value));

        if (seatExists)
        {
            ModelState.AddModelError(nameof(model.RowLabel), "Sjedalo s tom oznakom već postoji u dvorani.");
        }
    }

    private void SoftDeleteSeat(Seat seat)
    {
        var deletedAt = DateTime.UtcNow;
        seat.DeletedAt = deletedAt;

        var tickets = _dbContext.Tickets
            .Where(ticket => ticket.SeatId == seat.Id && ticket.DeletedAt == null)
            .ToList();

        foreach (var ticket in tickets)
        {
            ticket.DeletedAt = deletedAt;
        }
    }
}
