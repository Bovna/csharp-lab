using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vjezba.DAL;
using Vjezba.Model.Entities;

namespace Vjezba.Web.Controllers;

[Route("kina")]
public class CinemaController : Controller
{
    private readonly CinemaDbContext _dbContext;

    public CinemaController(CinemaDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet("")]
    [HttpGet("grad={city}")]
    public IActionResult Index(string city)
    {
        var query = _dbContext.Cinemas
            .Include(c => c.Halls)
            .AsQueryable();

        ViewBag.Cities = _dbContext.Cinemas
            .Select(c => c.City)
            .Where(c => c != null && c != "")
            .Distinct()
            .OrderBy(c => c)
            .ToList();

        ViewBag.SelectedCity = city;

        if (!string.IsNullOrWhiteSpace(city))
        {
            query = query.Where(c => c.City == city);
        }

        var cinemas = query.ToList();

        return View(cinemas);
    }

    [HttpGet("detalji/{id}")]
    public IActionResult Details(int id)
    {
        var cinema = _dbContext.Cinemas
            .Include(c => c.Halls)
            .FirstOrDefault(c => c.Id == id);

        if (cinema is null)
        {
            return NotFound();
        }

        return View(cinema);
    }

    [HttpGet("create")]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Cinema cinema)
    {
        if (!ModelState.IsValid)
        {
            return View(cinema);
        }

        _dbContext.Cinemas.Add(cinema);
        _dbContext.SaveChanges();

        return RedirectToAction(nameof(Index));
    }

    [HttpGet("edit/{id}")]
    public IActionResult Edit(int id)
    {
        var cinema = _dbContext.Cinemas.FirstOrDefault(c => c.Id == id);

        if (cinema is null)
        {
            return NotFound();
        }

        return View(cinema);
    }

    [HttpPost("edit/{id}")]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, Cinema cinema)
    {
        if (id != cinema.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return View(cinema);
        }

        _dbContext.Cinemas.Update(cinema);
        _dbContext.SaveChanges();

        return RedirectToAction(nameof(Details), new { id = cinema.Id });
    }

    [HttpPost("delete/{id}")]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int id)
    {
        var cinema = _dbContext.Cinemas.Find(id);

        if (cinema is null)
        {
            return NotFound();
        }

        _dbContext.Cinemas.Remove(cinema);
        _dbContext.SaveChanges();

        return RedirectToAction(nameof(Index));
    }
}
