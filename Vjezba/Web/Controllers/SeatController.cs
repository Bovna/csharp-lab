using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Vjezba.DAL;
using Vjezba.Model.Entities;
using Vjezba.Web.ViewModels;

namespace Vjezba.Web.Controllers;

[Route("sjedala")]
public class SeatController : Controller
{
    private readonly CinemaDbContext _dbContext;

    public SeatController(CinemaDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [Route("")]
    public IActionResult Index(SeatType? seatType, string? search, bool partial = false)
    {
        var normalizedSearch = (search ?? string.Empty).Trim();
        var query = _dbContext.Seats
            .Where(seat => seat.DeletedAt == null
                && seat.Hall.DeletedAt == null
                && seat.Hall.Cinema.DeletedAt == null)
            .Include(s => s.Hall)
                .ThenInclude(h => h.Cinema)
            .AsQueryable();

        if (seatType.HasValue)
        {
            query = query.Where(seat => seat.SeatType == seatType.Value);
        }

        if (!string.IsNullOrWhiteSpace(normalizedSearch))
        {
            if (int.TryParse(normalizedSearch, out var seatNumber))
            {
                query = query.Where(seat => seat.SeatNumber == seatNumber);
            }
            else
            {
                query = query.Where(seat =>
                    EF.Functions.Like(seat.RowLabel, $"%{normalizedSearch}%")
                    || EF.Functions.Like(seat.Hall.Name, $"%{normalizedSearch}%")
                    || EF.Functions.Like(seat.Hall.Cinema.Name, $"%{normalizedSearch}%"));
            }
        }

        var seats = query
            .OrderBy(seat => seat.Id)
            .ToList();

        ViewBag.SelectedSeatType = seatType?.ToString();
        ViewBag.Search = search;

        if (partial)
        {
            return PartialView("_IndexResults", seats);
        }

        return View(seats);
    }

    [Route("pretraga")]
    public IActionResult Search(string? query)
    {
        var normalizedQuery = (query ?? string.Empty).Trim();

        var seats = _dbContext.Seats
            .Include(seat => seat.Hall)
                .ThenInclude(hall => hall.Cinema)
            .Where(seat => seat.DeletedAt == null
                && seat.Hall.DeletedAt == null
                && seat.Hall.Cinema.DeletedAt == null)
            .Where(seat => string.IsNullOrEmpty(normalizedQuery)
                || EF.Functions.Like(seat.RowLabel, $"%{normalizedQuery}%")
                || EF.Functions.Like(seat.Hall.Name, $"%{normalizedQuery}%")
                || EF.Functions.Like(seat.Hall.Cinema.Name, $"%{normalizedQuery}%"))
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
    public IActionResult Details(int id)
    {
        var seat = _dbContext.Seats
            .Include(s => s.Hall)
                .ThenInclude(h => h.Cinema)
            .FirstOrDefault(s => s.Id == id
                && s.DeletedAt == null
                && s.Hall.DeletedAt == null
                && s.Hall.Cinema.DeletedAt == null);

        if (seat is null)
        {
            return NotFound();
        }

        return View(seat);
    }

    [Route("dodaj")]
    public IActionResult Create()
    {
        var model = new SeatFormViewModel();
        PrepareSeatForm(model, isCreate: true);
        return View(model);
    }

    [HttpPost("dodaj")]
    public IActionResult Create(SeatFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            PrepareSeatForm(model, isCreate: true);
            return View(model);
        }

        var seat = new Seat
        {
            RowLabel = model.RowLabel,
            SeatNumber = model.SeatNumber,
            SeatType = model.SeatType,
            HallId = model.HallId!.Value
        };

        _dbContext.Seats.Add(seat);
        _dbContext.SaveChanges();

        return RedirectToAction(nameof(Index));
    }

    [Route("uredi/{id}")]
    [ActionName("Edit")]
    public IActionResult EditGet(int id)
    {
        var seat = _dbContext.Seats.FirstOrDefault(s => s.Id == id && s.DeletedAt == null);

        if (seat is null)
        {
            return NotFound();
        }

        var model = new SeatFormViewModel
        {
            Id = seat.Id,
            RowLabel = seat.RowLabel,
            SeatNumber = seat.SeatNumber,
            SeatType = seat.SeatType,
            HallId = seat.HallId
        };

        PrepareSeatForm(model, isCreate: false);
        return View(model);
    }

    [HttpPost("uredi/{id}")]
    [ActionName("Edit")]
    public async Task<IActionResult> EditPost(int id)
    {
        var seat = _dbContext.Seats.FirstOrDefault(s => s.Id == id && s.DeletedAt == null);

        if (seat is null)
        {
            return NotFound();
        }

        var model = new SeatFormViewModel
        {
            Id = seat.Id,
            RowLabel = seat.RowLabel,
            SeatNumber = seat.SeatNumber,
            SeatType = seat.SeatType,
            HallId = seat.HallId
        };

        var ok = await TryUpdateModelAsync(model, string.Empty,
            m => m.RowLabel,
            m => m.SeatNumber,
            m => m.SeatType,
            m => m.HallId);

        if (ok && ModelState.IsValid)
        {
            seat.RowLabel = model.RowLabel;
            seat.SeatNumber = model.SeatNumber;
            seat.SeatType = model.SeatType;
            seat.HallId = model.HallId!.Value;

            _dbContext.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        PrepareSeatForm(model, isCreate: false);
        return View(model);
    }

    [HttpPost("obrisi/{id}")]
    public IActionResult Delete(int id)
    {
        var seat = _dbContext.Seats.FirstOrDefault(s => s.Id == id && s.DeletedAt == null);

        if (seat is null)
        {
            return NotFound();
        }

        SoftDeleteSeat(seat);
        _dbContext.SaveChanges();

        return RedirectToAction(nameof(Index));
    }

    private void PrepareSeatForm(SeatFormViewModel model, bool isCreate = false)
    {
        model.HallSelector = new AutocompleteViewModel
        {
            InputName = nameof(model.HallId),
            Label = "Dvorana",
            Endpoint = Url.Action(nameof(HallController.Search), "Hall") ?? "/dvorana/pretraga",
            SearchPlaceholder = "Pretražite dvoranu po kinu ili nazivu",
            RequiredMessage = "Dvorana je obavezna.",
            Items = BuildSelectItems(
                _dbContext.Halls
                    .Include(hall => hall.Cinema)
                    .Where(hall => hall.DeletedAt == null && hall.Cinema.DeletedAt == null)
                    .OrderBy(hall => hall.Cinema.Name)
                    .ThenBy(hall => hall.Name)
                    .Select(hall => new SelectListItem
                    {
                        Value = hall.Id.ToString(),
                        Text = hall.Cinema.Name + " - " + hall.Name,
                        Selected = model.HallId.HasValue && hall.Id == model.HallId.Value
                    })
                    .ToList(),
                model.HallId,
                isCreate)
        };
    }

    private static List<SelectListItem> BuildSelectItems(List<SelectListItem> items, int? selectedValue, bool isCreate = false)
    {
        var selectItems = new List<SelectListItem>();

        if (!isCreate)
        {
            selectItems.Add(new SelectListItem
            {
                Text = "- odaberite -",
                Value = string.Empty,
                Selected = !selectedValue.HasValue
            });
        }

        selectItems.AddRange(items);
        return selectItems;
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
