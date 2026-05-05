using Microsoft.EntityFrameworkCore;
using Vjezba.Model.Entities;

namespace Vjezba.DAL.Repositories;

public class ScreeningRepository
{
    private readonly CinemaDbContext _context;

    public ScreeningRepository(CinemaDbContext context)
    {
        _context = context;
    }

    public List<Screening> GetAll()
    {
        return _context.Screenings
            .Include(s => s.Movie)
            .Include(s => s.Hall)
            .ThenInclude(h => h.Cinema)
            .AsNoTracking()
            .ToList();
    }

    public Screening? GetById(int id)
    {
        return _context.Screenings
            .Include(s => s.Movie)
            .Include(s => s.Hall)
            .ThenInclude(h => h.Cinema)
            .AsNoTracking()
            .FirstOrDefault(s => s.Id == id);
    }
}
