using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vjezba.DAL;
using Vjezba.Model.Entities;
using Vjezba.Web.DTOs;

namespace Vjezba.Web.Controllers.Api;

[ApiController]
[Authorize]
[Route("api/dvorane")]
public class HallApiController : ControllerBase
{
    private readonly CinemaDbContext _dbContext;
    private readonly ILogger<HallApiController> _logger;

    public HallApiController(CinemaDbContext dbContext, ILogger<HallApiController> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    [HttpGet]
    [AllowAnonymous]
    public ActionResult<IEnumerable<HallDTO>> Get(bool? supports3D)
    {
        var hallsQuery = ActiveHallsQuery();

        if (supports3D.HasValue)
        {
            hallsQuery = hallsQuery.Where(hall => hall.Supports3D == supports3D.Value);
        }

        var halls = hallsQuery
            .OrderBy(hall => hall.Id)
            .ToList()
            .Select(ToDTO)
            .ToList();

        return Ok(halls);
    }

    [HttpGet("{id}")]
    public ActionResult<HallDTO> Get(int id)
    {
        var hall = ActiveHallsQuery().FirstOrDefault(hall => hall.Id == id);

        if (hall is null)
        {
            return NotFound();
        }

        return Ok(ToDTO(hall));
    }

    [HttpGet("pretraga/{query}")]
    [AllowAnonymous]
    public ActionResult<IEnumerable<HallDTO>> Search(string query, bool? supports3D)
    {
        var normalizedQuery = query.Trim();

        var hallsQuery = ActiveHallsQuery();

        if (supports3D.HasValue)
        {
            hallsQuery = hallsQuery.Where(hall => hall.Supports3D == supports3D.Value);
        }

        if (!string.IsNullOrWhiteSpace(normalizedQuery))
        {
            hallsQuery = hallsQuery.Where(hall =>
                hall.Name.Contains(normalizedQuery) ||
                hall.Cinema.Name.Contains(normalizedQuery));
        }

        var halls = hallsQuery
            .OrderBy(hall => hall.Id)
            .ToList()
            .Select(ToDTO)
            .ToList();

        return Ok(halls);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Manager")]
    public ActionResult<HallDTO> Post([FromBody] HallWriteDTO dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var validationError = ValidateHallWriteDto(dto);
        if (validationError is not null)
        {
            return BadRequest(validationError);
        }

        var hall = new Hall
        {
            Name = dto.Name.Trim(),
            Capacity = dto.Capacity,
            Supports3D = dto.Supports3D,
            CinemaId = dto.CinemaId
        };

        _dbContext.Halls.Add(hall);
        _dbContext.SaveChanges();

        _logger.LogInformation(
            "Hall created by API. HallId={HallId}, CinemaId={CinemaId}, Capacity={Capacity}, Supports3D={Supports3D}, UserId={UserId}",
            hall.Id,
            hall.CinemaId,
            hall.Capacity,
            hall.Supports3D,
            GetCurrentUserId());

        var createdHall = ActiveHallsQuery().FirstOrDefault(existing => existing.Id == hall.Id);

        return CreatedAtAction(nameof(Get), new { id = hall.Id }, ToDTO(createdHall ?? hall));
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Manager")]
    public ActionResult<HallDTO> Put(int id, [FromBody] HallWriteDTO dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var hall = _dbContext.Halls.FirstOrDefault(hall => hall.Id == id && hall.DeletedAt == null);

        if (hall is null)
        {
            return NotFound();
        }

        var validationError = ValidateHallWriteDto(dto, id);
        if (validationError is not null)
        {
            return BadRequest(validationError);
        }

        hall.Name = dto.Name.Trim();
        hall.Capacity = dto.Capacity;
        hall.Supports3D = dto.Supports3D;
        hall.CinemaId = dto.CinemaId;

        _dbContext.SaveChanges();

        _logger.LogInformation(
            "Hall updated by API. HallId={HallId}, CinemaId={CinemaId}, Capacity={Capacity}, Supports3D={Supports3D}, UserId={UserId}",
            hall.Id,
            hall.CinemaId,
            hall.Capacity,
            hall.Supports3D,
            GetCurrentUserId());

        var updatedHall = ActiveHallsQuery().FirstOrDefault(existing => existing.Id == id);

        return Ok(ToDTO(updatedHall ?? hall));
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public ActionResult Delete(int id)
    {
        var hall = _dbContext.Halls.FirstOrDefault(hall => hall.Id == id && hall.DeletedAt == null);

        if (hall is null)
        {
            return NotFound();
        }

        var deleteSummary = SoftDeleteHall(hall);
        _dbContext.SaveChanges();

        _logger.LogInformation(
            "Hall soft deleted by API. HallId={HallId}, CinemaId={CinemaId}, DeletedSeatCount={DeletedSeatCount}, DeletedScreeningCount={DeletedScreeningCount}, DeletedTicketCount={DeletedTicketCount}, UserId={UserId}",
            hall.Id,
            hall.CinemaId,
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

    private IQueryable<Hall> ActiveHallsQuery()
    {
        return _dbContext.Halls
            .Include(hall => hall.Cinema)
            .Where(hall => hall.DeletedAt == null && hall.Cinema.DeletedAt == null);
    }

    private object? ValidateHallWriteDto(HallWriteDTO dto, int? currentHallId = null)
    {
        dto.Name = (dto.Name ?? string.Empty).Trim();

        var cinemaExists = _dbContext.Cinemas.Any(cinema => cinema.Id == dto.CinemaId && cinema.DeletedAt == null);

        if (!cinemaExists)
        {
            return new { error = "Odabrano kino ne postoji." };
        }

        var normalizedName = dto.Name.ToLower();
        var hallExists = _dbContext.Halls.Any(hall =>
            hall.DeletedAt == null
            && hall.CinemaId == dto.CinemaId
            && hall.Name.ToLower() == normalizedName
            && (!currentHallId.HasValue || hall.Id != currentHallId.Value));

        if (hallExists)
        {
            return new { error = "Dvorana s tim nazivom već postoji u odabranom kinu." };
        }

        return null;
    }

    private (int DeletedSeatCount, int DeletedScreeningCount, int DeletedTicketCount) SoftDeleteHall(Hall hall)
    {
        var deletedAt = DateTime.UtcNow;
        hall.DeletedAt = deletedAt;

        var seats = _dbContext.Seats
            .Where(seat => seat.HallId == hall.Id && seat.DeletedAt == null)
            .ToList();

        foreach (var seat in seats)
        {
            seat.DeletedAt = deletedAt;
        }

        var screenings = _dbContext.Screenings
            .Where(screening => screening.HallId == hall.Id && screening.DeletedAt == null)
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

        return (seats.Count, screenings.Count, tickets.Count);
    }

    private static HallDTO ToDTO(Hall hall)
    {
        return new HallDTO
        {
            Id = hall.Id,
            Name = hall.Name,
            Capacity = hall.Capacity,
            Supports3D = hall.Supports3D,
            CinemaId = hall.CinemaId,
            CinemaName = hall.Cinema == null ? string.Empty : hall.Cinema.Name
        };
    }
}
