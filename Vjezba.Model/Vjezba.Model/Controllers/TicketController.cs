using Microsoft.AspNetCore.Mvc;
using Vjezba.Model.Data;

namespace Vjezba.Model.Controllers;

public class TicketController : Controller
{
    private readonly TicketMockRepository _ticketRepository;
    private readonly ScreeningMockRepository _screeningRepository;

    public TicketController(TicketMockRepository ticketRepository, ScreeningMockRepository screeningRepository)
    {
        _ticketRepository = ticketRepository;
        _screeningRepository = screeningRepository;
    }

    public IActionResult Index()
    {
        var screeningsById = _screeningRepository.GetAll().ToDictionary(s => s.Id);
        var tickets = _ticketRepository.GetAll();

        foreach (var ticket in tickets)
        {
            if (ticket.Screening is not null && screeningsById.TryGetValue(ticket.Screening.Id, out var screening))
            {
                ticket.Screening = screening;
            }
        }

        return View(tickets);
    }

    public IActionResult Details(int id)
    {
        var ticket = _ticketRepository.GetById(id);

        if (ticket is null)
        {
            return NotFound();
        }

        var screening = _screeningRepository.GetById(ticket.Screening?.Id ?? 0);
        if (screening is not null)
        {
            ticket.Screening = screening;
        }

        return View(ticket);
    }
}
