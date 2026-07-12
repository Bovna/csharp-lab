using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KinoKlik.DAL;
using KinoKlik.Model.Entities;
using KinoKlik.Web.ViewModels;

namespace KinoKlik.Web.Controllers;

[AutoValidateAntiforgeryToken]
[Route("kupci")]
[Authorize(Roles = "Admin,Manager")]
public class CustomerController : Controller
{
    private readonly CinemaDbContext _dbContext;
    private readonly ILogger<CustomerController> _logger;

    public CustomerController(CinemaDbContext dbContext, ILogger<CustomerController> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    [Route("")]
    [Route("pretraga")]
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

        ValidateCustomerBusinessRules(model);

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

        _logger.LogInformation(
            "Customer created by MVC. CustomerId={CustomerId}, IsLoyaltyMember={IsLoyaltyMember}, UserId={UserId}",
            customer.Id,
            customer.IsLoyaltyMember,
            GetCurrentUserId());

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

        ValidateCustomerBusinessRules(model, id);

        if (!ModelState.IsValid)
        {
            model.Id = id;
            return View(model);
        }

        MapCustomerForm(model, customer);
        _dbContext.SaveChanges();

        _logger.LogInformation(
            "Customer updated by MVC. CustomerId={CustomerId}, IsLoyaltyMember={IsLoyaltyMember}, UserId={UserId}",
            customer.Id,
            customer.IsLoyaltyMember,
            GetCurrentUserId());

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

        var deleteSummary = SoftDeleteCustomer(customer);
        _dbContext.SaveChanges();

        _logger.LogInformation(
            "Customer soft deleted by MVC. CustomerId={CustomerId}, DeletedTicketCount={DeletedTicketCount}, DeletedFavoriteCount={DeletedFavoriteCount}, UserId={UserId}",
            customer.Id,
            deleteSummary.DeletedTicketCount,
            deleteSummary.DeletedFavoriteCount,
            GetCurrentUserId());

        return RedirectToAction(nameof(Index));
    }

    private string GetCurrentUserId()
    {
        return User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "unknown";
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
        customer.FirstName = model.FirstName.Trim();
        customer.LastName = model.LastName.Trim();
        customer.City = model.City.Trim();
        customer.Street = model.Street.Trim();
        customer.HouseNumber = model.HouseNumber.Trim();
        customer.PostalCode = model.PostalCode.Trim();
        customer.Email = model.Email.Trim();
        customer.Phone = model.Phone.Trim();
        customer.RegisteredAt = model.RegisteredAt;
        customer.IsLoyaltyMember = model.IsLoyaltyMember;
        customer.LoyaltyPoints = model.LoyaltyPoints;
    }

    private void ValidateCustomerBusinessRules(CustomerFormViewModel model, int? currentCustomerId = null)
    {
        model.Email = (model.Email ?? string.Empty).Trim();

        if (!string.IsNullOrWhiteSpace(model.Email))
        {
            var normalizedEmail = model.Email.ToLower();
            var emailExists = _dbContext.Customers.Any(customer =>
                customer.DeletedAt == null
                && customer.Email.ToLower() == normalizedEmail
                && (!currentCustomerId.HasValue || customer.Id != currentCustomerId.Value));

            if (emailExists)
            {
                ModelState.AddModelError(nameof(model.Email), "Kupac s tom email adresom već postoji.");
            }
        }

        if (model.LoyaltyPoints < 0)
        {
            ModelState.AddModelError(nameof(model.LoyaltyPoints), "Broj loyalty bodova ne može biti negativan.");
        }
    }

    private (int DeletedTicketCount, int DeletedFavoriteCount) SoftDeleteCustomer(Customer customer)
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

        return (tickets.Count, favorites.Count);
    }
}
