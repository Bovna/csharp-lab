using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vjezba.DAL;
using Vjezba.Model.Entities;

namespace Vjezba.Web.Controllers;

[Route("sjedala")]
public class SeatController : Controller
{
    private readonly CinemaDbContext _dbContext;

    public SeatController(CinemaDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet("")]
    public IActionResult Index(SeatType? seatType)
    {
        var query = _dbContext.Seats
            .Include(s => s.Hall)
                .ThenInclude(h => h.Cinema)
            .AsQueryable();

        if (seatType.HasValue)
        {
            query = query.Where(seat => seat.SeatType == seatType.Value);
        }

        var seats = query
            .OrderBy(seat => seat.Id)
            .ToList();

        ViewBag.SelectedSeatType = seatType?.ToString();

        return View(seats);
    }

    [HttpGet("detalji/{id}")]
    public IActionResult Details(int id)
    {
        var seat = _dbContext.Seats
            .Include(s => s.Hall)
                .ThenInclude(h => h.Cinema)
            .FirstOrDefault(s => s.Id == id);

        if (seat is null)
        {
            return NotFound();
        }

        return View(seat);
    }

    [HttpGet("dodaj")]
    public IActionResult Create()
    {
        LoadSeatFormData();
        return View(new Seat());
    }

    [HttpPost("dodaj")]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Seat seat)
    {
        if (!ModelState.IsValid)
        {
            LoadSeatFormData();
            return View(seat);
        }

        _dbContext.Seats.Add(seat);
        _dbContext.SaveChanges();

        return RedirectToAction(nameof(Details), new { id = seat.Id });
    }

    [HttpGet("uredi/{id}")]
    public IActionResult Edit(int id)
    {
        var seat = _dbContext.Seats.FirstOrDefault(s => s.Id == id);

        if (seat is null)
        {
            return NotFound();
        }

        LoadSeatFormData();
        return View(seat);
    }

    [HttpPost("uredi/{id}")]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, Seat seat)
    {
        if (id != seat.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            LoadSeatFormData();
            return View(seat);
        }

        _dbContext.Seats.Update(seat);
        _dbContext.SaveChanges();

        return RedirectToAction(nameof(Details), new { id = seat.Id });
    }

    [HttpPost("obrisi/{id}")]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int id)
    {
        var seat = _dbContext.Seats.Find(id);

        if (seat is null)
        {
            return NotFound();
        }

        _dbContext.Seats.Remove(seat);
        _dbContext.SaveChanges();

        return RedirectToAction(nameof(Index));
    }

    private void LoadSeatFormData()
    {
        ViewBag.Halls = _dbContext.Halls
            .Include(hall => hall.Cinema)
            .OrderBy(hall => hall.Cinema.Name)
            .ThenBy(hall => hall.Name)
            .ToList();
    }
}
