using Microsoft.AspNetCore.Mvc;
using Vjezba.DAL.Repositories;

namespace Vjezba.Web.Controllers;

public class MovieController : Controller
{
    private readonly MovieRepository _movieRepository;

    public MovieController(MovieRepository movieRepository)
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
