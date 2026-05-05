using Microsoft.EntityFrameworkCore;
using Vjezba.Model.Entities;

namespace Vjezba.DAL.Repositories;

public class CustomerRepository
{
    private readonly CinemaDbContext _context;

    public CustomerRepository(CinemaDbContext context)
    {
        _context = context;
    }

    public List<Customer> GetAll()
    {
        return _context.Customers
            .AsNoTracking()
            .ToList();
    }

    public Customer? GetById(int id)
    {
        return _context.Customers
            .AsNoTracking()
            .FirstOrDefault(c => c.Id == id);
    }
}
