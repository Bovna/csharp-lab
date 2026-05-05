using Microsoft.EntityFrameworkCore;
using Vjezba.Model.Entities;

namespace Vjezba.DAL.Repositories;

public class CinemaRepository
{
    private readonly CinemaDbContext _context;

    public CinemaRepository(CinemaDbContext context)
    {
        _context = context;
    }

    public List<Cinema> GetAll()
    {
        return _context.Cinemas
            .Include(c => c.Halls)
            .AsNoTracking()
            .ToList();
    }

    public Cinema? GetById(int id)
    {
        return _context.Cinemas
            .Include(c => c.Halls)
            .AsNoTracking()
            .FirstOrDefault(c => c.Id == id);
    }
}
