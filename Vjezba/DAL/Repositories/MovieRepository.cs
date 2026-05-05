using Vjezba.Model.Entities;

using Microsoft.EntityFrameworkCore;
namespace Vjezba.DAL.Repositories;

public class MovieRepository
{
    private readonly CinemaDbContext _context;

    public MovieRepository(CinemaDbContext context)
    {
        _context = context;
    }

    public List<Movie> GetAll()
    {
        return _context.Movies.AsNoTracking().ToList();
    }

    public Movie? GetById(int id)
    {
        return _context.Movies
            .AsNoTracking()
            .FirstOrDefault(m => m.Id == id);
    }
}