using Microsoft.AspNetCore.Mvc;
using Vjezba.Model.Data;

namespace Vjezba.Model.Controllers;

public class CinemaController : Controller
{
    private readonly CinemaMockRepository _cinemaRepository;

    public CinemaController(CinemaMockRepository cinemaRepository)
    {
        _cinemaRepository = cinemaRepository;
    }

    public IActionResult Index()
    {
        var cinemas = _cinemaRepository.GetAll();

        return View(cinemas);
    }

    public IActionResult Details(int id)
    {
        var cinema = _cinemaRepository.GetById(id);

        if (cinema is null)
        {
            return NotFound();
        }

        return View(cinema);
    }
}
