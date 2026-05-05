using Microsoft.AspNetCore.Mvc;
using Vjezba.DAL.Repositories;
using Vjezba.Web.ViewModels;

namespace Vjezba.Web.Controllers;

public class HallController : Controller
{
    private readonly HallRepository _hallRepository;
    private readonly SeatRepository _seatRepository;
    private readonly ScreeningRepository _screeningRepository;

    public HallController(
        HallRepository hallRepository,
        SeatRepository seatRepository,
        ScreeningRepository screeningRepository)
    {
        _hallRepository = hallRepository;
        _seatRepository = seatRepository;
        _screeningRepository = screeningRepository;
    }

    public IActionResult Index()
    {
        var halls = _hallRepository.GetAll();

        return View(halls);
    }

    public IActionResult Details(int id)
    {
        var hall = _hallRepository.GetById(id);

        if (hall is null)
        {
            return NotFound();
        }

        var seats = _seatRepository.GetAll()
            .Where(s => s.Hall?.Id == hall.Id)
            .OrderBy(s => s.RowLabel)
            .ThenBy(s => s.SeatNumber)
            .ToList();

        var screenings = _screeningRepository.GetAll()
            .Where(s => s.Hall?.Id == hall.Id)
            .OrderBy(s => s.StartTime)
            .ToList();

        var viewModel = new HallDetailsViewModel
        {
            Hall = hall,
            Seats = seats,
            Screenings = screenings
        };

        return View(viewModel);
    }
}
