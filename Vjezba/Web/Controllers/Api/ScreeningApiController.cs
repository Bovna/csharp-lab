using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vjezba.DAL;
using Vjezba.Model.Entities;
using Vjezba.Web.DTOs;

namespace Vjezba.Web.Controllers.Api;

[ApiController]
[Authorize]
[Route("api/projekcije")]
public class ScreeningApiController : ControllerBase
{
    private readonly CinemaDbContext _dbContext;

    public ScreeningApiController(CinemaDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    [AllowAnonymous]
    public ActionResult<IEnumerable<ScreeningDTO>> Get(int? dayOfWeek)
    {
        var screeningsQuery = ActiveScreeningsQuery();

        if (dayOfWeek.HasValue)
        {
            var targetDayOfWeek = (DayOfWeek)(dayOfWeek.Value % 7);
            screeningsQuery = screeningsQuery.Where(screening => screening.StartTime.DayOfWeek == targetDayOfWeek);
        }

        var screenings = screeningsQuery
            .OrderBy(screening => screening.Id)
            .ToList()
            .Select(ToDTO)
            .ToList();

        return Ok(screenings);
    }

    [HttpGet("{id}")]
    public ActionResult<ScreeningDTO> Get(int id)
    {
        var screening = ActiveScreeningsQuery()
            .FirstOrDefault(screening => screening.Id == id);

        if (screening is null)
        {
            return NotFound();
        }

        return Ok(ToDTO(screening));
    }

    [HttpGet("pretraga/{query}")]
    [AllowAnonymous]
    public ActionResult<IEnumerable<ScreeningDTO>> Search(string query, int? dayOfWeek)
    {
        var normalizedQuery = query.Trim();
        var screeningsQuery = ActiveScreeningsQuery();

        if (dayOfWeek.HasValue)
        {
            var targetDayOfWeek = (DayOfWeek)(dayOfWeek.Value % 7);
            screeningsQuery = screeningsQuery.Where(screening => screening.StartTime.DayOfWeek == targetDayOfWeek);
        }

        if (!string.IsNullOrWhiteSpace(normalizedQuery))
        {
            screeningsQuery = screeningsQuery.Where(screening =>
                screening.Movie.Title.Contains(normalizedQuery) ||
                screening.Hall.Name.Contains(normalizedQuery) ||
                screening.Hall.Cinema.Name.Contains(normalizedQuery));
        }

        var screenings = screeningsQuery
            .OrderBy(screening => screening.Id)
            .ToList()
            .Select(ToDTO)
            .ToList();

        return Ok(screenings);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Manager")]
    public ActionResult<ScreeningDTO> Post([FromBody] ScreeningWriteDTO dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var validationError = ValidateScreeningWriteDto(dto);
        if (validationError is not null)
        {
            return BadRequest(validationError);
        }

        var screening = new Screening
        {
            StartTime = dto.StartTime,
            EndTime = dto.EndTime,
            Is3D = dto.Is3D,
            MovieId = dto.MovieId,
            HallId = dto.HallId
        };

        _dbContext.Screenings.Add(screening);
        _dbContext.SaveChanges();

        var createdScreening = ActiveScreeningsQuery()
            .FirstOrDefault(existing => existing.Id == screening.Id);

        return CreatedAtAction(nameof(Get), new { id = screening.Id }, ToDTO(createdScreening ?? screening));
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Manager")]
    public ActionResult<ScreeningDTO> Put(int id, [FromBody] ScreeningWriteDTO dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var existingScreening = _dbContext.Screenings.FirstOrDefault(screening => screening.Id == id && screening.DeletedAt == null);

        if (existingScreening is null)
        {
            return NotFound();
        }

        var validationError = ValidateScreeningWriteDto(dto, id);
        if (validationError is not null)
        {
            return BadRequest(validationError);
        }

        existingScreening.StartTime = dto.StartTime;
        existingScreening.EndTime = dto.EndTime;
        existingScreening.Is3D = dto.Is3D;
        existingScreening.MovieId = dto.MovieId;
        existingScreening.HallId = dto.HallId;

        _dbContext.SaveChanges();

        var updatedScreening = ActiveScreeningsQuery()
            .FirstOrDefault(screening => screening.Id == id);

        return Ok(ToDTO(updatedScreening ?? existingScreening));
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public ActionResult Delete(int id)
    {
        var screening = _dbContext.Screenings.FirstOrDefault(screening => screening.Id == id && screening.DeletedAt == null);

        if (screening is null)
        {
            return NotFound();
        }

        SoftDeleteScreening(screening);
        _dbContext.SaveChanges();

        return NoContent();
    }

    private IQueryable<Screening> ActiveScreeningsQuery()
    {
        return _dbContext.Screenings
            .Include(screening => screening.Movie)
            .Include(screening => screening.Hall)
                .ThenInclude(hall => hall.Cinema)
            .Where(screening =>
                screening.DeletedAt == null &&
                screening.Movie.DeletedAt == null &&
                screening.Hall.DeletedAt == null &&
                screening.Hall.Cinema.DeletedAt == null);
    }

    private object? ValidateScreeningWriteDto(ScreeningWriteDTO dto, int? currentScreeningId = null)
    {
        if (dto.EndTime <= dto.StartTime)
        {
            return new { error = "Vrijeme završetka mora biti nakon vremena početka." };
        }

        var movieExists = _dbContext.Movies.Any(movie => movie.Id == dto.MovieId && movie.DeletedAt == null);
        if (!movieExists)
        {
            return new { error = "Odabrani film ne postoji." };
        }

        var hall = _dbContext.Halls
            .Include(hall => hall.Cinema)
            .FirstOrDefault(hall => hall.Id == dto.HallId
                && hall.DeletedAt == null
                && hall.Cinema.DeletedAt == null);

        if (hall is null)
        {
            return new { error = "Odabrana dvorana ne postoji." };
        }

        if (dto.Is3D && !hall.Supports3D)
        {
            return new { error = "Odabrana dvorana ne podržava 3D projekcije." };
        }

        var hasOverlappingScreening = _dbContext.Screenings.Any(screening =>
            screening.DeletedAt == null
            && screening.HallId == dto.HallId
            && (!currentScreeningId.HasValue || screening.Id != currentScreeningId.Value)
            && screening.StartTime < dto.EndTime
            && dto.StartTime < screening.EndTime);

        if (hasOverlappingScreening)
        {
            return new { error = "U odabranoj dvorani već postoji projekcija u tom terminu." };
        }

        return null;
    }

    private void SoftDeleteScreening(Screening screening)
    {
        var deletedAt = screening.DeletedAt ?? DateTime.UtcNow;
        screening.DeletedAt = deletedAt;

        var tickets = _dbContext.Tickets
            .Where(ticket => ticket.ScreeningId == screening.Id && ticket.DeletedAt == null)
            .ToList();

        foreach (var ticket in tickets)
        {
            ticket.DeletedAt = deletedAt;
        }
    }

    private static ScreeningDTO ToDTO(Screening screening)
    {
        return new ScreeningDTO
        {
            Id = screening.Id,
            StartTime = screening.StartTime,
            EndTime = screening.EndTime,
            Is3D = screening.Is3D,
            Movie = screening.Movie == null ? new MovieDTO() : new MovieDTO
            {
                Id = screening.Movie.Id,
                Title = screening.Movie.Title,
                Description = screening.Movie.Description,
                DurationMinutes = screening.Movie.DurationMinutes,
                ReleaseDate = screening.Movie.ReleaseDate,
                Genre = screening.Movie.Genre,
                Language = screening.Movie.Language,
                AgeRating = screening.Movie.AgeRating
            },
            Hall = screening.Hall == null ? new HallDTO() : new HallDTO
            {
                Id = screening.Hall.Id,
                Name = screening.Hall.Name,
                Capacity = screening.Hall.Capacity,
                Supports3D = screening.Hall.Supports3D,
                CinemaId = screening.Hall.CinemaId,
                CinemaName = screening.Hall.Cinema == null ? string.Empty : screening.Hall.Cinema.Name
            }
        };
    }
}
