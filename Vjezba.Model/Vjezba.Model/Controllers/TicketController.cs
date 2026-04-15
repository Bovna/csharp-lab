using Microsoft.AspNetCore.Mvc;
using Vjezba.Model.Data;

namespace Vjezba.Model.Controllers;

public class TicketController : Controller
{
    private readonly TicketMockRepository _ticketRepository;

    public TicketController(TicketMockRepository ticketRepository)
    {
        _ticketRepository = ticketRepository;
    }

    public IActionResult Index()
    {
        var tickets = _ticketRepository.GetAll();

        return View(tickets);
    }

    public IActionResult Details(int id)
    {
        var ticket = _ticketRepository.GetById(id);

        if (ticket is null)
        {
            return NotFound();
        }

        return View(ticket);
    }
}
