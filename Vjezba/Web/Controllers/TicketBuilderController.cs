using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vjezba.DAL;
using Vjezba.Model.Entities;
using Vjezba.Web.ViewModels;

namespace Vjezba.Web.Controllers;

[AutoValidateAntiforgeryToken]
[Route("[controller]")]
[Route("brza kupovina")]
public class TicketBuilderController : Controller
{
    private readonly CinemaDbContext _dbContext;
    private readonly ILogger<TicketBuilderController> _logger;
    private const decimal StandardSeatPrice = 7.50m;
    private const decimal VipSeatPrice = 11.00m;
    private const decimal CoupleSeatPrice = 13.50m;
    private const decimal ThreeDSurcharge = 2.00m;

    public TicketBuilderController(CinemaDbContext dbContext, ILogger<TicketBuilderController> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
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
            _logger.LogWarning(
                "Checkout blocked because seat is already taken. ScreeningId={ScreeningId}, SeatId={SeatId}, CinemaId={CinemaId}, MovieId={MovieId}",
                screeningId,
                seatId,
                cinemaId,
                movieId);

            TempData["PurchaseError"] = "Odabrano sjedalo je u meduvremenu zauzeto. Odaberite drugo sjedalo.";
            return RedirectToAction(nameof(Seats), new { cinemaId, movieId, screeningId });
        }

        var model = BuildCheckoutViewModel(cinema!, movie!, screening!, seat!);
        return View(model);
    }

    [HttpPost("Purchase")]
    [ValidateAntiForgeryToken]
    public IActionResult Purchase([Bind(Prefix = "Input")] TicketBuilderCheckoutInputModel input)
    {
        if (!TryGetPurchaseContext(input.CinemaId, input.MovieId, input.ScreeningId, input.SeatId, out var cinema, out var movie, out var screening, out var seat))
        {
            _logger.LogWarning(
                "Ticket purchase context was invalid. CinemaId={CinemaId}, MovieId={MovieId}, ScreeningId={ScreeningId}, SeatId={SeatId}",
                input.CinemaId,
                input.MovieId,
                input.ScreeningId,
                input.SeatId);

            return RedirectToAction(nameof(Index));
        }

        NormalizeCheckoutInput(input);
        ApplyKnownUserEmail(input);
        ValidateCheckoutInput(input);

        if (!ModelState.IsValid)
        {
            return View("Checkout", BuildCheckoutViewModel(cinema!, movie!, screening!, seat!, input));
        }

        if (IsSeatTaken(input.ScreeningId, input.SeatId))
        {
            _logger.LogWarning(
                "Ticket purchase blocked because seat is already taken. ScreeningId={ScreeningId}, SeatId={SeatId}, CinemaId={CinemaId}, MovieId={MovieId}",
                input.ScreeningId,
                input.SeatId,
                input.CinemaId,
                input.MovieId);

            ModelState.AddModelError(string.Empty, "Odabrano sjedalo je vec zauzeto. Odaberite drugo sjedalo.");
            return View("Checkout", BuildCheckoutViewModel(cinema!, movie!, screening!, seat!, input));
        }

        var customer = new Customer
        {
            FirstName = input.FirstName,
            LastName = input.LastName,
            City = input.City,
            Street = input.Street,
            HouseNumber = input.HouseNumber,
            PostalCode = input.PostalCode,
            Email = input.Email,
            Phone = input.Phone,
            RegisteredAt = DateTime.Now,
            IsLoyaltyMember = input.IsLoyaltyMember,
            LoyaltyPoints = 0
        };

        var ticket = new Ticket
        {
            TicketNumber = GenerateTicketNumber(cinema!),
            ConfirmationCode = Guid.NewGuid(),
            PurchasedAt = DateTime.Now,
            Price = CalculateSeatPrice(seat!.SeatType, screening!.Is3D),
            Status = TicketStatus.Active,
            ScreeningId = screening.Id,
            SeatId = seat.Id,
            Customer = customer
        };

        _dbContext.Customers.Add(customer);
        _dbContext.Tickets.Add(ticket);
        EnsureCustomersIdentity();
        EnsureTicketsIdentity();
        _dbContext.SaveChanges();

        _logger.LogInformation(
            "Ticket purchased. TicketId={TicketId}, TicketNumber={TicketNumber}, ScreeningId={ScreeningId}, SeatId={SeatId}, CinemaId={CinemaId}, MovieId={MovieId}, Price={Price}",
            ticket.Id,
            ticket.TicketNumber,
            ticket.ScreeningId,
            ticket.SeatId,
            input.CinemaId,
            input.MovieId,
            ticket.Price);

        return RedirectToAction(nameof(Success), new { confirmationCode = ticket.ConfirmationCode });
    }

    [HttpGet("success/{confirmationCode:guid}")]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public IActionResult Success(Guid confirmationCode)
    {
        var ticket = _dbContext.Tickets
            .Include(t => t.Customer)
            .Include(t => t.Seat)
            .Include(t => t.Screening)
                .ThenInclude(s => s.Movie)
            .Include(t => t.Screening)
                .ThenInclude(s => s.Hall)
                    .ThenInclude(h => h.Cinema)
            .FirstOrDefault(t => t.ConfirmationCode == confirmationCode
                && t.DeletedAt == null
                && t.Customer.DeletedAt == null
                && t.Screening.DeletedAt == null
                && t.Screening.Movie.DeletedAt == null
                && t.Screening.Hall.DeletedAt == null
                && t.Screening.Hall.Cinema.DeletedAt == null);

        if (ticket is null)
        {
            return RedirectToAction(nameof(Index));
        }

        var model = new TicketBuilderPurchaseSuccessViewModel
        {
            TicketId = ticket.Id,
            TicketNumber = ticket.TicketNumber,
            CustomerName = $"{ticket.Customer.FirstName} {ticket.Customer.LastName}",
            CustomerEmail = ticket.Customer.Email,
            CinemaName = ticket.Screening.Hall.Cinema.Name,
            MovieTitle = ticket.Screening.Movie.Title,
            HallName = ticket.Screening.Hall.Name,
            SeatLabel = ticket.Seat is null ? "-" : $"{ticket.Seat.RowLabel}{ticket.Seat.SeatNumber}",
            StartTime = ticket.Screening.StartTime,
            Price = ticket.Price
        };

        return View(model);
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
        Seat seat,
        TicketBuilderCheckoutInputModel? input = null)
    {
        var knownEmail = GetCurrentUserEmail();
        var isAuthenticated = User.Identity?.IsAuthenticated == true;
        var checkoutInput = input ?? new TicketBuilderCheckoutInputModel();

        checkoutInput.CinemaId = cinema.Id;
        checkoutInput.MovieId = movie.Id;
        checkoutInput.ScreeningId = screening.Id;
        checkoutInput.SeatId = seat.Id;

        if (isAuthenticated && !string.IsNullOrWhiteSpace(knownEmail))
        {
            checkoutInput.Email = knownEmail;
        }

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
            IsAuthenticated = isAuthenticated,
            KnownEmail = knownEmail,
            Input = checkoutInput
        };
    }

    private void ApplyKnownUserEmail(TicketBuilderCheckoutInputModel input)
    {
        var knownEmail = GetCurrentUserEmail();
        if (User.Identity?.IsAuthenticated == true && !string.IsNullOrWhiteSpace(knownEmail))
        {
            input.Email = knownEmail;
            ModelState.Remove(nameof(TicketBuilderCheckoutInputModel.Email));
            ModelState.Remove($"Input.{nameof(TicketBuilderCheckoutInputModel.Email)}");
        }
    }

    private string GetCurrentUserEmail()
    {
        var email = User.FindFirstValue(ClaimTypes.Email);

        if (string.IsNullOrWhiteSpace(email))
        {
            email = User.Identity?.Name;
        }

        return email?.Trim() ?? string.Empty;
    }

    private static void NormalizeCheckoutInput(TicketBuilderCheckoutInputModel input)
    {
        input.FirstName = Clean(input.FirstName);
        input.LastName = Clean(input.LastName);
        input.City = Clean(input.City);
        input.Street = Clean(input.Street);
        input.HouseNumber = Clean(input.HouseNumber);
        input.PostalCode = Clean(input.PostalCode);
        input.Email = Clean(input.Email);
        input.Phone = Clean(input.Phone);
    }

    private static string Clean(string? value)
    {
        return (value ?? string.Empty).Trim();
    }

    private void ValidateCheckoutInput(TicketBuilderCheckoutInputModel input)
    {
        AddRequiredErrorIfEmpty(
            nameof(TicketBuilderCheckoutInputModel.FirstName),
            input.FirstName,
            "Ime je obavezno.");
        AddRequiredErrorIfEmpty(
            nameof(TicketBuilderCheckoutInputModel.LastName),
            input.LastName,
            "Prezime je obavezno.");
        AddRequiredErrorIfEmpty(
            nameof(TicketBuilderCheckoutInputModel.City),
            input.City,
            "Grad je obavezan.");
        AddRequiredErrorIfEmpty(
            nameof(TicketBuilderCheckoutInputModel.Street),
            input.Street,
            "Ulica je obavezna.");
        AddRequiredErrorIfEmpty(
            nameof(TicketBuilderCheckoutInputModel.HouseNumber),
            input.HouseNumber,
            "Kucni broj je obavezan.");
        AddRequiredErrorIfEmpty(
            nameof(TicketBuilderCheckoutInputModel.PostalCode),
            input.PostalCode,
            "Postanski broj je obavezan.");
        AddRequiredErrorIfEmpty(
            nameof(TicketBuilderCheckoutInputModel.Email),
            input.Email,
            "E-posta je obavezna.");
        AddRequiredErrorIfEmpty(
            nameof(TicketBuilderCheckoutInputModel.Phone),
            input.Phone,
            "Telefon je obavezan.");
    }

    private void AddRequiredErrorIfEmpty(string propertyName, string value, string message)
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            return;
        }

        var prefixedKey = $"Input.{propertyName}";
        var hasExistingError =
            (ModelState.TryGetValue(prefixedKey, out var prefixedState) && prefixedState.Errors.Count > 0)
            || (ModelState.TryGetValue(propertyName, out var state) && state.Errors.Count > 0);

        if (!hasExistingError)
        {
            ModelState.AddModelError(prefixedKey, message);
        }
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
        var maxId = _dbContext.Tickets.Select(t => (int?)t.Id).Max() ?? 0;
        EnsureIdentity("Tickets", maxId);
    }

    private void EnsureCustomersIdentity()
    {
        var maxId = _dbContext.Customers.Select(c => (int?)c.Id).Max() ?? 0;
        EnsureIdentity("Customers", maxId);
    }

    private void EnsureIdentity(string tableName, int maxId)
    {
        try
        {
            var sql = tableName switch
            {
                "Customers" => "DBCC CHECKIDENT ('Customers', RESEED, {0})",
                "Tickets" => "DBCC CHECKIDENT ('Tickets', RESEED, {0})",
                _ => string.Empty
            };

            if (string.IsNullOrEmpty(sql))
            {
                return;
            }

            _dbContext.Database.ExecuteSqlRaw(sql, maxId);
        }
        catch
        {
            // If DB does not support DBCC or this fails, skip reseed silently.
        }
    }
}
