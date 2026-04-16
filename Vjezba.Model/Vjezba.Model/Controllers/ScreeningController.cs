using Microsoft.AspNetCore.Mvc;
using Vjezba.Model.Data;
using Vjezba.Model.Models.ViewModels;

namespace Vjezba.Model.Controllers;

public class ScreeningController : Controller
{
    private readonly ScreeningMockRepository _screeningRepository;
    private readonly TicketMockRepository _ticketRepository;

    public ScreeningController(ScreeningMockRepository screeningRepository, TicketMockRepository ticketRepository)
    {
        _screeningRepository = screeningRepository;
        _ticketRepository = ticketRepository;
    }

    public IActionResult Index()
    {
        var screenings = _screeningRepository.GetAll();

        return View(screenings);
    }

    public IActionResult Details(int id)
    {
        var screening = _screeningRepository.GetById(id);

        if (screening is null)
        {
            return NotFound();
        }

        var tickets = _ticketRepository.GetAll()
            .Where(t => t.Screening?.Id == screening.Id)
            .OrderByDescending(t => t.PurchasedAt)
            .ToList();

        var viewModel = new ScreeningDetailsViewModel
        {
            Screening = screening,
            Tickets = tickets
        };

        return View(viewModel);
    }
}
