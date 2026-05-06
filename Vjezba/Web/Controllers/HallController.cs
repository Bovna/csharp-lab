using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vjezba.DAL;
using Vjezba.Web.ViewModels;

namespace Vjezba.Web.Controllers;

public class HallController : Controller
{
    private readonly CinemaDbContext _dbContext;

    public HallController(CinemaDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IActionResult Index()
    {
        var halls = _dbContext.Halls
            .Include(h => h.Cinema)
            .ToList();

        return View(halls);
    }

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
}
