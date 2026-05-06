using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vjezba.DAL;

namespace Vjezba.Web.Controllers;

public class TicketController : Controller
{
    private readonly CinemaDbContext _dbContext;

    public TicketController(CinemaDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IActionResult Index()
    {
        var tickets = _dbContext.Tickets
            .Include(t => t.Customer)
            .Include(t => t.Seat)
            .Include(t => t.Screening)
                .ThenInclude(s => s.Movie)
            .Include(t => t.Screening)
                .ThenInclude(s => s.Hall)
                    .ThenInclude(h => h.Cinema)
            .ToList();

        return View(tickets);
    }

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
}
