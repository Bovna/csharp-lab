using Microsoft.EntityFrameworkCore;
using Vjezba.Model.Entities;

namespace Vjezba.DAL.Repositories;

public class TicketRepository
{
    private readonly CinemaDbContext _context;

    public TicketRepository(CinemaDbContext context)
    {
        _context = context;
    }

    public List<Ticket> GetAll()
    {
        return _context.Tickets
            .Include(t => t.Customer)
            .Include(t => t.Seat)
            .Include(t => t.Screening)
            .ThenInclude(s => s.Movie)
            .Include(t => t.Screening)
            .ThenInclude(s => s.Hall)
            .ThenInclude(h => h.Cinema)
            .AsNoTracking()
            .ToList();
    }

    public Ticket? GetById(int id)
    {
        return _context.Tickets
            .Include(t => t.Customer)
            .Include(t => t.Seat)
            .Include(t => t.Screening)
            .ThenInclude(s => s.Movie)
            .Include(t => t.Screening)
            .ThenInclude(s => s.Hall)
            .ThenInclude(h => h.Cinema)
            .AsNoTracking()
            .FirstOrDefault(t => t.Id == id);
    }
}
