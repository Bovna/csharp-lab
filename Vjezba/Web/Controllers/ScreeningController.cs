using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vjezba.DAL;
using Vjezba.Web.ViewModels;

namespace Vjezba.Web.Controllers;

public class ScreeningController : Controller
{
    private readonly CinemaDbContext _dbContext;

    public ScreeningController(CinemaDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IActionResult Index()
    {
        var screenings = _dbContext.Screenings
            .Include(s => s.Movie)
            .Include(s => s.Hall)
                .ThenInclude(h => h.Cinema)
            .ToList();

        return View(screenings);
    }

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
}
