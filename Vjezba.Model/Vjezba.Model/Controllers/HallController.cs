using Microsoft.AspNetCore.Mvc;
using Vjezba.Model.Data;
using Vjezba.Model.Models.ViewModels;

namespace Vjezba.Model.Controllers;

public class HallController : Controller
{
    private readonly HallMockRepository _hallRepository;
    private readonly SeatMockRepository _seatRepository;
    private readonly ScreeningMockRepository _screeningRepository;

    public HallController(
        HallMockRepository hallRepository,
        SeatMockRepository seatRepository,
        ScreeningMockRepository screeningRepository)
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
