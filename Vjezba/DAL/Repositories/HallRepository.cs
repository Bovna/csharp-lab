using Microsoft.EntityFrameworkCore;
using Vjezba.Model.Entities;

namespace Vjezba.DAL.Repositories;

public class HallRepository
{
    private readonly CinemaDbContext _context;

    public HallRepository(CinemaDbContext context)
    {
        _context = context;
    }

    public List<Hall> GetAll()
    {
        return _context.Halls
            .Include(h => h.Cinema)
            .AsNoTracking()
            .ToList();
    }

    public Hall? GetById(int id)
    {
        return _context.Halls
            .Include(h => h.Cinema)
            .AsNoTracking()
            .FirstOrDefault(h => h.Id == id);
    }
}
