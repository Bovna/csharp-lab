using Microsoft.AspNetCore.Mvc;
using Vjezba.Model.Data;

namespace Vjezba.Model.Controllers;

public class CustomerController : Controller
{
    private readonly CustomerMockRepository _customerRepository;
    private readonly TicketMockRepository _ticketRepository;

    public CustomerController(CustomerMockRepository customerRepository, TicketMockRepository ticketRepository)
    {
        _customerRepository = customerRepository;
        _ticketRepository = ticketRepository;
    }

    public IActionResult Index()
    {
        var customers = _customerRepository.GetAll();

        return View(customers);
    }

    public IActionResult Details(int id)
    {
        var customer = _customerRepository.GetById(id);

        if (customer is null)
        {
            return NotFound();
        }

        customer.Tickets = _ticketRepository
            .GetAll()
            .Where(t => t.Customer?.Id == customer.Id)
            .OrderByDescending(t => t.PurchasedAt)
            .ToList();

        return View(customer);
    }
}
