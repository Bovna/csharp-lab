using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vjezba.DAL;
using Vjezba.Model.Entities;

namespace Vjezba.Web.Controllers;

[Route("ulaznice")]
public class TicketController : Controller
{
    private readonly CinemaDbContext _dbContext;

    public TicketController(CinemaDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet("")]
    public IActionResult Index(decimal? price)
    {
        var query = _dbContext.Tickets
            .Include(t => t.Customer)
            .Include(t => t.Seat)
            .Include(t => t.Screening)
                .ThenInclude(s => s.Movie)
            .Include(t => t.Screening)
                .ThenInclude(s => s.Hall)
                    .ThenInclude(h => h.Cinema)
            .AsQueryable();

        if (price.HasValue)
        {
            query = query.Where(ticket => ticket.Price <= price.Value);
        }

        var tickets = query
            .OrderBy(ticket => ticket.Id)
            .ToList();

        ViewBag.SelectedPrice = price?.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture);

        return View(tickets);
    }

    [HttpGet("detalji/{id}")]
    public IActionResult Details(int id)
    {
        var ticket = _dbContext.Tickets
            .Include(t => t.Customer)
            .Include(t => t.Seat)
            .Include(t => t.Screening)
                .ThenInclude(s => s.Movie)
            .Include(t => t.Screening)
                .ThenInclude(s => s.Hall)
                    .ThenInclude(h => h.Cinema)
            .FirstOrDefault(t => t.Id == id);

        if (ticket is null)
        {
            return NotFound();
        }

        return View(ticket);
    }

    [HttpGet("dodaj")]
    public IActionResult Create()
    {
        LoadTicketFormData();
        return View(new Ticket
        {
            PurchasedAt = DateTime.Now,
            Status = TicketStatus.Active
        });
    }

    [HttpPost("dodaj")]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Ticket ticket)
    {
        if (!ModelState.IsValid)
        {
            LoadTicketFormData();
            return View(ticket);
        }

        _dbContext.Tickets.Add(ticket);
        _dbContext.SaveChanges();

        return RedirectToAction(nameof(Details), new { id = ticket.Id });
    }

    [HttpGet("uredi/{id}")]
    public IActionResult Edit(int id)
    {
        var ticket = _dbContext.Tickets.FirstOrDefault(t => t.Id == id);

        if (ticket is null)
        {
            return NotFound();
        }

        LoadTicketFormData();
        return View(ticket);
    }

    [HttpPost("uredi/{id}")]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, Ticket ticket)
    {
        if (id != ticket.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            LoadTicketFormData();
            return View(ticket);
        }

        _dbContext.Tickets.Update(ticket);
        _dbContext.SaveChanges();

        return RedirectToAction(nameof(Details), new { id = ticket.Id });
    }

    [HttpPost("obrisi/{id}")]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int id)
    {
        var ticket = _dbContext.Tickets.Find(id);

        if (ticket is null)
        {
            return NotFound();
        }

        _dbContext.Tickets.Remove(ticket);
        _dbContext.SaveChanges();

        return RedirectToAction(nameof(Index));
    }

    private void LoadTicketFormData()
    {
        ViewBag.Customers = _dbContext.Customers
            .OrderBy(customer => customer.LastName)
            .ThenBy(customer => customer.FirstName)
            .ToList();

        ViewBag.Screenings = _dbContext.Screenings
            .Include(screening => screening.Movie)
            .Include(screening => screening.Hall)
                .ThenInclude(hall => hall.Cinema)
            .OrderBy(screening => screening.StartTime)
            .ToList();

        ViewBag.Seats = _dbContext.Seats
            .Include(seat => seat.Hall)
                .ThenInclude(hall => hall.Cinema)
            .OrderBy(seat => seat.Hall.Cinema.Name)
            .ThenBy(seat => seat.Hall.Name)
            .ThenBy(seat => seat.RowLabel)
            .ThenBy(seat => seat.SeatNumber)
            .ToList();
    }
}
