using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vjezba.DAL;
using Vjezba.Web.ViewModels;

namespace Vjezba.Web.Controllers;

public class CustomerController : Controller
{
    private readonly CinemaDbContext _dbContext;

    public CustomerController(CinemaDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IActionResult Index()
    {
        var customers = _dbContext.Customers.ToList();

        return View(customers);
    }

    public IActionResult Details(int id)
    {
        var customer = _dbContext.Customers.FirstOrDefault(c => c.Id == id);

        if (customer is null)
        {
            return NotFound();
        }

        var tickets = _dbContext.Tickets
            .Where(t => t.CustomerId == customer.Id)
            .Include(t => t.Screening)
                .ThenInclude(s => s.Movie)
            .Include(t => t.Screening)
                .ThenInclude(s => s.Hall)
                    .ThenInclude(h => h.Cinema)
            .OrderByDescending(t => t.PurchasedAt)
            .ToList();

        var viewModel = new CustomerDetailsViewModel
        {
            Customer = customer,
            Tickets = tickets
        };

        return View(viewModel);
    }
}
