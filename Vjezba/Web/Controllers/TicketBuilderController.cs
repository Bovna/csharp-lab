using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vjezba.DAL;
using Vjezba.Model.Entities;
using Vjezba.Web.ViewModels;

namespace Vjezba.Web.Controllers;

public class TicketBuilderController : Controller
{
    private readonly CinemaDbContext _dbContext;

    public TicketBuilderController(CinemaDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var model = new TicketBuilderCinemaListViewModel
        {
            Cinemas = _dbContext.Cinemas.OrderBy(c => c.Name).ToList()
        };

        return View(model);
    }

    [HttpGet]
    public IActionResult Movies(int cinemaId)
    {
        var cinema = _dbContext.Cinemas.FirstOrDefault(c => c.Id == cinemaId);
        if (cinema is null)
        {
            return RedirectToAction(nameof(Index));
        }

        var movies = _dbContext.Movies
            .Where(m => m.Screenings.Any(s => s.Hall.CinemaId == cinemaId))
            .OrderBy(m => m.Title)
            .Select(m => new TicketBuilderMovieCardViewModel
            {
                Id = m.Id,
                Title = m.Title,
                Genre = m.Genre.ToString(),
                DurationMinutes = m.DurationMinutes,
                Language = m.Language,
                AgeRating = m.AgeRating
            })
            .ToList();

        var model = new TicketBuilderMovieListViewModel
        {
            CinemaId = cinema.Id,
            CinemaName = cinema.Name,
            Movies = movies
        };

        return View(model);
    }

    [HttpGet]
    public IActionResult Screenings(int cinemaId, int movieId)
    {
        var cinema = _dbContext.Cinemas.FirstOrDefault(c => c.Id == cinemaId);
        var movie = _dbContext.Movies.FirstOrDefault(m => m.Id == movieId);
        if (cinema is null || movie is null)
        {
            return RedirectToAction(nameof(Index));
        }

        var screenings = _dbContext.Screenings
            .Where(s => s.Hall.CinemaId == cinemaId && s.MovieId == movieId)
            .Include(s => s.Hall)
            .OrderBy(s => s.StartTime)
            .Select(s => new TicketBuilderScreeningCardViewModel
            {
                Id = s.Id,
                HallId = s.HallId,
                HallName = s.Hall.Name,
                StartTime = s.StartTime,
                EndTime = s.EndTime,
                Is3D = s.Is3D
            })
            .ToList();

        var model = new TicketBuilderScreeningListViewModel
        {
            CinemaId = cinema.Id,
            CinemaName = cinema.Name,
            MovieId = movie.Id,
            MovieTitle = movie.Title,
            Screenings = screenings
        };

        return View(model);
    }

    [HttpGet]
    public IActionResult Seats(int cinemaId, int movieId, int screeningId)
    {
        var cinema = _dbContext.Cinemas.FirstOrDefault(c => c.Id == cinemaId);
        var movie = _dbContext.Movies.FirstOrDefault(m => m.Id == movieId);
        var screening = _dbContext.Screenings
            .Include(s => s.Hall)
            .FirstOrDefault(s => s.Id == screeningId);

        if (cinema is null || movie is null || screening is null)
        {
            return RedirectToAction(nameof(Index));
        }

        var seats = _dbContext.Seats
            .Where(s => s.HallId == screening.HallId)
            .OrderBy(s => s.RowLabel)
            .ThenBy(s => s.SeatNumber)
            .Select(s => new TicketBuilderSeatViewModel
            {
                Id = s.Id,
                RowLabel = s.RowLabel,
                SeatNumber = s.SeatNumber,
                SeatType = s.SeatType.ToString()
            })
            .ToList();

        var takenSeatIds = _dbContext.Tickets
            .Where(t => t.ScreeningId == screeningId
                && t.SeatId.HasValue
                && (t.Status == TicketStatus.Active || t.Status == TicketStatus.Used))
            .Select(t => t.SeatId!.Value)
            .Distinct()
            .ToHashSet();

        var seatStatuses = seats.ToDictionary(
            s => s.Id,
            s => takenSeatIds.Contains(s.Id) ? "taken" : "available");

        var model = new TicketBuilderSeatPageViewModel
        {
            CinemaId = cinema.Id,
            CinemaName = cinema.Name,
            MovieId = movie.Id,
            MovieTitle = movie.Title,
            ScreeningId = screening.Id,
            HallName = screening.Hall.Name,
            StartTime = screening.StartTime,
            EndTime = screening.EndTime,
            Is3D = screening.Is3D,
            Seats = seats,
            SeatStatuses = seatStatuses
        };

        return View(model);
    }
}
