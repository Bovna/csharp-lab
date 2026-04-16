using Microsoft.AspNetCore.Mvc;
using Vjezba.Model.Data;

namespace Vjezba.Model.Controllers;

public class SeatController : Controller
{
    private readonly SeatMockRepository _seatRepository;

    public SeatController(SeatMockRepository seatRepository)
    {
        _seatRepository = seatRepository;
    }

    public IActionResult Index()
    {
        var seats = _seatRepository.GetAll();

        return View(seats);
    }

    public IActionResult Details(int id)
    {
        var seat = _seatRepository.GetById(id);

        if (seat is null)
        {
            return NotFound();
        }

        return View(seat);
    }
}
