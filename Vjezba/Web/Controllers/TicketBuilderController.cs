using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vjezba.DAL;
using Vjezba.Model.Entities;
using Vjezba.Web.ViewModels;

namespace Vjezba.Web.Controllers;

[Route("[controller]")]
[Route("brza kupovina")]
public class TicketBuilderController : Controller
{
    private readonly CinemaDbContext _dbContext;
    private const decimal StandardSeatPrice = 7.50m;
    private const decimal VipSeatPrice = 11.00m;
    private const decimal CoupleSeatPrice = 13.50m;
    private const decimal ThreeDSurcharge = 2.00m;

    public TicketBuilderController(CinemaDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [Route("")]
    public IActionResult Index()
    {
        var model = new TicketBuilderCinemaListViewModel
        {
            Cinemas = _dbContext.Cinemas
                .Where(c => c.DeletedAt == null)
                .OrderBy(c => c.Name)
                .ToList()
        };

        return View(model);
    }

    [Route("movies/{cinemaId}")]
    public IActionResult Movies(int cinemaId)
    {
        var cinema = _dbContext.Cinemas.FirstOrDefault(c => c.Id == cinemaId && c.DeletedAt == null);
        if (cinema is null)
        {
            return RedirectToAction(nameof(Index));
        }

        var movies = _dbContext.Movies
            .Where(m => m.DeletedAt == null
                && m.Screenings.Any(s => s.DeletedAt == null
                    && s.Hall.CinemaId == cinemaId
                    && s.Hall.DeletedAt == null
                    && s.Hall.Cinema.DeletedAt == null))
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

    [Route("screenings/{cinemaId}/{movieId}")]
    public IActionResult Screenings(int cinemaId, int movieId)
    {
        var cinema = _dbContext.Cinemas.FirstOrDefault(c => c.Id == cinemaId && c.DeletedAt == null);
        var movie = _dbContext.Movies.FirstOrDefault(m => m.Id == movieId && m.DeletedAt == null);
        if (cinema is null || movie is null)
        {
            return RedirectToAction(nameof(Index));
        }

        var screenings = _dbContext.Screenings
            .Where(s => s.Hall.CinemaId == cinemaId
                && s.MovieId == movieId
                && s.DeletedAt == null
                && s.Hall.DeletedAt == null
                && s.Hall.Cinema.DeletedAt == null)
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

    [Route("seats/{cinemaId}/{movieId}/{screeningId}")]
    public IActionResult Seats(int cinemaId, int movieId, int screeningId)
    {
        var cinema = _dbContext.Cinemas.FirstOrDefault(c => c.Id == cinemaId && c.DeletedAt == null);
        var movie = _dbContext.Movies.FirstOrDefault(m => m.Id == movieId && m.DeletedAt == null);
        var screening = _dbContext.Screenings
            .Include(s => s.Hall)
            .FirstOrDefault(s => s.Id == screeningId
                && s.DeletedAt == null
                && s.Hall.DeletedAt == null
                && s.Hall.Cinema.DeletedAt == null);

        if (cinema is null || movie is null || screening is null)
        {
            return RedirectToAction(nameof(Index));
        }

        var seats = _dbContext.Seats
            .Where(s => s.HallId == screening.HallId && s.DeletedAt == null)
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
                && t.DeletedAt == null
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

    [HttpGet("Checkout")]
    [Route("checkout/{cinemaId}/{movieId}/{screeningId}/{seatId}")]
    public IActionResult Checkout(int cinemaId, int movieId, int screeningId, int seatId)
    {
        if (!TryGetPurchaseContext(cinemaId, movieId, screeningId, seatId, out var cinema, out var movie, out var screening, out var seat))
        {
            return RedirectToAction(nameof(Index));
        }

        if (IsSeatTaken(screeningId, seatId))
        {
            TempData["PurchaseError"] = "Odabrano sjedalo je u meduvremenu zauzeto. Odaberite drugo sjedalo.";
            return RedirectToAction(nameof(Seats), new { cinemaId, movieId, screeningId });
        }

        var model = BuildCheckoutViewModel(cinema!, movie!, screening!, seat!);
        return View(model);
    }

    [HttpPost]
    public IActionResult Purchase(TicketBuilderCheckoutInputModel input)
    {
        if (!TryGetPurchaseContext(input.CinemaId, input.MovieId, input.ScreeningId, input.SeatId, out var cinema, out var movie, out var screening, out var seat))
        {
            return RedirectToAction(nameof(Index));
        }

        var customer = _dbContext.Customers.FirstOrDefault(c => c.Id == input.CustomerId && c.DeletedAt == null);
        if (customer is null)
        {
            ModelState.AddModelError(nameof(TicketBuilderCheckoutInputModel.CustomerId), "Odaberite valjanog kupca.");
            return View("Checkout", BuildCheckoutViewModel(cinema!, movie!, screening!, seat!));
        }

        if (IsSeatTaken(input.ScreeningId, input.SeatId))
        {
            ModelState.AddModelError(string.Empty, "Odabrano sjedalo je vec zauzeto. Odaberite drugo sjedalo.");
            return View("Checkout", BuildCheckoutViewModel(cinema!, movie!, screening!, seat!));
        }

        var ticket = new Ticket
        {
            TicketNumber = GenerateTicketNumber(cinema!),
            PurchasedAt = DateTime.Now,
            Price = CalculateSeatPrice(seat!.SeatType, screening!.Is3D),
            Status = TicketStatus.Active,
            ScreeningId = screening.Id,
            SeatId = seat.Id,
            CustomerId = customer.Id
        };

        _dbContext.Tickets.Add(ticket);
        EnsureTicketsIdentity();
        _dbContext.SaveChanges();

        return RedirectToAction("Details", "Ticket", new { id = ticket.Id });
    }

    private bool TryGetPurchaseContext(
        int cinemaId,
        int movieId,
        int screeningId,
        int seatId,
        out Cinema? cinema,
        out Movie? movie,
        out Screening? screening,
        out Seat? seat)
    {
        cinema = _dbContext.Cinemas.FirstOrDefault(c => c.Id == cinemaId && c.DeletedAt == null);
        movie = _dbContext.Movies.FirstOrDefault(m => m.Id == movieId && m.DeletedAt == null);
        screening = _dbContext.Screenings
            .Include(s => s.Hall)
            .FirstOrDefault(s => s.Id == screeningId
                && s.DeletedAt == null
                && s.Hall.DeletedAt == null
                && s.Hall.Cinema.DeletedAt == null);
        seat = _dbContext.Seats.FirstOrDefault(s => s.Id == seatId
            && s.DeletedAt == null
            && s.Hall.DeletedAt == null
            && s.Hall.Cinema.DeletedAt == null);

        if (cinema is null || movie is null || screening is null || seat is null)
        {
            return false;
        }

        var screeningMatchesFlow = screening.MovieId == movieId && screening.Hall.CinemaId == cinemaId;
        var seatMatchesScreening = seat.HallId == screening.HallId;

        return screeningMatchesFlow && seatMatchesScreening;
    }

    private bool IsSeatTaken(int screeningId, int seatId)
    {
        return _dbContext.Tickets.Any(t =>
            t.ScreeningId == screeningId
            && t.SeatId == seatId
            && t.DeletedAt == null
            && (t.Status == TicketStatus.Active || t.Status == TicketStatus.Used));
    }

    private TicketBuilderCheckoutViewModel BuildCheckoutViewModel(
        Cinema cinema,
        Movie movie,
        Screening screening,
        Seat seat)
    {
        return new TicketBuilderCheckoutViewModel
        {
            CinemaId = cinema.Id,
            CinemaName = cinema.Name,
            MovieId = movie.Id,
            MovieTitle = movie.Title,
            ScreeningId = screening.Id,
            StartTime = screening.StartTime,
            HallName = screening.Hall.Name,
            Is3D = screening.Is3D,
            SeatId = seat.Id,
            SeatLabel = $"{seat.RowLabel}{seat.SeatNumber}",
            SeatType = seat.SeatType.ToString(),
            Price = CalculateSeatPrice(seat.SeatType, screening.Is3D),
            Customers = _dbContext.Customers
                .Where(c => c.DeletedAt == null)
                .OrderBy(c => c.LastName)
                .ThenBy(c => c.FirstName)
                .Select(c => new TicketBuilderCheckoutCustomerViewModel
                {
                    Id = c.Id,
                    FullName = $"{c.FirstName} {c.LastName}"
                })
                .ToList()
        };
    }

    private static decimal CalculateSeatPrice(SeatType seatType, bool is3D)
    {
        var basePrice = seatType switch
        {
            SeatType.Vip => VipSeatPrice,
            SeatType.Couple => CoupleSeatPrice,
            _ => StandardSeatPrice
        };

        if (is3D)
        {
            basePrice += ThreeDSurcharge;
        }

        return basePrice;
    }

    private string GenerateTicketNumber(Cinema cinema)
    {
        // Use city code (first two letters of city) and year, then a zero-padded sequence
        var city = (cinema?.City ?? string.Empty).ToUpperInvariant();
        var cleaned = new string(city.Where(char.IsLetter).ToArray());
        var code = cleaned.Length >= 2 ? cleaned.Substring(0, 2) : cleaned.PadRight(2, 'X');
        var year = DateTime.Now.Year;
        var prefix = $"{code}-{year}-";

        // Find existing max sequence for this prefix
        var seqs = _dbContext.Tickets
            .Where(t => t.TicketNumber != null && t.TicketNumber.StartsWith(prefix))
            .Select(t => t.TicketNumber.Substring(prefix.Length))
            .ToList()
            .Select(s =>
            {
                return int.TryParse(s, out var n) ? n : 0;
            });

        var max = seqs.Any() ? seqs.Max() : 0;
        var next = max + 1;
        var number = $"{prefix}{next:0000}";
        return number;
    }

    private void EnsureTicketsIdentity()
    {
        try
        {
            var maxId = _dbContext.Tickets.Select(t => (int?)t.Id).Max() ?? 0;
            // Reseed identity so next inserted id is maxId + 1
            // Works for SQL Server: DBCC CHECKIDENT
            _dbContext.Database.ExecuteSqlRaw($"DBCC CHECKIDENT ('Tickets', RESEED, {maxId})");
        }
        catch
        {
            // If DB does not support DBCC or this fails, skip reseed silently.
        }
    }
}
