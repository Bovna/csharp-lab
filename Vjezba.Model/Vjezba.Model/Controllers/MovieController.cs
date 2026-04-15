using Microsoft.AspNetCore.Mvc;
using Vjezba.Model.Data;

namespace Vjezba.Model.Controllers;

public class MovieController : Controller
{
    private readonly MovieMockRepository _movieRepository;
    private readonly ScreeningMockRepository _screeningRepository;

    public MovieController(MovieMockRepository movieRepository, ScreeningMockRepository screeningRepository)
    {
        _movieRepository = movieRepository;
        _screeningRepository = screeningRepository;
    }

    public IActionResult Index()
    {
        var movies = _movieRepository.GetAll();

        return View(movies);
    }

    public IActionResult Details(int id)
    {
        var movie = _movieRepository.GetById(id);

        if (movie is null)
        {
            return NotFound();
        }

        movie.Screenings = _screeningRepository
            .GetAll()
            .Where(s => s.Movie?.Id == movie.Id)
            .OrderBy(s => s.StartTime)
            .ToList();

        return View(movie);
    }
}
