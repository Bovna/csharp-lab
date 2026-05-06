using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vjezba.DAL;

namespace Vjezba.Web.Controllers;

public class SeatController : Controller
{
    private readonly CinemaDbContext _dbContext;

    public SeatController(CinemaDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IActionResult Index()
    {
        var seats = _dbContext.Seats
            .Include(s => s.Hall)
                .ThenInclude(h => h.Cinema)
            .ToList();

        return View(seats);
    }

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
}
