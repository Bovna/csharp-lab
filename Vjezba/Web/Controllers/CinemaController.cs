using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vjezba.DAL;
using Vjezba.Model.Entities;
using Vjezba.Web.ViewModels;

namespace Vjezba.Web.Controllers;

[Route("kina")]
public class CinemaController : Controller
{
    private readonly CinemaDbContext _dbContext;

    public CinemaController(CinemaDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [Route("")]
    [Route("grad={city}")]
    public IActionResult Index(string? city, string? search, bool partial = false)
    {
        var normalizedSearch = (search ?? string.Empty).Trim();
        var query = _dbContext.Cinemas
            .Where(cinema => cinema.DeletedAt == null)
            .AsQueryable();

        ViewBag.Cities = _dbContext.Cinemas
            .Where(cinema => cinema.DeletedAt == null)
            .Select(c => c.City)
            .Where(c => c != null && c != "")
            .Distinct()
            .OrderBy(c => c)
            .ToList();

        ViewBag.SelectedCity = city;
        ViewBag.Search = search;

        if (!string.IsNullOrWhiteSpace(city))
        {
            query = query.Where(c => c.City == city);
        }

        if (!string.IsNullOrWhiteSpace(normalizedSearch))
        {
            query = query.Where(cinema =>
                EF.Functions.Like(cinema.Name, $"%{normalizedSearch}%")
                || EF.Functions.Like(cinema.City, $"%{normalizedSearch}%")
                || EF.Functions.Like(cinema.Street, $"%{normalizedSearch}%"));
        }

        var cinemas = query
            .OrderBy(cinema => cinema.Id)
            .ToList();

        if (partial)
        {
            return PartialView("_IndexResults", cinemas);
        }

        return View(cinemas);
    }

    [Route("pretraga")]
    public IActionResult Search(string? query)
    {
        var normalizedQuery = (query ?? string.Empty).Trim();

        var cinemas = _dbContext.Cinemas
            .Where(cinema => cinema.DeletedAt == null)
            .Where(cinema => string.IsNullOrEmpty(normalizedQuery)
                || EF.Functions.Like(cinema.Name, $"%{normalizedQuery}%")
                || EF.Functions.Like(cinema.City, $"%{normalizedQuery}%")
                || EF.Functions.Like(cinema.Street, $"%{normalizedQuery}%"))
            .OrderBy(cinema => cinema.Name)
            .Take(12)
            .Select(cinema => new
            {
                value = cinema.Id,
                text = cinema.Name
            })
            .ToList();

        return Json(cinemas);
    }

    [Route("detalji/{id}")]
    public IActionResult Details(int id)
    {
        var cinema = _dbContext.Cinemas
            .Where(c => c.DeletedAt == null)
            .Include(c => c.Halls.Where(hall => hall.DeletedAt == null))
            .FirstOrDefault(c => c.Id == id);

        if (cinema is null)
        {
            return NotFound();
        }

        return View(cinema);
    }

    [Route("dodaj")]
    public IActionResult Create()
    {
        return View(new CinemaFormViewModel());
    }

    [HttpPost("dodaj")]
    public IActionResult Create(CinemaFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var cinema = new Cinema
        {
            Name = model.Name,
            City = model.City,
            Street = model.Street,
            HouseNumber = model.HouseNumber,
            PostalCode = model.PostalCode,
            Email = model.Email,
            Phone = model.Phone
        };

        _dbContext.Cinemas.Add(cinema);
        _dbContext.SaveChanges();

        return RedirectToAction(nameof(Index));
    }

    [Route("uredi/{id}")]
    [ActionName("Edit")]
    public IActionResult EditGet(int id)
    {
        var cinema = _dbContext.Cinemas.FirstOrDefault(c => c.Id == id && c.DeletedAt == null);

        if (cinema is null)
        {
            return NotFound();
        }

        var model = new CinemaFormViewModel
        {
            Id = cinema.Id,
            Name = cinema.Name,
            City = cinema.City,
            Street = cinema.Street,
            HouseNumber = cinema.HouseNumber,
            PostalCode = cinema.PostalCode,
            Email = cinema.Email,
            Phone = cinema.Phone
        };

        return View(model);
    }

    [HttpPost("uredi/{id}")]
    [ActionName("Edit")]
    public async Task<IActionResult> EditPost(int id)
    {
        var cinema = _dbContext.Cinemas.FirstOrDefault(c => c.Id == id && c.DeletedAt == null);

        if (cinema is null)
        {
            return NotFound();
        }

        var model = new CinemaFormViewModel
        {
            Id = cinema.Id,
            Name = cinema.Name,
            City = cinema.City,
            Street = cinema.Street,
            HouseNumber = cinema.HouseNumber,
            PostalCode = cinema.PostalCode,
            Email = cinema.Email,
            Phone = cinema.Phone
        };

        var ok = await TryUpdateModelAsync(model, string.Empty,
            m => m.Name,
            m => m.City,
            m => m.Street,
            m => m.HouseNumber,
            m => m.PostalCode,
            m => m.Email,
            m => m.Phone);

        if (ok && ModelState.IsValid)
        {
            cinema.Name = model.Name;
            cinema.City = model.City;
            cinema.Street = model.Street;
            cinema.HouseNumber = model.HouseNumber;
            cinema.PostalCode = model.PostalCode;
            cinema.Email = model.Email;
            cinema.Phone = model.Phone;

            _dbContext.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        return View(model);
    }

    [HttpPost("izbrisi/{id}")]
    public IActionResult Delete(int id)
    {
        var cinema = _dbContext.Cinemas.FirstOrDefault(c => c.Id == id && c.DeletedAt == null);

        if (cinema is null)
        {
            return NotFound();
        }

        SoftDeleteCinema(cinema);
        _dbContext.SaveChanges();

        return RedirectToAction(nameof(Index));
    }

    private void SoftDeleteCinema(Cinema cinema)
    {
        var deletedAt = DateTime.UtcNow;
        cinema.DeletedAt = deletedAt;

        var halls = _dbContext.Halls
            .Where(hall => hall.CinemaId == cinema.Id && hall.DeletedAt == null)
            .ToList();

        foreach (var hall in halls)
        {
            hall.DeletedAt = deletedAt;
        }

        var hallIds = halls.Select(hall => hall.Id).ToList();

        var seats = _dbContext.Seats
            .Where(seat => hallIds.Contains(seat.HallId) && seat.DeletedAt == null)
            .ToList();

        foreach (var seat in seats)
        {
            seat.DeletedAt = deletedAt;
        }

        var screenings = _dbContext.Screenings
            .Where(screening => hallIds.Contains(screening.HallId) && screening.DeletedAt == null)
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
