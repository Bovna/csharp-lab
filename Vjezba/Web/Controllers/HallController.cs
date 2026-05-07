using Microsoft.AspNetCore.Mvc;
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

    [HttpGet("")]
    public IActionResult Index(string? supports3D)
    {
        var query = _dbContext.Halls
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

        var halls = query
            .OrderBy(hall => hall.Id)
            .ToList();

        ViewBag.Supports3D = supports3D;

        return View(halls);
    }

    [HttpGet("detalji/{id}")]
    public IActionResult Details(int id)
    {
        var hall = _dbContext.Halls
            .Include(h => h.Cinema)
            .FirstOrDefault(h => h.Id == id);

        if (hall is null)
        {
            return NotFound();
        }

        var seats = _dbContext.Seats
            .Where(s => s.HallId == hall.Id)
            .OrderBy(s => s.RowLabel)
            .ThenBy(s => s.SeatNumber)
            .ToList();

        var screenings = _dbContext.Screenings
            .Where(s => s.HallId == hall.Id)
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

    [HttpGet("dodaj")]
    public IActionResult Create()
    {
        ViewBag.Cinemas = _dbContext.Cinemas
            .OrderBy(cinema => cinema.Name)
            .ToList();

        return View(new Hall());
    }

    [HttpPost("dodaj")]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Hall hall)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Cinemas = _dbContext.Cinemas.OrderBy(cinema => cinema.Name).ToList();
            return View(hall);
        }

        _dbContext.Halls.Add(hall);
        _dbContext.SaveChanges();

        return RedirectToAction(nameof(Details), new { id = hall.Id });
    }

    [HttpGet("uredi/{id}")]
    public IActionResult Edit(int id)
    {
        var hall = _dbContext.Halls.FirstOrDefault(h => h.Id == id);

        if (hall is null)
        {
            return NotFound();
        }

        ViewBag.Cinemas = _dbContext.Cinemas
            .OrderBy(cinema => cinema.Name)
            .ToList();

        return View(hall);
    }

    [HttpPost("uredi/{id}")]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, Hall hall)
    {
        if (id != hall.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            ViewBag.Cinemas = _dbContext.Cinemas.OrderBy(cinema => cinema.Name).ToList();
            return View(hall);
        }

        _dbContext.Halls.Update(hall);
        _dbContext.SaveChanges();

        return RedirectToAction(nameof(Details), new { id = hall.Id });
    }

    [HttpPost("izbrisi/{id}")]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int id)
    {
        var hall = _dbContext.Halls.Find(id);

        if (hall is null)
        {
            return NotFound();
        }

        _dbContext.Halls.Remove(hall);
        _dbContext.SaveChanges();

        return RedirectToAction(nameof(Index));
    }
}
