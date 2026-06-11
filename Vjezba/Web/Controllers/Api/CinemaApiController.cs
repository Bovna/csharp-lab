using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vjezba.DAL;
using Vjezba.Model.Entities;
using Vjezba.Web.DTOs;

namespace Vjezba.Web.Controllers.Api;

[ApiController]
[Authorize]
[Route("api/kina")]
public class CinemaApiController : ControllerBase
{
    private readonly CinemaDbContext _dbContext;

    public CinemaApiController(CinemaDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
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
    public ActionResult<CinemaDTO> Post([FromBody] CinemaWriteDTO dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var cinema = new Cinema
        {
            Name = dto.Name,
            City = dto.City,
            Street = dto.Street,
            HouseNumber = dto.HouseNumber,
            PostalCode = dto.PostalCode,
            Email = dto.Email,
            Phone = dto.Phone
        };

        _dbContext.Cinemas.Add(cinema);
        _dbContext.SaveChanges();

        return CreatedAtAction(nameof(Get), new { id = cinema.Id }, ToDTO(cinema));
    }

    [HttpPut("{id}")]
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

        cinema.Name = dto.Name;
        cinema.City = dto.City;
        cinema.Street = dto.Street;
        cinema.HouseNumber = dto.HouseNumber;
        cinema.PostalCode = dto.PostalCode;
        cinema.Email = dto.Email;
        cinema.Phone = dto.Phone;

        _dbContext.SaveChanges();

        return Ok(ToDTO(cinema));
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        var cinema = _dbContext.Cinemas.FirstOrDefault(cinema => cinema.Id == id && cinema.DeletedAt == null);

        if (cinema is null)
        {
            return NotFound();
        }

        SoftDeleteCinema(cinema);
        _dbContext.SaveChanges();

        return NoContent();
    }

    private IQueryable<Cinema> ActiveCinemasQuery()
    {
        return _dbContext.Cinemas.Where(cinema => cinema.DeletedAt == null);
    }

    private void SoftDeleteCinema(Cinema cinema)
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
