using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Vjezba.DAL;
using Vjezba.Model.Entities;
using Vjezba.Web.ViewModels;

namespace Vjezba.Web.Controllers;

[Route("ulaznice")]
public class TicketController : Controller
{
    private readonly CinemaDbContext _dbContext;

    public TicketController(CinemaDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [Route("")]
    public IActionResult Index(decimal? price, string? search, bool partial = false)
    {
        var normalizedSearch = (search ?? string.Empty).Trim();
        var query = _dbContext.Tickets
            .Where(ticket => ticket.DeletedAt == null
                && ticket.Customer.DeletedAt == null
                && ticket.Screening.DeletedAt == null
                && ticket.Screening.Hall.DeletedAt == null
                && ticket.Screening.Hall.Cinema.DeletedAt == null
                && (ticket.Seat == null || ticket.Seat.DeletedAt == null))
            .Include(t => t.Customer)
            .Include(t => t.Seat)
            .Include(t => t.Screening)
                .ThenInclude(s => s.Movie)
            .Include(t => t.Screening)
                .ThenInclude(s => s.Hall)
                    .ThenInclude(h => h.Cinema)
            .AsQueryable();

        if (price.HasValue)
        {
            query = query.Where(ticket => ticket.Price <= price.Value);
        }

        if (!string.IsNullOrWhiteSpace(normalizedSearch))
        {
            query = query.Where(ticket =>
                EF.Functions.Like(ticket.TicketNumber, $"%{normalizedSearch}%")
                || EF.Functions.Like(ticket.Customer.FirstName + " " + ticket.Customer.LastName, $"%{normalizedSearch}%")
                || EF.Functions.Like(ticket.Screening.Movie.Title, $"%{normalizedSearch}%"));
        }

        var tickets = query
            .OrderBy(ticket => ticket.Id)
            .ToList();

        ViewBag.SelectedPrice = price?.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture);
        ViewBag.Search = search;

        if (partial)
        {
            return PartialView("_IndexResults", tickets);
        }

        return View(tickets);
    }

    [Route("detalji/{id}")]
    public IActionResult Details(int id)
    {
        var ticket = _dbContext.Tickets
            .Include(t => t.Customer)
            .Include(t => t.Seat)
            .Include(t => t.Screening)
                .ThenInclude(s => s.Movie)
            .Include(t => t.Screening)
                .ThenInclude(s => s.Hall)
                    .ThenInclude(h => h.Cinema)
            .FirstOrDefault(t => t.Id == id
                && t.DeletedAt == null
                && t.Customer.DeletedAt == null
                && t.Screening.DeletedAt == null
                && t.Screening.Hall.DeletedAt == null
                && t.Screening.Hall.Cinema.DeletedAt == null
                && (t.Seat == null || t.Seat.DeletedAt == null));

        if (ticket is null)
        {
            return NotFound();
        }

        return View(ticket);
    }

    [Route("dodaj")]
    public IActionResult Create()
    {
        var model = new TicketFormViewModel
        {
            PurchasedAt = DateTime.Now,
            Status = TicketStatus.Active
        };

        PrepareTicketForm(model, isCreate: true);
        return View(model);
    }

    [HttpPost("dodaj")]
    public IActionResult Create(TicketFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            PrepareTicketForm(model, isCreate: true);
            return View(model);
        }

        var ticket = new Ticket
        {
            TicketNumber = model.TicketNumber,
            PurchasedAt = model.PurchasedAt,
            Price = model.Price,
            Status = model.Status,
            ScreeningId = model.ScreeningId!.Value,
            SeatId = model.SeatId,
            CustomerId = model.CustomerId!.Value
        };

        _dbContext.Tickets.Add(ticket);
        _dbContext.SaveChanges();

        return RedirectToAction(nameof(Index));
    }

    [Route("uredi/{id}")]
    [ActionName("Edit")]
    public IActionResult EditGet(int id)
    {
        var ticket = _dbContext.Tickets.FirstOrDefault(t => t.Id == id && t.DeletedAt == null);

        if (ticket is null)
        {
            return NotFound();
        }

        var model = new TicketFormViewModel
        {
            Id = ticket.Id,
            TicketNumber = ticket.TicketNumber,
            PurchasedAt = ticket.PurchasedAt,
            Price = ticket.Price,
            Status = ticket.Status,
            ScreeningId = ticket.ScreeningId,
            SeatId = ticket.SeatId,
            CustomerId = ticket.CustomerId
        };

        PrepareTicketForm(model, isCreate: false);
        return View(model);
    }

    [HttpPost("uredi/{id}")]
    [ActionName("Edit")]
    public async Task<IActionResult> EditPost(int id)
    {
        var ticket = _dbContext.Tickets.FirstOrDefault(t => t.Id == id && t.DeletedAt == null);

        if (ticket is null)
        {
            return NotFound();
        }

        var model = new TicketFormViewModel
        {
            Id = ticket.Id,
            TicketNumber = ticket.TicketNumber,
            PurchasedAt = ticket.PurchasedAt,
            Price = ticket.Price,
            Status = ticket.Status,
            ScreeningId = ticket.ScreeningId,
            SeatId = ticket.SeatId,
            CustomerId = ticket.CustomerId
        };

        var ok = await TryUpdateModelAsync(model, string.Empty,
            m => m.TicketNumber,
            m => m.PurchasedAt,
            m => m.Price,
            m => m.Status,
            m => m.ScreeningId,
            m => m.SeatId,
            m => m.CustomerId);

        if (ok && ModelState.IsValid)
        {
            ticket.TicketNumber = model.TicketNumber;
            ticket.PurchasedAt = model.PurchasedAt;
            ticket.Price = model.Price;
            ticket.Status = model.Status;
            ticket.ScreeningId = model.ScreeningId!.Value;
            ticket.SeatId = model.SeatId;
            ticket.CustomerId = model.CustomerId!.Value;

            _dbContext.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        PrepareTicketForm(model, isCreate: false);
        return View(model);
    }

    [HttpPost("obrisi/{id}")]
    public IActionResult Delete(int id)
    {
        var ticket = _dbContext.Tickets.FirstOrDefault(t => t.Id == id && t.DeletedAt == null);

        if (ticket is null)
        {
            return NotFound();
        }

        ticket.DeletedAt = DateTime.UtcNow;
        _dbContext.SaveChanges();

        return RedirectToAction(nameof(Index));
    }

    private void PrepareTicketForm(TicketFormViewModel model, bool isCreate = false)
    {
        model.CustomerSelector = new AutocompleteViewModel
        {
            InputName = nameof(model.CustomerId),
            Label = "Kupac",
            Endpoint = Url.Action(nameof(CustomerController.Search), "Customer") ?? "/kupci/pretraga",
            SearchPlaceholder = "Pretražite kupca po imenu",
            RequiredMessage = "Kupac je obavezan.",
            EnableRemoteSearch = true,
            Items = BuildSelectedCustomerItems(model.CustomerId)
        };

        model.ScreeningSelector = new AutocompleteViewModel
        {
            InputName = nameof(model.ScreeningId),
            Label = "Projekcija",
            Endpoint = Url.Action(nameof(ScreeningController.Search), "Screening") ?? "/projekcije/search",
            SearchPlaceholder = "Pretražite projekciju po filmu",
            RequiredMessage = "Projekcija je obavezna.",
            EnableRemoteSearch = true,
            Items = BuildSelectedScreeningItems(model.ScreeningId)
        };

        model.SeatSelector = new AutocompleteViewModel
        {
            InputName = nameof(model.SeatId),
            Label = "Sjedalo",
            Endpoint = Url.Action(nameof(SeatController.Search), "Seat") ?? "/sjedala/pretraga",
            SearchPlaceholder = "Pretražite sjedalo po oznaci",
            RequiredMessage = string.Empty,
            EnableRemoteSearch = true,
            Items = BuildSelectedSeatItems(model.SeatId)
        };
    }

    private List<SelectListItem> BuildSelectedCustomerItems(int? selectedCustomerId)
    {
        if (!selectedCustomerId.HasValue)
        {
            return new List<SelectListItem>();
        }

        return _dbContext.Customers
            .Where(c => c.DeletedAt == null && c.Id == selectedCustomerId.Value)
            .Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.FirstName + " " + c.LastName,
                Selected = true
            })
            .ToList();
    }

    private List<SelectListItem> BuildSelectedScreeningItems(int? selectedScreeningId)
    {
        if (!selectedScreeningId.HasValue)
        {
            return new List<SelectListItem>();
        }

        return _dbContext.Screenings
            .Include(s => s.Movie)
            .Include(s => s.Hall)
                .ThenInclude(h => h.Cinema)
            .Where(s => s.DeletedAt == null && s.Id == selectedScreeningId.Value)
            .Select(s => new SelectListItem
            {
                Value = s.Id.ToString(),
                Text = s.Movie.Title + " - " + s.Hall.Cinema.Name + " / " + s.Hall.Name + " - " + s.StartTime.ToString("dd.MM.yyyy HH:mm"),
                Selected = true
            })
            .ToList();
    }

    private List<SelectListItem> BuildSelectedSeatItems(int? selectedSeatId)
    {
        if (!selectedSeatId.HasValue)
        {
            return new List<SelectListItem>();
        }

        return _dbContext.Seats
            .Include(seat => seat.Hall)
                .ThenInclude(h => h.Cinema)
            .Where(seat => seat.DeletedAt == null && seat.Id == selectedSeatId.Value)
            .Select(seat => new SelectListItem
            {
                Value = seat.Id.ToString(),
                Text = seat.Hall.Cinema.Name + " - " + seat.Hall.Name + " - " + seat.RowLabel + seat.SeatNumber,
                Selected = true
            })
            .ToList();
    }

    private static List<SelectListItem> BuildSelectItems(List<SelectListItem> items, int? selectedValue, bool isCreate = false)
    {
        var selectItems = new List<SelectListItem>();

        if (!isCreate)
        {
            selectItems.Add(new SelectListItem
            {
                Text = "- odaberite -",
                Value = string.Empty,
                Selected = !selectedValue.HasValue
            });
        }

        selectItems.AddRange(items);
        return selectItems;
    }
}
