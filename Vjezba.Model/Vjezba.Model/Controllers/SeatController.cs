using Microsoft.AspNetCore.Mvc;
using Vjezba.Model.Data;

namespace Vjezba.Model.Controllers;

public class SeatController : Controller
{
    private readonly SeatMockRepository _seatRepository;
    private readonly TicketMockRepository _ticketRepository;

    public SeatController(SeatMockRepository seatRepository, TicketMockRepository ticketRepository)
    {
        _seatRepository = seatRepository;
        _ticketRepository = ticketRepository;
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

        seat.Tickets = _ticketRepository
            .GetAll()
            .Where(t => t.Seat?.Id == seat.Id)
            .OrderByDescending(t => t.PurchasedAt)
            .ToList();

        return View(seat);
    }
}
