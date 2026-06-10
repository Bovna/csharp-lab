using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vjezba.DAL;
using Vjezba.Model.Entities;
using Vjezba.Web.DTOs;

namespace Vjezba.Web.Controllers.Api;

[ApiController]
[Route("api/projekcije")]
public class ScreeningApiController : ControllerBase
{
    private readonly CinemaDbContext _dbContext;

    public ScreeningApiController(CinemaDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public ActionResult<IEnumerable<ScreeningDTO>> Get()
    {
        var screenings = _dbContext.Screenings
            .Include(screening => screening.Movie)
            .Include(screening => screening.Hall)
                .ThenInclude(hall => hall.Cinema)
            .Where(screening =>
                screening.DeletedAt == null &&
                screening.Movie != null && screening.Movie.DeletedAt == null &&
                screening.Hall != null && screening.Hall.DeletedAt == null &&
                screening.Hall.Cinema != null && screening.Hall.Cinema.DeletedAt == null)
            .ToList()
            .Select(ToDTO)
            .ToList();

        return Ok(screenings);
    }

    [HttpGet("{id}")]
    public ActionResult<ScreeningDTO> Get(int id)
    {
        var screening = _dbContext.Screenings
            .Include(screening => screening.Movie)
            .Include(screening => screening.Hall)
                .ThenInclude(hall => hall.Cinema)
            .Where(screening => screening.DeletedAt == null
                && screening.Movie != null && screening.Movie.DeletedAt == null
                && screening.Hall != null && screening.Hall.DeletedAt == null
                && screening.Hall.Cinema != null && screening.Hall.Cinema.DeletedAt == null)
            .FirstOrDefault(screening => screening.Id == id);

        if (screening is null)
        {
            return NotFound();
        }

        return Ok(ToDTO(screening));
    }

    [HttpGet("pretraga/{query}")]
    public ActionResult<IEnumerable<ScreeningDTO>> Search(string query)
    {
        var normalizedQuery = query.Trim();

        var screenings = _dbContext.Screenings
            .Include(screening => screening.Movie)
            .Include(screening => screening.Hall)
                .ThenInclude(hall => hall.Cinema)
            .Where(screening => screening.DeletedAt == null
                && screening.Movie != null && screening.Movie.DeletedAt == null
                && screening.Hall != null && screening.Hall.DeletedAt == null
                && screening.Hall.Cinema != null && screening.Hall.Cinema.DeletedAt == null)
            .Where(screening => screening.Movie.Title.Contains(normalizedQuery)
                || screening.Hall.Name.Contains(normalizedQuery)
                || screening.Hall.Cinema.Name.Contains(normalizedQuery))
            .ToList()
            .Select(ToDTO)
            .ToList();

        return Ok(screenings);
    }

    [HttpPost]
    public ActionResult<ScreeningDTO> Post([FromBody] ScreeningWriteDTO screening)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var entity = new Screening
        {
            StartTime = screening.StartTime,
            EndTime = screening.EndTime,
            Is3D = screening.Is3D,
            MovieId = screening.MovieId,
            HallId = screening.HallId
        };

        _dbContext.Screenings.Add(entity);
        _dbContext.SaveChanges();

        var createdScreening = _dbContext.Screenings
            .Include(s => s.Movie)
            .Include(s => s.Hall)
                .ThenInclude(h => h.Cinema)
            .FirstOrDefault(s => s.Id == entity.Id);

        return CreatedAtAction(nameof(Get), new { id = entity.Id }, ToDTO(createdScreening ?? entity));
    }

    [HttpPut("{id}")]
    public ActionResult<ScreeningDTO> Put(int id, [FromBody] ScreeningWriteDTO screening)
    {
        var existingScreening = _dbContext.Screenings.FirstOrDefault(s => s.Id == id);

        if (existingScreening is null)
        {
            return NotFound();
        }

        existingScreening.StartTime = screening.StartTime;
        existingScreening.EndTime = screening.EndTime;
        existingScreening.Is3D = screening.Is3D;
        existingScreening.MovieId = screening.MovieId;
        existingScreening.HallId = screening.HallId;
        _dbContext.SaveChanges();


        var updatedScreening = _dbContext.Screenings
            .Include(s => s.Movie)
            .Include(s => s.Hall)
                .ThenInclude(h => h.Cinema)
            .FirstOrDefault(s => s.Id == id);

        return Ok(ToDTO(updatedScreening ?? existingScreening));
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        var screening = _dbContext.Screenings.FirstOrDefault(s => s.Id == id && s.DeletedAt == null);

        if (screening is null)
        {
            return NotFound();
        }

        SoftDeleteScreening(screening);
        _dbContext.SaveChanges();

        return Ok();
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
                CinemaId = screening.Hall.CinemaId,
                CinemaName = screening.Hall.Cinema == null ? string.Empty : screening.Hall.Cinema.Name
            }
        };
    }
}