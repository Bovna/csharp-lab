using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KinoKlik.DAL;
using KinoKlik.Model.Entities;
using KinoKlik.Web.ViewModels;

namespace KinoKlik.Web.Controllers;

[AutoValidateAntiforgeryToken]
[Route("kina")]
[Authorize]
public class CinemaController : Controller
{
    private readonly CinemaDbContext _dbContext;
    private readonly ILogger<CinemaController> _logger;

    public CinemaController(CinemaDbContext dbContext, ILogger<CinemaController> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    [Route("")]
    [Route("pretraga")]
    [AllowAnonymous]
    public IActionResult Index(string? city, bool management = false, bool partial = false)
    {
        var cinemasQuery = ActiveCinemasQuery();

        if (!string.IsNullOrWhiteSpace(city))
        {
            cinemasQuery = cinemasQuery.Where(cinema => cinema.City == city);
        }

        PrepareCinemaIndex(city, null); ViewBag.Management = management;

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
    public IActionResult Search(string? query, string? city, bool management = false, bool partial = false)
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

        PrepareCinemaIndex(city, query); ViewBag.Management = management;

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

        ValidateCinemaBusinessRules(model);

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var cinema = new Cinema();
        MapCinemaForm(model, cinema);

        _dbContext.Cinemas.Add(cinema);
        _dbContext.SaveChanges();

        _logger.LogInformation(
            "Cinema created by MVC. CinemaId={CinemaId}, City={City}, UserId={UserId}",
            cinema.Id,
            cinema.City,
            GetCurrentUserId());

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

        ValidateCinemaBusinessRules(model, id);

        if (!ModelState.IsValid)
        {
            model.Id = id;
            return View(model);
        }

        MapCinemaForm(model, cinema);
        _dbContext.SaveChanges();

        _logger.LogInformation(
            "Cinema updated by MVC. CinemaId={CinemaId}, City={City}, UserId={UserId}",
            cinema.Id,
            cinema.City,
            GetCurrentUserId());

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

        var deleteSummary = SoftDeleteCinema(cinema);
        _dbContext.SaveChanges();

        _logger.LogInformation(
            "Cinema soft deleted by MVC. CinemaId={CinemaId}, DeletedHallCount={DeletedHallCount}, DeletedSeatCount={DeletedSeatCount}, DeletedScreeningCount={DeletedScreeningCount}, DeletedTicketCount={DeletedTicketCount}, UserId={UserId}",
            cinema.Id,
            deleteSummary.DeletedHallCount,
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
        cinema.Name = model.Name.Trim();
        cinema.City = model.City.Trim();
        cinema.Street = model.Street.Trim();
        cinema.HouseNumber = model.HouseNumber.Trim();
        cinema.PostalCode = model.PostalCode.Trim();
        cinema.Email = model.Email.Trim();
        cinema.Phone = model.Phone.Trim();
    }

    private void ValidateCinemaBusinessRules(CinemaFormViewModel model, int? currentCinemaId = null)
    {
        model.Name = (model.Name ?? string.Empty).Trim();
        model.City = (model.City ?? string.Empty).Trim();
        model.Email = (model.Email ?? string.Empty).Trim();

        if (!string.IsNullOrWhiteSpace(model.Email))
        {
            var normalizedEmail = model.Email.ToLower();
            var emailExists = _dbContext.Cinemas.Any(cinema =>
                cinema.DeletedAt == null
                && cinema.Email.ToLower() == normalizedEmail
                && (!currentCinemaId.HasValue || cinema.Id != currentCinemaId.Value));

            if (emailExists)
            {
                ModelState.AddModelError(nameof(model.Email), "Kino s tom email adresom već postoji.");
            }
        }

        if (!string.IsNullOrWhiteSpace(model.Name) && !string.IsNullOrWhiteSpace(model.City))
        {
            var normalizedName = model.Name.ToLower();
            var normalizedCity = model.City.ToLower();
            var cinemaExists = _dbContext.Cinemas.Any(cinema =>
                cinema.DeletedAt == null
                && cinema.Name.ToLower() == normalizedName
                && cinema.City.ToLower() == normalizedCity
                && (!currentCinemaId.HasValue || cinema.Id != currentCinemaId.Value));

            if (cinemaExists)
            {
                ModelState.AddModelError(nameof(model.Name), "Kino s tim nazivom već postoji u odabranom gradu.");
            }
        }
    }

    private (int DeletedHallCount, int DeletedSeatCount, int DeletedScreeningCount, int DeletedTicketCount) SoftDeleteCinema(Cinema cinema)
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

        return (halls.Count, seats.Count, screenings.Count, tickets.Count);
    }
}
