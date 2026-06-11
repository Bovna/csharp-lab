using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vjezba.DAL;
using Vjezba.Model.Entities;
using Vjezba.Web.ViewModels;

namespace Vjezba.Web.Controllers;

[Route("kina")]
[Authorize]
public class CinemaController : Controller
{
    private readonly CinemaDbContext _dbContext;

    public CinemaController(CinemaDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [Route("")]
    [Route("pretraga")]
    [AllowAnonymous]
    public IActionResult Index(string? city, bool partial = false)
    {
        var cinemasQuery = ActiveCinemasQuery();

        if (!string.IsNullOrWhiteSpace(city))
        {
            cinemasQuery = cinemasQuery.Where(cinema => cinema.City == city);
        }

        PrepareCinemaIndex(city, null);

        var cinemas = cinemasQuery
            .OrderBy(cinema => cinema.Id)
            .ToList();

        if (partial)
        {
            return PartialView("_IndexResults", cinemas);
        }

        return View(cinemas);
    }

    [HttpGet("rezultati")]
    [AllowAnonymous]
    public IActionResult Search(string? query, string? city, bool partial = false)
    {
        var normalizedQuery = (query ?? string.Empty).Trim();
        var cinemasQuery = ActiveCinemasQuery();

        if (!string.IsNullOrWhiteSpace(city))
        {
            cinemasQuery = cinemasQuery.Where(cinema => cinema.City == city);
        }

        if (!string.IsNullOrWhiteSpace(normalizedQuery))
        {
            cinemasQuery = cinemasQuery.Where(cinema =>
                cinema.Name.Contains(normalizedQuery) ||
                cinema.City.Contains(normalizedQuery) ||
                cinema.Street.Contains(normalizedQuery));
        }

        PrepareCinemaIndex(city, query);

        var cinemas = cinemasQuery
            .OrderBy(cinema => cinema.Id)
            .ToList();

        if (partial)
        {
            return PartialView("_IndexResults", cinemas);
        }

        return View(nameof(Index), cinemas);
    }

    [HttpGet("autocomplete")]
    [AllowAnonymous]
    public IActionResult Autocomplete(string? query)
    {
        var normalizedQuery = (query ?? string.Empty).Trim();

        var cinemas = ActiveCinemasQuery()
            .Where(cinema => string.IsNullOrEmpty(normalizedQuery)
                || cinema.Name.Contains(normalizedQuery)
                || cinema.City.Contains(normalizedQuery)
                || cinema.Street.Contains(normalizedQuery))
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
    [Authorize]
    public IActionResult Details(int id)
    {
        var cinema = ActiveCinemasQuery()
            .Include(cinema => cinema.Halls.Where(hall => hall.DeletedAt == null))
            .FirstOrDefault(cinema => cinema.Id == id);

        if (cinema is null)
        {
            return NotFound();
        }

        return View(cinema);
    }

    [Route("dodaj")]
    [Authorize(Roles = "Admin,Manager")]
    public IActionResult Create()
    {
        return View(new CinemaFormViewModel());
    }

    [HttpPost("dodaj")]
    [Authorize(Roles = "Admin,Manager")]
    public IActionResult Create(CinemaFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var cinema = new Cinema();
        MapCinemaForm(model, cinema);

        _dbContext.Cinemas.Add(cinema);
        _dbContext.SaveChanges();

        return RedirectToAction(nameof(Index));
    }

    [Route("uredi/{id}")]
    [ActionName("Edit")]
    [Authorize(Roles = "Admin,Manager")]
    public IActionResult EditGet(int id)
    {
        var cinema = ActiveCinemasQuery().FirstOrDefault(cinema => cinema.Id == id);

        if (cinema is null)
        {
            return NotFound();
        }

        return View(ToCinemaForm(cinema));
    }

    [HttpPost("uredi/{id}")]
    [ActionName("Edit")]
    [Authorize(Roles = "Admin,Manager")]
    public IActionResult EditPost(int id, CinemaFormViewModel model)
    {
        var cinema = ActiveCinemasQuery().FirstOrDefault(cinema => cinema.Id == id);

        if (cinema is null)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            model.Id = id;
            return View(model);
        }

        MapCinemaForm(model, cinema);
        _dbContext.SaveChanges();

        return RedirectToAction(nameof(Index));
    }

    [HttpPost("obrisi/{id}")]
    [Authorize(Roles = "Admin")]
    public IActionResult Delete(int id)
    {
        var cinema = ActiveCinemasQuery().FirstOrDefault(cinema => cinema.Id == id);

        if (cinema is null)
        {
            return NotFound();
        }

        SoftDeleteCinema(cinema);
        _dbContext.SaveChanges();

        return RedirectToAction(nameof(Index));
    }

    private IQueryable<Cinema> ActiveCinemasQuery()
    {
        return _dbContext.Cinemas.Where(cinema => cinema.DeletedAt == null);
    }

    private void PrepareCinemaIndex(string? city, string? search)
    {
        ViewBag.Cities = ActiveCinemasQuery()
            .Select(cinema => cinema.City)
            .Where(cityName => cityName != null && cityName != "")
            .Distinct()
            .OrderBy(cityName => cityName)
            .ToList();
        ViewBag.SelectedCity = city;
        ViewBag.Search = search;
    }

    private static CinemaFormViewModel ToCinemaForm(Cinema cinema)
    {
        return new CinemaFormViewModel
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
    }

    private static void MapCinemaForm(CinemaFormViewModel model, Cinema cinema)
    {
        cinema.Name = model.Name;
        cinema.City = model.City;
        cinema.Street = model.Street;
        cinema.HouseNumber = model.HouseNumber;
        cinema.PostalCode = model.PostalCode;
        cinema.Email = model.Email;
        cinema.Phone = model.Phone;
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
