using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vjezba.DAL;
using Vjezba.Model.Entities;
using Vjezba.Web.ViewModels;

namespace Vjezba.Web.Controllers;

[Route("projekcije")]
public class ScreeningController : Controller
{
    private readonly CinemaDbContext _dbContext;

    public ScreeningController(CinemaDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet("")]
    public IActionResult Index(int? dayOfWeek)
    {
        var query = _dbContext.Screenings
            .Include(s => s.Movie)
            .Include(s => s.Hall)
                .ThenInclude(h => h.Cinema)
            .AsQueryable();

        var screenings = query
            .OrderBy(screening => screening.StartTime)
            .ToList();

        if (dayOfWeek.HasValue)
        {
            var targetDayOfWeek = (DayOfWeek)(dayOfWeek.Value % 7);

            screenings = screenings
                .Where(screening => screening.StartTime.DayOfWeek == targetDayOfWeek)
                .ToList();
        }

        ViewBag.SelectedDayOfWeek = dayOfWeek;

        return View(screenings);
    }

    [HttpGet("detalji/{id}")]
    public IActionResult Details(int id)
    {
        var screening = _dbContext.Screenings
            .Include(s => s.Movie)
            .Include(s => s.Hall)
                .ThenInclude(h => h.Cinema)
            .FirstOrDefault(s => s.Id == id);

        if (screening is null)
        {
            return NotFound();
        }

        var tickets = _dbContext.Tickets
            .Where(t => t.ScreeningId == screening.Id)
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

    [HttpGet("dodaj")]
    public IActionResult Create()
    {
        LoadScreeningFormData();
        return View(new Screening());
    }

    [HttpPost("dodaj")]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Screening screening)
    {
        if (!ModelState.IsValid)
        {
            LoadScreeningFormData();
            return View(screening);
        }

        _dbContext.Screenings.Add(screening);
        _dbContext.SaveChanges();

        return RedirectToAction(nameof(Details), new { id = screening.Id });
    }

    [HttpGet("uredi/{id}")]
    public IActionResult Edit(int id)
    {
        var screening = _dbContext.Screenings.FirstOrDefault(s => s.Id == id);

        if (screening is null)
        {
            return NotFound();
        }

        LoadScreeningFormData();
        return View(screening);
    }

    [HttpPost("uredi/{id}")]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, Screening screening)
    {
        if (id != screening.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            LoadScreeningFormData();
            return View(screening);
        }

        _dbContext.Screenings.Update(screening);
        _dbContext.SaveChanges();

        return RedirectToAction(nameof(Details), new { id = screening.Id });
    }

    [HttpPost("obrisi/{id}")]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int id)
    {
        var screening = _dbContext.Screenings.Find(id);

        if (screening is null)
        {
            return NotFound();
        }

        _dbContext.Screenings.Remove(screening);
        _dbContext.SaveChanges();

        return RedirectToAction(nameof(Index));
    }

    private void LoadScreeningFormData()
    {
        ViewBag.Movies = _dbContext.Movies
            .OrderBy(movie => movie.Title)
            .ToList();

        ViewBag.Halls = _dbContext.Halls
            .Include(hall => hall.Cinema)
            .OrderBy(hall => hall.Cinema.Name)
            .ThenBy(hall => hall.Name)
            .ToList();
    }
}
