using Microsoft.AspNetCore.Mvc;
using Vjezba.Model.Data;

namespace Vjezba.Model.Controllers;

public class MovieController : Controller
{
    private readonly MovieMockRepository _movieRepository;

    public MovieController(MovieMockRepository movieRepository)
    {
        _movieRepository = movieRepository;
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

        return View(movie);
    }
}
