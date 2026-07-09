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
[Route("api/sjedala")]
public class SeatApiController : ControllerBase
{
    private readonly CinemaDbContext _dbContext;
    private readonly ILogger<SeatApiController> _logger;

    public SeatApiController(CinemaDbContext dbContext, ILogger<SeatApiController> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    [HttpGet]
    [AllowAnonymous]
    public ActionResult<IEnumerable<SeatDTO>> Get(SeatType? seatType)
    {
        var seatsQuery = ActiveSeatsQuery();

        if (seatType.HasValue)
        {
            seatsQuery = seatsQuery.Where(seat => seat.SeatType == seatType.Value);
        }

        var seats = seatsQuery
            .OrderBy(seat => seat.Id)
            .ToList()
            .Select(ToDTO)
            .ToList();

        return Ok(seats);
    }

    [HttpGet("{id}")]
    public ActionResult<SeatDTO> Get(int id)
    {
        var seat = ActiveSeatsQuery().FirstOrDefault(seat => seat.Id == id);

        if (seat is null)
        {
            return NotFound();
        }

        return Ok(ToDTO(seat));
    }

    [HttpGet("pretraga/{query}")]
    [AllowAnonymous]
    public ActionResult<IEnumerable<SeatDTO>> Search(string query, SeatType? seatType)
    {
        var normalizedQuery = query.Trim();

        var seatsQuery = ActiveSeatsQuery();

        if (seatType.HasValue)
        {
            seatsQuery = seatsQuery.Where(seat => seat.SeatType == seatType.Value);
        }

        if (!string.IsNullOrWhiteSpace(normalizedQuery))
        {
            if (int.TryParse(normalizedQuery, out var seatNumber))
            {
                seatsQuery = seatsQuery.Where(seat => seat.SeatNumber == seatNumber);
            }
            else
            {
                seatsQuery = seatsQuery.Where(seat =>
                    seat.RowLabel.Contains(normalizedQuery) ||
                    seat.Hall.Name.Contains(normalizedQuery) ||
                    seat.Hall.Cinema.Name.Contains(normalizedQuery));
            }
        }

        var seats = seatsQuery
            .OrderBy(seat => seat.Id)
            .ToList()
            .Select(ToDTO)
            .ToList();

        return Ok(seats);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Manager")]
    public ActionResult<SeatDTO> Post([FromBody] SeatWriteDTO dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var validationError = ValidateSeatWriteDto(dto);
        if (validationError is not null)
        {
            return BadRequest(validationError);
        }

        var seat = new Seat
        {
            RowLabel = dto.RowLabel.Trim().ToUpperInvariant(),
            SeatNumber = dto.SeatNumber,
            SeatType = dto.SeatType,
            HallId = dto.HallId
        };

        _dbContext.Seats.Add(seat);
        _dbContext.SaveChanges();

        _logger.LogInformation(
            "Seat created by API. SeatId={SeatId}, HallId={HallId}, RowLabel={RowLabel}, SeatNumber={SeatNumber}, SeatType={SeatType}, UserId={UserId}",
            seat.Id,
            seat.HallId,
            seat.RowLabel,
            seat.SeatNumber,
            seat.SeatType,
            GetCurrentUserId());

        var createdSeat = ActiveSeatsQuery().FirstOrDefault(existing => existing.Id == seat.Id);

        return CreatedAtAction(nameof(Get), new { id = seat.Id }, ToDTO(createdSeat ?? seat));
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Manager")]
    public ActionResult<SeatDTO> Put(int id, [FromBody] SeatWriteDTO dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var seat = _dbContext.Seats.FirstOrDefault(seat => seat.Id == id && seat.DeletedAt == null);

        if (seat is null)
        {
            return NotFound();
        }

        var validationError = ValidateSeatWriteDto(dto, id);
        if (validationError is not null)
        {
            return BadRequest(validationError);
        }

        seat.RowLabel = dto.RowLabel.Trim().ToUpperInvariant();
        seat.SeatNumber = dto.SeatNumber;
        seat.SeatType = dto.SeatType;
        seat.HallId = dto.HallId;

        _dbContext.SaveChanges();

        _logger.LogInformation(
            "Seat updated by API. SeatId={SeatId}, HallId={HallId}, RowLabel={RowLabel}, SeatNumber={SeatNumber}, SeatType={SeatType}, UserId={UserId}",
            seat.Id,
            seat.HallId,
            seat.RowLabel,
            seat.SeatNumber,
            seat.SeatType,
            GetCurrentUserId());

        var updatedSeat = ActiveSeatsQuery().FirstOrDefault(existing => existing.Id == id);

        return Ok(ToDTO(updatedSeat ?? seat));
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public ActionResult Delete(int id)
    {
        var seat = _dbContext.Seats.FirstOrDefault(seat => seat.Id == id && seat.DeletedAt == null);

        if (seat is null)
        {
            return NotFound();
        }

        var deletedTicketCount = SoftDeleteSeat(seat);
        _dbContext.SaveChanges();

        _logger.LogInformation(
            "Seat soft deleted by API. SeatId={SeatId}, HallId={HallId}, DeletedTicketCount={DeletedTicketCount}, UserId={UserId}",
            seat.Id,
            seat.HallId,
            deletedTicketCount,
            GetCurrentUserId());

        return NoContent();
    }

    private string GetCurrentUserId()
    {
        return User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "unknown";
    }

    private IQueryable<Seat> ActiveSeatsQuery()
    {
        return _dbContext.Seats
            .Include(seat => seat.Hall)
                .ThenInclude(hall => hall.Cinema)
            .Where(seat => seat.DeletedAt == null
                && seat.Hall.DeletedAt == null
                && seat.Hall.Cinema.DeletedAt == null);
    }

    private object? ValidateSeatWriteDto(SeatWriteDTO dto, int? currentSeatId = null)
    {
        dto.RowLabel = (dto.RowLabel ?? string.Empty).Trim().ToUpperInvariant();

        var hallExists = _dbContext.Halls.Any(hall => hall.Id == dto.HallId
            && hall.DeletedAt == null
            && hall.Cinema.DeletedAt == null);

        if (!hallExists)
        {
            return new { error = "Odabrana dvorana ne postoji." };
        }

        var seatExists = _dbContext.Seats.Any(seat =>
            seat.DeletedAt == null
            && seat.HallId == dto.HallId
            && seat.RowLabel.ToLower() == dto.RowLabel.ToLower()
            && seat.SeatNumber == dto.SeatNumber
            && (!currentSeatId.HasValue || seat.Id != currentSeatId.Value));

        if (seatExists)
        {
            return new { error = "Sjedalo s tom oznakom već postoji u dvorani." };
        }

        return null;
    }

    private int SoftDeleteSeat(Seat seat)
    {
        var deletedAt = DateTime.UtcNow;
        seat.DeletedAt = deletedAt;

        var tickets = _dbContext.Tickets
            .Where(ticket => ticket.SeatId == seat.Id && ticket.DeletedAt == null)
            .ToList();

        foreach (var ticket in tickets)
        {
            ticket.DeletedAt = deletedAt;
        }

        return tickets.Count;
    }

    private static SeatDTO ToDTO(Seat seat)
    {
        return new SeatDTO
        {
            Id = seat.Id,
            RowLabel = seat.RowLabel,
            SeatNumber = seat.SeatNumber,
            SeatType = seat.SeatType,
            Hall = ToHallDTO(seat.Hall)
        };
    }

    private static HallDTO ToHallDTO(Hall hall)
    {
        return hall == null ? new HallDTO() : new HallDTO
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
