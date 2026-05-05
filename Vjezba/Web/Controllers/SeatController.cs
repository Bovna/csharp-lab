using Microsoft.AspNetCore.Mvc;
using Vjezba.DAL.Repositories;

namespace Vjezba.Web.Controllers;

public class SeatController : Controller
{
    private readonly SeatRepository _seatRepository;

    public SeatController(SeatRepository seatRepository)
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
