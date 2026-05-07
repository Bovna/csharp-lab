using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vjezba.DAL;
using Vjezba.Model.Entities;
using Vjezba.Web.ViewModels;

namespace Vjezba.Web.Controllers;

[Route("kupci")]
public class CustomerController : Controller
{
    private readonly CinemaDbContext _dbContext;

    public CustomerController(CinemaDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet("")]
    public IActionResult Index(string? firstName, string? lastName)
    {
        var query = _dbContext.Customers.AsQueryable();

        if (!string.IsNullOrWhiteSpace(firstName))
        {
            query = query.Where(customer => customer.FirstName.Contains(firstName));
        }

        if (!string.IsNullOrWhiteSpace(lastName))
        {
            query = query.Where(customer => customer.LastName.Contains(lastName));
        }

        var customers = query
            .OrderBy(customer => customer.Id)
            .ToList();

        ViewBag.FirstName = firstName;
        ViewBag.LastName = lastName;

        return View(customers);
    }

    [HttpGet("detalji/{id}")]
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

    [HttpGet("dodaj")]
    public IActionResult Create()
    {
        return View(new Customer
        {
            RegisteredAt = DateTime.Now
        });
    }

    [HttpPost("dodaj")]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Customer customer)
    {
        if (!ModelState.IsValid)
        {
            return View(customer);
        }

        if (customer.RegisteredAt == default)
        {
            customer.RegisteredAt = DateTime.Now;
        }

        _dbContext.Customers.Add(customer);
        _dbContext.SaveChanges();

        return RedirectToAction(nameof(Details), new { id = customer.Id });
    }

    [HttpGet("uredi/{id}")]
    public IActionResult Edit(int id)
    {
        var customer = _dbContext.Customers.FirstOrDefault(c => c.Id == id);

        if (customer is null)
        {
            return NotFound();
        }

        return View(customer);
    }

    [HttpPost("uredi/{id}")]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, Customer customer)
    {
        if (id != customer.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return View(customer);
        }

        _dbContext.Customers.Update(customer);
        _dbContext.SaveChanges();

        return RedirectToAction(nameof(Details), new { id = customer.Id });
    }

    [HttpPost("obrisi/{id}")]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int id)
    {
        var customer = _dbContext.Customers.Find(id);

        if (customer is null)
        {
            return NotFound();
        }

        _dbContext.Customers.Remove(customer);
        _dbContext.SaveChanges();

        return RedirectToAction(nameof(Index));
    }
}
