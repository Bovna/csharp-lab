using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vjezba.DAL;
using Vjezba.Model.Entities;
using Vjezba.Web.DTOs;

namespace Vjezba.Web.Controllers.Api;

[ApiController]
[Authorize]
[Route("api/film")]
public class MovieApiController : ControllerBase
{
    private readonly CinemaDbContext _dbContext;

    public MovieApiController(CinemaDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public ActionResult<IEnumerable<MovieDTO>> Get(string? language)
    {
        var normalizedLanguage = (language ?? string.Empty).Trim();

        var moviesQuery = _dbContext.Movies
            .Where(movie => movie.DeletedAt == null)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(normalizedLanguage))
        {
            moviesQuery = moviesQuery.Where(movie => movie.Language == normalizedLanguage);
        }

        var movies = moviesQuery
            .OrderBy(movie => movie.Id)
            .ToList()
            .Select(ToDTO)
            .ToList();

        return Ok(movies);
    }

    [HttpGet("{id}")]
    public ActionResult<MovieDTO> Get(int id)
    {
        var movie = _dbContext.Movies.FirstOrDefault(movie => movie.Id == id && movie.DeletedAt == null);

        if (movie is null)
        {
            return NotFound();
        }

        return Ok(ToDTO(movie));
    }

    [HttpGet("pretraga/{query}")]
    public ActionResult<IEnumerable<MovieDTO>> Search(string query, string? language)
    {
        var normalizedQuery = query.Trim();
        var normalizedLanguage = (language ?? string.Empty).Trim();

        var moviesQuery = _dbContext.Movies
            .Where(movie => movie.DeletedAt == null)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(normalizedLanguage))
        {
            moviesQuery = moviesQuery.Where(movie => movie.Language == normalizedLanguage);
        }

        if (!string.IsNullOrWhiteSpace(normalizedQuery))
        {
            moviesQuery = moviesQuery.Where(movie =>
                movie.Title.Contains(normalizedQuery) ||
                movie.Description.Contains(normalizedQuery));
        }

        var movies = moviesQuery
            .OrderBy(movie => movie.Id)
            .ToList()
            .Select(ToDTO)
            .ToList();

        return Ok(movies);
    }

    [HttpPost]
    public ActionResult<MovieDTO> Post([FromBody] MovieWriteDTO dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var movie = new Movie
        {
            Title = dto.Title,
            Description = dto.Description,
            DurationMinutes = dto.DurationMinutes,
            ReleaseDate = dto.ReleaseDate,
            Genre = dto.Genre,
            Language = dto.Language,
            AgeRating = dto.AgeRating
        };

        _dbContext.Movies.Add(movie);
        _dbContext.SaveChanges();

        return CreatedAtAction(nameof(Get), new { id = movie.Id }, ToDTO(movie));
    }

    [HttpPut("{id}")]
    public ActionResult<MovieDTO> Put(int id, [FromBody] MovieWriteDTO dto)
    {

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var existing = _dbContext.Movies.FirstOrDefault(m => m.Id == id && m.DeletedAt == null);

        if (existing is null)
        {
            return NotFound();
        }

        existing.Title = dto.Title;
        existing.Description = dto.Description;
        existing.DurationMinutes = dto.DurationMinutes;
        existing.ReleaseDate = dto.ReleaseDate;
        existing.Genre = dto.Genre;
        existing.Language = dto.Language;
        existing.AgeRating = dto.AgeRating;

        _dbContext.SaveChanges();

        return Ok(ToDTO(existing));
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        var movie = _dbContext.Movies.FirstOrDefault(m => m.Id == id && m.DeletedAt == null);

        if (movie is null)
        {
            return NotFound();
        }

        SoftDeleteMovies(movie);
        _dbContext.SaveChanges();

        return NoContent();
    }

    private void SoftDeleteMovies(Movie movie)
    {
        var deletedAt = DateTime.UtcNow;
        movie.DeletedAt = deletedAt;

        var screenings = _dbContext.Screenings
            .Where(s => s.MovieId == movie.Id && s.DeletedAt == null)
            .ToList();

        foreach (var s in screenings)
        {
            s.DeletedAt = deletedAt;
        }

        var screeningIds = screenings.Select(s => s.Id).ToList();

        var tickets = _dbContext.Tickets
            .Where(t => t.DeletedAt == null && screeningIds.Contains(t.ScreeningId))
            .ToList();

        foreach (var t in tickets)
        {
            t.DeletedAt = deletedAt;
        }

        var favorites = _dbContext.CustomerFavoriteMovies
            .Where(f => f.MovieId == movie.Id && f.DeletedAt == null)
            .ToList();

        foreach (var f in favorites)
        {
            f.DeletedAt = deletedAt;
        }
    }

    private static MovieDTO ToDTO(Movie movie)
    {
        return new MovieDTO
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
}
