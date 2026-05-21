using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Vjezba.DAL;
using Vjezba.Model.Entities;
using Vjezba.Web.ViewModels;

namespace Vjezba.Web.Controllers;

[Route("dvorana")]
public class HallController : Controller
{
    private readonly CinemaDbContext _dbContext;

    public HallController(CinemaDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [Route("")]
    public IActionResult Index(string? supports3D, string? search, bool partial = false)
    {
        var normalizedSearch = (search ?? string.Empty).Trim();
        var query = _dbContext.Halls
            .Where(hall => hall.DeletedAt == null && hall.Cinema.DeletedAt == null)
            .Include(h => h.Cinema)
            .AsQueryable();

        if (string.Equals(supports3D, "true", StringComparison.OrdinalIgnoreCase))
        {
            query = query.Where(hall => hall.Supports3D);
        }
        else if (string.Equals(supports3D, "false", StringComparison.OrdinalIgnoreCase))
        {
            query = query.Where(hall => !hall.Supports3D);
        }

        if (!string.IsNullOrWhiteSpace(normalizedSearch))
        {
            query = query.Where(hall => EF.Functions.Like(hall.Name, $"%{normalizedSearch}%")
                || EF.Functions.Like(hall.Cinema.Name, $"%{normalizedSearch}%"));
        }

        var halls = query
            .OrderBy(hall => hall.Cinema.Name)
            .ThenBy(hall => hall.Name)
            .ToList();

        ViewBag.Supports3D = supports3D;
        ViewBag.Search = search;

        if (partial)
        {
            return PartialView("_IndexResults", halls);
        }

        return View(halls);
    }

    [Route("detalji/{id}")]
    public IActionResult Details(int id)
    {
        var hall = _dbContext.Halls
            .Include(h => h.Cinema)
            .FirstOrDefault(h => h.Id == id && h.DeletedAt == null && h.Cinema.DeletedAt == null);

        if (hall is null)
        {
            return NotFound();
        }

        var seats = _dbContext.Seats
            .Where(s => s.HallId == hall.Id && s.DeletedAt == null)
            .OrderBy(s => s.RowLabel)
            .ThenBy(s => s.SeatNumber)
            .ToList();

        var screenings = _dbContext.Screenings
            .Where(s => s.HallId == hall.Id && s.DeletedAt == null && s.Movie.DeletedAt == null)
            .Include(s => s.Movie)
            .OrderBy(s => s.StartTime)
            .ToList();

        var viewModel = new HallDetailsViewModel
        {
            Hall = hall,
            Seats = seats,
            Screenings = screenings
        };

        return View(viewModel);
    }

    [Route("dodaj")]
    public IActionResult Create()
    {
        var model = new HallFormViewModel();
        PrepareHallForm(model, isCreate: true);
        return View(model);
    }

    [HttpPost("dodaj")]
    public IActionResult Create(HallFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            PrepareHallForm(model, isCreate: true);
            return View(model);
        }

        var hall = new Hall
        {
            Name = model.Name,
            Capacity = model.Capacity,
            Supports3D = model.Supports3D,
            CinemaId = model.CinemaId!.Value
        };

        _dbContext.Halls.Add(hall);
        _dbContext.SaveChanges();

        return RedirectToAction(nameof(Index));
    }

    [Route("uredi/{id}")]
    [ActionName("Edit")]
    public IActionResult EditGet(int id)
    {
        var hall = _dbContext.Halls.FirstOrDefault(h => h.Id == id && h.DeletedAt == null);

        if (hall is null)
        {
            return NotFound();
        }

        var model = new HallFormViewModel
        {
            Id = hall.Id,
            Name = hall.Name,
            Capacity = hall.Capacity,
            Supports3D = hall.Supports3D,
            CinemaId = hall.CinemaId
        };

        PrepareHallForm(model, isCreate: false);
        return View(model);
    }

    [HttpPost("uredi/{id}")]
    [ActionName("Edit")]
    public async Task<IActionResult> EditPost(int id)
    {
        var hall = _dbContext.Halls.FirstOrDefault(h => h.Id == id && h.DeletedAt == null);

        if (hall is null)
        {
            return NotFound();
        }

        var model = new HallFormViewModel
        {
            Id = hall.Id,
            Name = hall.Name,
            Capacity = hall.Capacity,
            Supports3D = hall.Supports3D,
            CinemaId = hall.CinemaId
        };

        var ok = await TryUpdateModelAsync(model, string.Empty,
            m => m.Name,
            m => m.Capacity,
            m => m.Supports3D,
            m => m.CinemaId);

        if (ok && ModelState.IsValid)
        {
            hall.Name = model.Name;
            hall.Capacity = model.Capacity;
            hall.Supports3D = model.Supports3D;
            hall.CinemaId = model.CinemaId!.Value;

            _dbContext.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        PrepareHallForm(model, isCreate: false);
        return View(model);
    }

    [HttpPost("izbrisi/{id}")]
    public IActionResult Delete(int id)
    {
        var hall = _dbContext.Halls.FirstOrDefault(h => h.Id == id && h.DeletedAt == null);

        if (hall is null)
        {
            return NotFound();
        }

        SoftDeleteHall(hall);
        _dbContext.SaveChanges();

        return RedirectToAction(nameof(Index));
    }

    private void PrepareHallForm(HallFormViewModel model, bool isCreate = false)
    {
        model.CinemaSelector = new AutocompleteViewModel
        {
            InputName = nameof(model.CinemaId),
            Label = "Kino",
            Endpoint = Url.Action(nameof(CinemaController.Index), "Cinema") ?? "/kina",
            SearchPlaceholder = "Pretražite kino po nazivu",
            RequiredMessage = "Kino je obavezno.",
            Items = BuildSelectItems(
                _dbContext.Cinemas
                    .Where(cinema => cinema.DeletedAt == null)
                    .OrderBy(cinema => cinema.Name)
                    .Select(cinema => new SelectListItem
                    {
                        Value = cinema.Id.ToString(),
                        Text = cinema.Name,
                        Selected = model.CinemaId.HasValue && cinema.Id == model.CinemaId.Value
                    })
                    .ToList(),
                model.CinemaId,
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

    private void SoftDeleteHall(Hall hall)
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
    }
}
