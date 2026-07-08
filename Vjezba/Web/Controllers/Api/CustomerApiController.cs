using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vjezba.DAL;
using Vjezba.Model.Entities;
using Vjezba.Web.DTOs;

namespace Vjezba.Web.Controllers.Api;

[ApiController]
[Authorize(Roles = "Admin,Manager")]
[Route("api/kupci")]
public class CustomerApiController : ControllerBase
{
    private readonly CinemaDbContext _dbContext;

    public CustomerApiController(CinemaDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public ActionResult<IEnumerable<CustomerDTO>> Get(bool? loyaltyMember)
    {
        var customersQuery = ActiveCustomersQuery();

        if (loyaltyMember.HasValue)
        {
            customersQuery = customersQuery.Where(customer => customer.IsLoyaltyMember == loyaltyMember.Value);
        }

        var customers = customersQuery
            .OrderBy(customer => customer.Id)
            .ToList()
            .Select(ToDTO)
            .ToList();

        return Ok(customers);
    }

    [HttpGet("{id}")]
    public ActionResult<CustomerDTO> Get(int id)
    {
        var customer = ActiveCustomersQuery().FirstOrDefault(customer => customer.Id == id);

        if (customer is null)
        {
            return NotFound();
        }

        return Ok(ToDTO(customer));
    }

    [HttpGet("pretraga/{query}")]
    public ActionResult<IEnumerable<CustomerDTO>> Search(string query, bool? loyaltyMember)
    {
        var normalizedQuery = query.Trim();

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

        var customers = customersQuery
            .OrderBy(customer => customer.Id)
            .ToList()
            .Select(ToDTO)
            .ToList();

        return Ok(customers);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Manager")]
    public ActionResult<CustomerDTO> Post([FromBody] CustomerWriteDTO dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var validationError = ValidateCustomerWriteDto(dto);
        if (validationError is not null)
        {
            return BadRequest(validationError);
        }

        var customer = new Customer
        {
            FirstName = dto.FirstName.Trim(),
            LastName = dto.LastName.Trim(),
            City = dto.City.Trim(),
            Street = dto.Street.Trim(),
            HouseNumber = dto.HouseNumber.Trim(),
            PostalCode = dto.PostalCode.Trim(),
            Email = dto.Email.Trim(),
            Phone = dto.Phone.Trim(),
            RegisteredAt = dto.RegisteredAt == default ? DateTime.Now : dto.RegisteredAt,
            IsLoyaltyMember = dto.IsLoyaltyMember,
            LoyaltyPoints = dto.LoyaltyPoints
        };

        _dbContext.Customers.Add(customer);
        _dbContext.SaveChanges();

        return CreatedAtAction(nameof(Get), new { id = customer.Id }, ToDTO(customer));
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Manager")]
    public ActionResult<CustomerDTO> Put(int id, [FromBody] CustomerWriteDTO dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var customer = _dbContext.Customers.FirstOrDefault(customer => customer.Id == id && customer.DeletedAt == null);

        if (customer is null)
        {
            return NotFound();
        }

        var validationError = ValidateCustomerWriteDto(dto, id);
        if (validationError is not null)
        {
            return BadRequest(validationError);
        }

        customer.FirstName = dto.FirstName.Trim();
        customer.LastName = dto.LastName.Trim();
        customer.City = dto.City.Trim();
        customer.Street = dto.Street.Trim();
        customer.HouseNumber = dto.HouseNumber.Trim();
        customer.PostalCode = dto.PostalCode.Trim();
        customer.Email = dto.Email.Trim();
        customer.Phone = dto.Phone.Trim();
        customer.RegisteredAt = dto.RegisteredAt;
        customer.IsLoyaltyMember = dto.IsLoyaltyMember;
        customer.LoyaltyPoints = dto.LoyaltyPoints;

        _dbContext.SaveChanges();

        return Ok(ToDTO(customer));
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public ActionResult Delete(int id)
    {
        var customer = _dbContext.Customers.FirstOrDefault(customer => customer.Id == id && customer.DeletedAt == null);

        if (customer is null)
        {
            return NotFound();
        }

        SoftDeleteCustomer(customer);
        _dbContext.SaveChanges();

        return NoContent();
    }

    private IQueryable<Customer> ActiveCustomersQuery()
    {
        return _dbContext.Customers
            .Include(customer => customer.Tickets.Where(ticket => ticket.DeletedAt == null))
            .Where(customer => customer.DeletedAt == null);
    }

    private object? ValidateCustomerWriteDto(CustomerWriteDTO dto, int? currentCustomerId = null)
    {
        dto.Email = (dto.Email ?? string.Empty).Trim();

        var normalizedEmail = dto.Email.ToLower();
        var emailExists = _dbContext.Customers.Any(customer =>
            customer.DeletedAt == null
            && customer.Email.ToLower() == normalizedEmail
            && (!currentCustomerId.HasValue || customer.Id != currentCustomerId.Value));

        if (emailExists)
        {
            return new { error = "Kupac s tom email adresom već postoji." };
        }

        if (dto.LoyaltyPoints < 0)
        {
            return new { error = "Broj loyalty bodova ne može biti negativan." };
        }

        return null;
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

    private static CustomerDTO ToDTO(Customer customer)
    {
        return new CustomerDTO
        {
            Id = customer.Id,
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            City = customer.City,
            TicketCount = customer.Tickets.Count(ticket => ticket.DeletedAt == null)
        };
    }
}
