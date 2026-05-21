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

    [Route("")]
    public IActionResult Index(string? firstName, string? lastName, bool partial = false)
    {
        var query = _dbContext.Customers
            .Where(customer => customer.DeletedAt == null)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(firstName))
        {
            query = query.Where(customer => customer.FirstName.Contains(firstName));
        }

        if (!string.IsNullOrWhiteSpace(lastName))
        {
            query = query.Where(customer => customer.LastName.Contains(lastName));
        }

        var customers = query
            .OrderBy(customer => customer.LastName)
            .ThenBy(customer => customer.FirstName)
            .ToList();

        ViewBag.FirstName = firstName;
        ViewBag.LastName = lastName;

        if (partial)
        {
            return PartialView("_IndexResults", customers);
        }

        return View(customers);
    }

    [Route("detalji/{id}")]
    public IActionResult Details(int id)
    {
        var customer = _dbContext.Customers.FirstOrDefault(c => c.Id == id && c.DeletedAt == null);

        if (customer is null)
        {
            return NotFound();
        }

        var tickets = _dbContext.Tickets
            .Where(t => t.CustomerId == customer.Id
                && t.DeletedAt == null
                && t.Screening.DeletedAt == null
                && t.Screening.Hall.DeletedAt == null
                && t.Screening.Hall.Cinema.DeletedAt == null)
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

    [Route("dodaj")]
    public IActionResult Create()
    {
        return View(new CustomerFormViewModel
        {
            RegisteredAt = DateTime.Now
        });
    }

    [HttpPost("dodaj")]
    public IActionResult Create(CustomerFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        if (model.RegisteredAt == default)
        {
            model.RegisteredAt = DateTime.Now;
        }

        var customer = new Customer
        {
            FirstName = model.FirstName,
            LastName = model.LastName,
            City = model.City,
            Street = model.Street,
            HouseNumber = model.HouseNumber,
            PostalCode = model.PostalCode,
            Email = model.Email,
            Phone = model.Phone,
            RegisteredAt = model.RegisteredAt,
            IsLoyaltyMember = model.IsLoyaltyMember,
            LoyaltyPoints = model.LoyaltyPoints
        };

        _dbContext.Customers.Add(customer);
        _dbContext.SaveChanges();

        return RedirectToAction(nameof(Index));
    }

    [Route("uredi/{id}")]
    [ActionName("Edit")]
    public IActionResult EditGet(int id)
    {
        var customer = _dbContext.Customers.FirstOrDefault(c => c.Id == id && c.DeletedAt == null);

        if (customer is null)
        {
            return NotFound();
        }

        var model = new CustomerFormViewModel
        {
            Id = customer.Id,
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            City = customer.City,
            Street = customer.Street,
            HouseNumber = customer.HouseNumber,
            PostalCode = customer.PostalCode,
            Email = customer.Email,
            Phone = customer.Phone,
            RegisteredAt = customer.RegisteredAt,
            IsLoyaltyMember = customer.IsLoyaltyMember,
            LoyaltyPoints = customer.LoyaltyPoints
        };

        return View(model);
    }

    [HttpPost("uredi/{id}")]
    [ActionName("Edit")]
    public async Task<IActionResult> EditPost(int id)
    {
        var customer = _dbContext.Customers.FirstOrDefault(c => c.Id == id && c.DeletedAt == null);

        if (customer is null)
        {
            return NotFound();
        }

        var model = new CustomerFormViewModel
        {
            Id = customer.Id,
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            City = customer.City,
            Street = customer.Street,
            HouseNumber = customer.HouseNumber,
            PostalCode = customer.PostalCode,
            Email = customer.Email,
            Phone = customer.Phone,
            RegisteredAt = customer.RegisteredAt,
            IsLoyaltyMember = customer.IsLoyaltyMember,
            LoyaltyPoints = customer.LoyaltyPoints
        };

        var ok = await TryUpdateModelAsync(model, string.Empty,
            m => m.FirstName,
            m => m.LastName,
            m => m.City,
            m => m.Street,
            m => m.HouseNumber,
            m => m.PostalCode,
            m => m.Email,
            m => m.Phone,
            m => m.RegisteredAt,
            m => m.IsLoyaltyMember,
            m => m.LoyaltyPoints);

        if (ok && ModelState.IsValid)
        {
            customer.FirstName = model.FirstName;
            customer.LastName = model.LastName;
            customer.City = model.City;
            customer.Street = model.Street;
            customer.HouseNumber = model.HouseNumber;
            customer.PostalCode = model.PostalCode;
            customer.Email = model.Email;
            customer.Phone = model.Phone;
            customer.RegisteredAt = model.RegisteredAt;
            customer.IsLoyaltyMember = model.IsLoyaltyMember;
            customer.LoyaltyPoints = model.LoyaltyPoints;

            _dbContext.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        return View(model);
    }

    [HttpPost("obrisi/{id}")]
    public IActionResult Delete(int id)
    {
        var customer = _dbContext.Customers.FirstOrDefault(c => c.Id == id && c.DeletedAt == null);

        if (customer is null)
        {
            return NotFound();
        }

        SoftDeleteCustomer(customer);
        _dbContext.SaveChanges();

        return RedirectToAction(nameof(Index));
    }

    private void SoftDeleteCustomer(Customer customer)
    {
        var deletedAt = DateTime.UtcNow;
        customer.DeletedAt = deletedAt;

        var tickets = _dbContext.Tickets
            .Where(ticket => ticket.CustomerId == customer.Id && ticket.DeletedAt == null)
            .ToList();

        foreach (var ticket in tickets)
        {
            ticket.DeletedAt = deletedAt;
        }

        var favorites = _dbContext.CustomerFavoriteMovies
            .Where(favorite => favorite.CustomerId == customer.Id && favorite.DeletedAt == null)
            .ToList();

        foreach (var favorite in favorites)
        {
            favorite.DeletedAt = deletedAt;
        }
    }
}
