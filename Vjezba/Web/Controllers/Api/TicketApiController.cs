using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vjezba.DAL;
using Vjezba.Model.Entities;
using Vjezba.Web.DTOs;

namespace Vjezba.Web.Controllers.Api;

[ApiController]
[Authorize]
[Route("api/ulaznice")]
public class TicketApiController : ControllerBase
{
    private readonly CinemaDbContext _dbContext;

    public TicketApiController(CinemaDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public ActionResult<IEnumerable<TicketDTO>> Get(TicketStatus? status)
    {
        var ticketsQuery = ActiveTicketsQuery();

        if (status.HasValue)
        {
            ticketsQuery = ticketsQuery.Where(ticket => ticket.Status == status.Value);
        }

        var tickets = ticketsQuery
            .OrderBy(ticket => ticket.Id)
            .ToList()
            .Select(ToDTO)
            .ToList();

        return Ok(tickets);
    }

    [HttpGet("{id}")]
    public ActionResult<TicketDTO> Get(int id)
    {
        var ticket = ActiveTicketsQuery().FirstOrDefault(ticket => ticket.Id == id);

        if (ticket is null)
        {
            return NotFound();
        }

        return Ok(ToDTO(ticket));
    }

    [HttpGet("pretraga/{query}")]
    public ActionResult<IEnumerable<TicketDTO>> Search(string query, TicketStatus? status)
    {
        var normalizedQuery = query.Trim();

        var ticketsQuery = ActiveTicketsQuery();

        if (status.HasValue)
        {
            ticketsQuery = ticketsQuery.Where(ticket => ticket.Status == status.Value);
        }

        if (!string.IsNullOrWhiteSpace(normalizedQuery))
        {
            ticketsQuery = ticketsQuery.Where(ticket =>
                ticket.TicketNumber.Contains(normalizedQuery) ||
                (ticket.Customer.FirstName + " " + ticket.Customer.LastName).Contains(normalizedQuery) ||
                ticket.Screening.Movie.Title.Contains(normalizedQuery) ||
                ticket.Screening.Hall.Name.Contains(normalizedQuery));
        }

        var tickets = ticketsQuery
            .OrderBy(ticket => ticket.Id)
            .ToList()
            .Select(ToDTO)
            .ToList();

        return Ok(tickets);
    }

    [HttpPost]
    public ActionResult<TicketDTO> Post([FromBody] TicketWriteDTO dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var validationError = ValidateTicketWriteDto(dto);
        if (validationError is not null)
        {
            return BadRequest(validationError);
        }

        var ticket = new Ticket
        {
            TicketNumber = dto.TicketNumber,
            PurchasedAt = dto.PurchasedAt,
            Price = dto.Price,
            Status = dto.Status,
            ScreeningId = dto.ScreeningId,
            SeatId = dto.SeatId,
            CustomerId = dto.CustomerId
        };

        _dbContext.Tickets.Add(ticket);
        _dbContext.SaveChanges();

        var createdTicket = ActiveTicketsQuery().FirstOrDefault(existing => existing.Id == ticket.Id);

        return CreatedAtAction(nameof(Get), new { id = ticket.Id }, ToDTO(createdTicket ?? ticket));
    }

    [HttpPut("{id}")]
    public ActionResult<TicketDTO> Put(int id, [FromBody] TicketWriteDTO dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var ticket = _dbContext.Tickets.FirstOrDefault(ticket => ticket.Id == id && ticket.DeletedAt == null);

        if (ticket is null)
        {
            return NotFound();
        }

        var validationError = ValidateTicketWriteDto(dto);
        if (validationError is not null)
        {
            return BadRequest(validationError);
        }

        ticket.TicketNumber = dto.TicketNumber;
        ticket.PurchasedAt = dto.PurchasedAt;
        ticket.Price = dto.Price;
        ticket.Status = dto.Status;
        ticket.ScreeningId = dto.ScreeningId;
        ticket.SeatId = dto.SeatId;
        ticket.CustomerId = dto.CustomerId;

        _dbContext.SaveChanges();

        var updatedTicket = ActiveTicketsQuery().FirstOrDefault(existing => existing.Id == id);

        return Ok(ToDTO(updatedTicket ?? ticket));
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        var ticket = _dbContext.Tickets.FirstOrDefault(ticket => ticket.Id == id && ticket.DeletedAt == null);

        if (ticket is null)
        {
            return NotFound();
        }

        ticket.DeletedAt = DateTime.UtcNow;
        _dbContext.SaveChanges();

        return NoContent();
    }

    private IQueryable<Ticket> ActiveTicketsQuery()
    {
        return _dbContext.Tickets
            .Include(ticket => ticket.Customer)
            .Include(ticket => ticket.Seat)
                .ThenInclude(seat => seat!.Hall)
                    .ThenInclude(hall => hall.Cinema)
            .Include(ticket => ticket.Screening)
                .ThenInclude(screening => screening.Movie)
            .Include(ticket => ticket.Screening)
                .ThenInclude(screening => screening.Hall)
                    .ThenInclude(hall => hall.Cinema)
            .Where(ticket => ticket.DeletedAt == null
                && ticket.Customer.DeletedAt == null
                && ticket.Screening.DeletedAt == null
                && ticket.Screening.Movie.DeletedAt == null
                && ticket.Screening.Hall.DeletedAt == null
                && ticket.Screening.Hall.Cinema.DeletedAt == null
                && (ticket.Seat == null
                    || (ticket.Seat.DeletedAt == null
                        && ticket.Seat.Hall.DeletedAt == null
                        && ticket.Seat.Hall.Cinema.DeletedAt == null)));
    }

    private object? ValidateTicketWriteDto(TicketWriteDTO dto)
    {
        var customerExists = _dbContext.Customers.Any(customer => customer.Id == dto.CustomerId && customer.DeletedAt == null);
        if (!customerExists)
        {
            return new { error = "Odabrani kupac ne postoji." };
        }

        var screening = _dbContext.Screenings
            .Include(existing => existing.Movie)
            .Include(existing => existing.Hall)
                .ThenInclude(hall => hall.Cinema)
            .FirstOrDefault(existing => existing.Id == dto.ScreeningId
                && existing.DeletedAt == null
                && existing.Movie.DeletedAt == null
                && existing.Hall.DeletedAt == null
                && existing.Hall.Cinema.DeletedAt == null);

        if (screening is null)
        {
            return new { error = "Odabrana projekcija ne postoji." };
        }

        if (dto.SeatId.HasValue)
        {
            var seat = _dbContext.Seats
                .Include(existing => existing.Hall)
                    .ThenInclude(hall => hall.Cinema)
                .FirstOrDefault(existing => existing.Id == dto.SeatId.Value
                    && existing.DeletedAt == null
                    && existing.Hall.DeletedAt == null
                    && existing.Hall.Cinema.DeletedAt == null);

            if (seat is null)
            {
                return new { error = "Odabrano sjedalo ne postoji." };
            }

            if (seat.HallId != screening.HallId)
            {
                return new { error = "Odabrano sjedalo ne pripada dvorani projekcije." };
            }
        }

        return null;
    }

    private static TicketDTO ToDTO(Ticket ticket)
    {
        return new TicketDTO
        {
            Id = ticket.Id,
            TicketNumber = ticket.TicketNumber,
            PurchasedAt = ticket.PurchasedAt,
            Price = ticket.Price,
            Status = ticket.Status,
            Customer = ToCustomerDTO(ticket.Customer),
            Screening = ToScreeningDTO(ticket.Screening),
            Movie = ToMovieDTO(ticket.Screening.Movie),
            Hall = ToHallDTO(ticket.Screening.Hall),
            Seat = ticket.Seat == null ? null : ToSeatDTO(ticket.Seat)
        };
    }

    private static CustomerDTO ToCustomerDTO(Customer customer)
    {
        return customer == null ? new CustomerDTO() : new CustomerDTO
        {
            Id = customer.Id,
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            City = customer.City,
            TicketCount = customer.Tickets.Count(ticket => ticket.DeletedAt == null)
        };
    }

    private static ScreeningDTO ToScreeningDTO(Screening screening)
    {
        return screening == null ? new ScreeningDTO() : new ScreeningDTO
        {
            Id = screening.Id,
            StartTime = screening.StartTime,
            EndTime = screening.EndTime,
            Is3D = screening.Is3D,
            Movie = ToMovieDTO(screening.Movie),
            Hall = ToHallDTO(screening.Hall)
        };
    }

    private static MovieDTO ToMovieDTO(Movie movie)
    {
        return movie == null ? new MovieDTO() : new MovieDTO
        {
            Id = movie.Id,
            Title = movie.Title,
            Description = movie.Description,
            DurationMinutes = movie.DurationMinutes,
            ReleaseDate = movie.ReleaseDate,
            Genre = movie.Genre,
            Language = movie.Language,
            AgeRating = movie.AgeRating
        };
    }

    private static SeatDTO ToSeatDTO(Seat seat)
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
