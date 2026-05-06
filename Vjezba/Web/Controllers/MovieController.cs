using Microsoft.AspNetCore.Mvc;
using Vjezba.DAL;
using Vjezba.Model.Entities;

namespace Vjezba.Web.Controllers;

[Route("filmovi")]
public class MovieController : Controller
{
    private readonly CinemaDbContext _dbContext;

    public MovieController(CinemaDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet("")]
    [HttpGet("jezik={language}")]
    public IActionResult Index(string language)
    {
        var query = _dbContext.Movies.AsQueryable();

        ViewBag.Languages = _dbContext.Movies
            .Select(m => m.Language)
            .Where(l => l != null && l != "")
            .Distinct()
            .OrderBy(l => l)
            .ToList();

        ViewBag.SelectedLanguage = language;

        if (!string.IsNullOrWhiteSpace(language))
        {
            query = query.Where(m => m.Language == language);
        }

        var movies = query.ToList();

        return View(movies);
    }

    [HttpGet("detalji/{id}")]
    public IActionResult Details(int id)
    {
        var movie = _dbContext.Movies.FirstOrDefault(m => m.Id == id);

        if (movie is null)
        {
            return NotFound();
        }

        return View(movie);
    }

    [HttpGet("uredi/{id}")]
    public IActionResult Edit(int id)
    {
        var movie = _dbContext.Movies.FirstOrDefault(m => m.Id == id);
        if (movie is null)
            return NotFound();

        return View(movie);
    }

    [HttpPost("uredi/{id}")]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, Movie movie)
    {
        if (id != movie.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return View(movie);
        }

        _dbContext.Movies.Update(movie);
        _dbContext.SaveChanges();

        return RedirectToAction(nameof(Details), new { id = movie.Id });
    }

    [HttpPost("obrisi/{id}")]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int id)
    {
        var movie = _dbContext.Movies.Find(id);
        if (movie is null)
            return NotFound();

        _dbContext.Movies.Remove(movie);
        _dbContext.SaveChanges();

        return RedirectToAction(nameof(Index));
    }


    [HttpGet("dodaj")]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost("dodaj")]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Movie movie)
    {
        if (!ModelState.IsValid)
        {
            return View(movie);
        }

        _dbContext.Movies.Add(movie);
        _dbContext.SaveChanges();

        return RedirectToAction(nameof(Index));
    }
}
