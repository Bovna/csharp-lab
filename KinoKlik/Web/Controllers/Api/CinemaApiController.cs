using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using KinoKlik.DAL;
using KinoKlik.Model.Entities;
using KinoKlik.Web.DTOs;

namespace KinoKlik.Web.Controllers.Api;

[ApiController]
[Authorize]
[Route("api/kina")]
public class CinemaApiController : ControllerBase
{
    private readonly CinemaDbContext _dbContext;
    private readonly ILogger<CinemaApiController> _logger;

    public CinemaApiController(CinemaDbContext dbContext, ILogger<CinemaApiController> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    [HttpGet]
    [AllowAnonymous]
    public ActionResult<IEnumerable<CinemaDTO>> Get(string? city)
    {
        var normalizedCity = (city ?? string.Empty).Trim();

        var cinemasQuery = ActiveCinemasQuery();

        if (!string.IsNullOrWhiteSpace(normalizedCity))
        {
            cinemasQuery = cinemasQuery.Where(cinema => cinema.City == normalizedCity);
        }

        var cinemas = cinemasQuery
            .OrderBy(cinema => cinema.Id)
            .ToList()
            .Select(ToDTO)
            .ToList();

        return Ok(cinemas);
    }

    [HttpGet("{id}")]
    public ActionResult<CinemaDTO> Get(int id)
    {
        var cinema = ActiveCinemasQuery().FirstOrDefault(cinema => cinema.Id == id);

        if (cinema is null)
        {
            return NotFound();
        }

        return Ok(ToDTO(cinema));
    }

    [HttpGet("pretraga/{query}")]
    [AllowAnonymous]
    public ActionResult<IEnumerable<CinemaDTO>> Search(string query, string? city)
    {
        var normalizedQuery = query.Trim();
        var normalizedCity = (city ?? string.Empty).Trim();

        var cinemasQuery = ActiveCinemasQuery();

        if (!string.IsNullOrWhiteSpace(normalizedCity))
        {
            cinemasQuery = cinemasQuery.Where(cinema => cinema.City == normalizedCity);
        }

        if (!string.IsNullOrWhiteSpace(normalizedQuery))
        {
            cinemasQuery = cinemasQuery.Where(cinema =>
                cinema.Name.Contains(normalizedQuery) ||
                cinema.City.Contains(normalizedQuery) ||
                cinema.Street.Contains(normalizedQuery));
        }

        var cinemas = cinemasQuery
            .OrderBy(cinema => cinema.Id)
            .ToList()
            .Select(ToDTO)
            .ToList();

        return Ok(cinemas);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Manager")]
    public ActionResult<CinemaDTO> Post([FromBody] CinemaWriteDTO dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var validationError = ValidateCinemaWriteDto(dto);
        if (validationError is not null)
        {
            return BadRequest(validationError);
        }

        var cinema = new Cinema
        {
            Name = dto.Name.Trim(),
            City = dto.City.Trim(),
            Street = dto.Street.Trim(),
            HouseNumber = dto.HouseNumber.Trim(),
            PostalCode = dto.PostalCode.Trim(),
            Email = dto.Email.Trim(),
            Phone = dto.Phone.Trim()
        };

        _dbContext.Cinemas.Add(cinema);
        _dbContext.SaveChanges();

        _logger.LogInformation(
            "Cinema created by API. CinemaId={CinemaId}, City={City}, UserId={UserId}",
            cinema.Id,
            cinema.City,
            GetCurrentUserId());

        return CreatedAtAction(nameof(Get), new { id = cinema.Id }, ToDTO(cinema));
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Manager")]
    public ActionResult<CinemaDTO> Put(int id, [FromBody] CinemaWriteDTO dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var cinema = _dbContext.Cinemas.FirstOrDefault(cinema => cinema.Id == id && cinema.DeletedAt == null);

        if (cinema is null)
        {
            return NotFound();
        }

        var validationError = ValidateCinemaWriteDto(dto, id);
        if (validationError is not null)
        {
            return BadRequest(validationError);
        }

        cinema.Name = dto.Name.Trim();
        cinema.City = dto.City.Trim();
        cinema.Street = dto.Street.Trim();
        cinema.HouseNumber = dto.HouseNumber.Trim();
        cinema.PostalCode = dto.PostalCode.Trim();
        cinema.Email = dto.Email.Trim();
        cinema.Phone = dto.Phone.Trim();

        _dbContext.SaveChanges();

        _logger.LogInformation(
            "Cinema updated by API. CinemaId={CinemaId}, City={City}, UserId={UserId}",
            cinema.Id,
            cinema.City,
            GetCurrentUserId());

        return Ok(ToDTO(cinema));
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public ActionResult Delete(int id)
    {
        var cinema = _dbContext.Cinemas.FirstOrDefault(cinema => cinema.Id == id && cinema.DeletedAt == null);

        if (cinema is null)
        {
            return NotFound();
        }

        var deleteSummary = SoftDeleteCinema(cinema);
        _dbContext.SaveChanges();

        _logger.LogInformation(
            "Cinema soft deleted by API. CinemaId={CinemaId}, DeletedHallCount={DeletedHallCount}, DeletedSeatCount={DeletedSeatCount}, DeletedScreeningCount={DeletedScreeningCount}, DeletedTicketCount={DeletedTicketCount}, UserId={UserId}",
            cinema.Id,
            deleteSummary.DeletedHallCount,
            deleteSummary.DeletedSeatCount,
            deleteSummary.DeletedScreeningCount,
            deleteSummary.DeletedTicketCount,
            GetCurrentUserId());

        return NoContent();
    }

    private string GetCurrentUserId()
    {
        return User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "unknown";
    }

    private IQueryable<Cinema> ActiveCinemasQuery()
    {
        return _dbContext.Cinemas.Where(cinema => cinema.DeletedAt == null);
    }

    private object? ValidateCinemaWriteDto(CinemaWriteDTO dto, int? currentCinemaId = null)
    {
        dto.Name = (dto.Name ?? string.Empty).Trim();
        dto.City = (dto.City ?? string.Empty).Trim();
        dto.Email = (dto.Email ?? string.Empty).Trim();

        var normalizedEmail = dto.Email.ToLower();
        var emailExists = _dbContext.Cinemas.Any(cinema =>
            cinema.DeletedAt == null
            && cinema.Email.ToLower() == normalizedEmail
            && (!currentCinemaId.HasValue || cinema.Id != currentCinemaId.Value));

        if (emailExists)
        {
            return new { error = "Kino s tom email adresom već postoji." };
        }

        var normalizedName = dto.Name.ToLower();
        var normalizedCity = dto.City.ToLower();
        var cinemaExists = _dbContext.Cinemas.Any(cinema =>
            cinema.DeletedAt == null
            && cinema.Name.ToLower() == normalizedName
            && cinema.City.ToLower() == normalizedCity
            && (!currentCinemaId.HasValue || cinema.Id != currentCinemaId.Value));

        if (cinemaExists)
        {
            return new { error = "Kino s tim nazivom već postoji u odabranom gradu." };
        }

        return null;
    }

    private (int DeletedHallCount, int DeletedSeatCount, int DeletedScreeningCount, int DeletedTicketCount) SoftDeleteCinema(Cinema cinema)
    {
        var deletedAt = DateTime.UtcNow;
        cinema.DeletedAt = deletedAt;

        var halls = _dbContext.Halls
            .Where(hall => hall.CinemaId == cinema.Id && hall.DeletedAt == null)
            .ToList();

        foreach (var hall in halls)
        {
            hall.DeletedAt = deletedAt;
        }

        var hallIds = halls.Select(hall => hall.Id).ToList();

        var seats = _dbContext.Seats
            .Where(seat => hallIds.Contains(seat.HallId) && seat.DeletedAt == null)
            .ToList();

        foreach (var seat in seats)
        {
            seat.DeletedAt = deletedAt;
        }

        var screenings = _dbContext.Screenings
            .Where(screening => hallIds.Contains(screening.HallId) && screening.DeletedAt == null)
            .ToList();

        foreach (var screening in screenings)
        {
            screening.DeletedAt = deletedAt;
        }

        var screeningIds = screenings.Select(screening => screening.Id).ToList();
        var seatIds = seats.Select(seat => seat.Id).ToList();

        var tickets = _dbContext.Tickets
            .Where(ticket => ticket.DeletedAt == null
                && (screeningIds.Contains(ticket.ScreeningId)
                    || (ticket.SeatId.HasValue && seatIds.Contains(ticket.SeatId.Value))))
            .ToList();

        foreach (var ticket in tickets)
        {
            ticket.DeletedAt = deletedAt;
        }

        return (halls.Count, seats.Count, screenings.Count, tickets.Count);
    }

    private static CinemaDTO ToDTO(Cinema cinema)
    {
        return new CinemaDTO
        {
            Id = cinema.Id,
            Name = cinema.Name,
            City = cinema.City,
            Street = cinema.Street,
            HouseNumber = cinema.HouseNumber,
            PostalCode = cinema.PostalCode,
            Email = cinema.Email,
            Phone = cinema.Phone
        };
    }
}
