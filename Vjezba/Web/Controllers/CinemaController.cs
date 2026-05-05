using Microsoft.AspNetCore.Mvc;
using Vjezba.DAL.Repositories;

namespace Vjezba.Web.Controllers;

public class CinemaController : Controller
{
    private readonly CinemaRepository _cinemaRepository;

    public CinemaController(CinemaRepository cinemaRepository)
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
