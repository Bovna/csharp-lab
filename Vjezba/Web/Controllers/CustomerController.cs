using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vjezba.DAL;
using Vjezba.Model.Entities;
using Vjezba.Web.ViewModels;

namespace Vjezba.Web.Controllers;

[Route("kupci")]
[Authorize]
public class CustomerController : Controller
{
    private readonly CinemaDbContext _dbContext;

    public CustomerController(CinemaDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [Route("")]
    [Route("pretraga")]
    [AllowAnonymous]
    public IActionResult Index(bool? loyaltyMember, bool partial = false)
    {
        var customersQuery = ActiveCustomersQuery();

        if (loyaltyMember.HasValue)
        {
            customersQuery = customersQuery.Where(customer => customer.IsLoyaltyMember == loyaltyMember.Value);
        }

        ViewBag.SelectedLoyaltyMember = loyaltyMember;
        ViewBag.Search = null;

        var customers = customersQuery.OrderBy(customer => customer.Id).ToList();

        if (partial)
        {
            return PartialView("_IndexResults", customers);
        }

        return View(customers);
    }

    [HttpGet("rezultati")]
    [AllowAnonymous]
    public IActionResult Search(string? query, bool? loyaltyMember, bool partial = false)
    {
        var normalizedQuery = (query ?? string.Empty).Trim();
        var customersQuery = ActiveCustomersQuery();

        if (loyaltyMember.HasValue)
        {
            customersQuery = customersQuery.Where(customer => customer.IsLoyaltyMember == loyaltyMember.Value);
        }

        if (!string.IsNullOrWhiteSpace(normalizedQuery))
        {
            customersQuery = customersQuery.Where(customer =>
                (customer.FirstName + " " + customer.LastName).Contains(normalizedQuery) ||
                customer.City.Contains(normalizedQuery) ||
                customer.Email.Contains(normalizedQuery));
        }

        ViewBag.SelectedLoyaltyMember = loyaltyMember;
        ViewBag.Search = query;

        var customers = customersQuery.OrderBy(customer => customer.Id).ToList();

        if (partial)
        {
            return PartialView("_IndexResults", customers);
        }

        return View(nameof(Index), customers);
    }

    [HttpGet("autocomplete")]
    [AllowAnonymous]
    public IActionResult Autocomplete(string? query)
    {
        var normalizedQuery = (query ?? string.Empty).Trim();

        var customers = ActiveCustomersQuery()
            .Where(customer => string.IsNullOrEmpty(normalizedQuery)
                || (customer.FirstName + " " + customer.LastName).Contains(normalizedQuery)
                || customer.City.Contains(normalizedQuery)
                || customer.Email.Contains(normalizedQuery))
            .OrderBy(customer => customer.LastName)
            .ThenBy(customer => customer.FirstName)
            .Take(12)
            .Select(customer => new
            {
                value = customer.Id,
                text = customer.FirstName + " " + customer.LastName
            })
            .ToList();

        return Json(customers);
    }

    [Route("detalji/{id}")]
    [Authorize]
    public IActionResult Details(int id)
    {
        var customer = ActiveCustomersQuery().FirstOrDefault(customer => customer.Id == id);

        if (customer is null)
        {
            return NotFound();
        }

        var tickets = _dbContext.Tickets
            .Where(ticket => ticket.CustomerId == customer.Id
                && ticket.DeletedAt == null
                && ticket.Screening.DeletedAt == null
                && ticket.Screening.Hall.DeletedAt == null
                && ticket.Screening.Hall.Cinema.DeletedAt == null)
            .Include(ticket => ticket.Screening)
                .ThenInclude(screening => screening.Movie)
            .Include(ticket => ticket.Screening)
                .ThenInclude(screening => screening.Hall)
                    .ThenInclude(hall => hall.Cinema)
            .OrderByDescending(ticket => ticket.PurchasedAt)
            .ToList();

        return View(new CustomerDetailsViewModel
        {
            Customer = customer,
            Tickets = tickets
        });
    }

    [Route("dodaj")]
    [Authorize(Roles = "Admin,Manager")]
    public IActionResult Create()
    {
        return View(new CustomerFormViewModel
        {
            RegisteredAt = DateTime.Now
        });
    }

    [HttpPost("dodaj")]
    [Authorize(Roles = "Admin,Manager")]
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

        var customer = new Customer();
        MapCustomerForm(model, customer);

        _dbContext.Customers.Add(customer);
        _dbContext.SaveChanges();

        return RedirectToAction(nameof(Index));
    }

    [Route("uredi/{id}")]
    [ActionName("Edit")]
    [Authorize(Roles = "Admin,Manager")]
    public IActionResult EditGet(int id)
    {
        var customer = ActiveCustomersQuery().FirstOrDefault(customer => customer.Id == id);

        if (customer is null)
        {
            return NotFound();
        }

        return View(ToCustomerForm(customer));
    }

    [HttpPost("uredi/{id}")]
    [ActionName("Edit")]
    [Authorize(Roles = "Admin,Manager")]
    public IActionResult EditPost(int id, CustomerFormViewModel model)
    {
        var customer = ActiveCustomersQuery().FirstOrDefault(customer => customer.Id == id);

        if (customer is null)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            model.Id = id;
            return View(model);
        }

        MapCustomerForm(model, customer);
        _dbContext.SaveChanges();

        return RedirectToAction(nameof(Index));
    }

    [HttpPost("obrisi/{id}")]
    [Authorize(Roles = "Admin")]
    public IActionResult Delete(int id)
    {
        var customer = ActiveCustomersQuery().FirstOrDefault(customer => customer.Id == id);

        if (customer is null)
        {
            return NotFound();
        }

        SoftDeleteCustomer(customer);
        _dbContext.SaveChanges();

        return RedirectToAction(nameof(Index));
    }

    private IQueryable<Customer> ActiveCustomersQuery()
    {
        return _dbContext.Customers.Where(customer => customer.DeletedAt == null);
    }

    private static CustomerFormViewModel ToCustomerForm(Customer customer)
    {
        return new CustomerFormViewModel
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
    }

    private static void MapCustomerForm(CustomerFormViewModel model, Customer customer)
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
