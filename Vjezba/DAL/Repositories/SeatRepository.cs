using Microsoft.EntityFrameworkCore;
using Vjezba.Model.Entities;

namespace Vjezba.DAL.Repositories;

public class SeatRepository
{
    private readonly CinemaDbContext _context;

    public SeatRepository(CinemaDbContext context)
    {
        _context = context;
    }

    public List<Seat> GetAll()
    {
        return _context.Seats
            .Include(s => s.Hall)
            .ThenInclude(h => h.Cinema)
            .AsNoTracking()
            .ToList();
    }

    public Seat? GetById(int id)
    {
        return _context.Seats
            .Include(s => s.Hall)
            .ThenInclude(h => h.Cinema)
            .AsNoTracking()
            .FirstOrDefault(s => s.Id == id);
    }
}
