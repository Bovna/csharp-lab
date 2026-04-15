using Microsoft.AspNetCore.Mvc;
using Vjezba.Model.Data;
using Vjezba.Model.Models.ViewModels;

namespace Vjezba.Model.Controllers;

public class TicketBuilderController : Controller
{
    private readonly CinemaMockRepository _cinemaRepository;
    private readonly MovieMockRepository _movieRepository;
    private readonly ScreeningMockRepository _screeningRepository;
    private readonly SeatMockRepository _seatRepository;
    private readonly TicketMockRepository _ticketRepository;

    public TicketBuilderController(
        CinemaMockRepository cinemaRepository,
        MovieMockRepository movieRepository,
        ScreeningMockRepository screeningRepository,
        SeatMockRepository seatRepository,
        TicketMockRepository ticketRepository)
    {
        _cinemaRepository = cinemaRepository;
        _movieRepository = movieRepository;
        _screeningRepository = screeningRepository;
        _seatRepository = seatRepository;
        _ticketRepository = ticketRepository;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var model = new TicketBuilderCinemaListViewModel
        {
            Cinemas = _cinemaRepository.GetAll().OrderBy(c => c.Name).ToList()
        };

        return View(model);
    }

    [HttpGet]
    public IActionResult Movies(int cinemaId)
    {
        var cinema = _cinemaRepository.GetById(cinemaId);
        if (cinema is null)
        {
            return RedirectToAction(nameof(Index));
        }

        var screeningsInCinema = _screeningRepository.GetAll()
            .Where(s => s.Hall?.Cinema?.Id == cinemaId && s.Movie is not null)
            .ToList();

        var movieIds = screeningsInCinema
            .Select(s => s.Movie!.Id)
            .Distinct()
            .ToHashSet();

        var movies = _movieRepository.GetAll()
            .Where(m => movieIds.Contains(m.Id))
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
        var cinema = _cinemaRepository.GetById(cinemaId);
        var movie = _movieRepository.GetById(movieId);
        if (cinema is null || movie is null)
        {
            return RedirectToAction(nameof(Index));
        }

        var screenings = _screeningRepository.GetAll()
            .Where(s => s.Hall?.Cinema?.Id == cinemaId && s.Movie?.Id == movieId)
            .OrderBy(s => s.StartTime)
            .Select(s => new TicketBuilderScreeningCardViewModel
            {
                Id = s.Id,
                HallId = s.Hall?.Id ?? 0,
                HallName = s.Hall?.Name ?? "Nepoznata dvorana",
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
        var cinema = _cinemaRepository.GetById(cinemaId);
        var movie = _movieRepository.GetById(movieId);
        var screening = _screeningRepository.GetById(screeningId);

        if (cinema is null || movie is null || screening is null)
        {
            return RedirectToAction(nameof(Index));
        }

        var hallId = screening.Hall?.Id;
        if (!hallId.HasValue)
        {
            return RedirectToAction(nameof(Screenings), new { cinemaId, movieId });
        }

        var seats = _seatRepository.GetAll()
            .Where(s => s.Hall?.Id == hallId.Value)
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

        var takenSeatIds = _ticketRepository.GetAll()
            .Where(t => t.Screening?.Id == screeningId
                && t.Seat is not null
                && (t.Status == Models.Entities.TicketStatus.Active || t.Status == Models.Entities.TicketStatus.Used))
            .Select(t => t.Seat!.Id)
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
            HallName = screening.Hall?.Name ?? "Nepoznata dvorana",
            StartTime = screening.StartTime,
            EndTime = screening.EndTime,
            Is3D = screening.Is3D,
            Seats = seats,
            SeatStatuses = seatStatuses
        };

        return View(model);
    }
}
