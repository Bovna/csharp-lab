using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Vjezba.DAL;
using Vjezba.Model.Entities;
using Vjezba.Web.ViewModels;

namespace Vjezba.Web.Controllers;

[AutoValidateAntiforgeryToken]
[Route("ulaznice")]
[Authorize(Roles = "Admin,Manager")]
public class TicketController : Controller
{
    private readonly CinemaDbContext _dbContext;
    private readonly ILogger<TicketController> _logger;

    public TicketController(CinemaDbContext dbContext, ILogger<TicketController> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    [Route("")]
    [Route("pretraga")]
    public IActionResult Index(TicketStatus? status, bool partial = false)
    {
        var ticketsQuery = ActiveTicketsQuery();

        if (status.HasValue)
        {
            ticketsQuery = ticketsQuery.Where(ticket => ticket.Status == status.Value);
        }

        ViewBag.SelectedStatus = status?.ToString();
        ViewBag.Search = null;

        var tickets = ticketsQuery.OrderBy(ticket => ticket.Id).ToList();

        if (partial)
        {
            return PartialView("_IndexResults", tickets);
        }

        return View(tickets);
    }

    [HttpGet("rezultati")]
    public IActionResult Search(string? query, TicketStatus? status, bool partial = false)
    {
        var normalizedQuery = (query ?? string.Empty).Trim();
        var ticketsQuery = ActiveTicketsQuery();

        if (status.HasValue)
        {
            ticketsQuery = ticketsQuery.Where(ticket => ticket.Status == status.Value);
        }

        if (!string.IsNullOrWhiteSpace(normalizedQuery))
        {
            ticketsQuery = ticketsQuery.Where(ticket =>
                ticket.TicketNumber.Contains(normalizedQuery) ||
                (ticket.Customer.FirstName + " " + ticket.Customer.LastName).Contains(normalizedQuery) ||
                ticket.Screening.Movie.Title.Contains(normalizedQuery) ||
                ticket.Screening.Hall.Name.Contains(normalizedQuery));
        }

        ViewBag.SelectedStatus = status?.ToString();
        ViewBag.Search = query;

        var tickets = ticketsQuery.OrderBy(ticket => ticket.Id).ToList();

        if (partial)
        {
            return PartialView("_IndexResults", tickets);
        }

        return View(nameof(Index), tickets);
    }

    [Route("detalji/{id}")]
    [Authorize]
    public IActionResult Details(int id)
    {
        var ticket = ActiveTicketsQuery().FirstOrDefault(ticket => ticket.Id == id);

        if (ticket is null)
        {
            return NotFound();
        }

        return View(ticket);
    }

    [Route("dodaj")]
    [Authorize(Roles = "Admin,Manager")]
    public IActionResult Create()
    {
        var model = new TicketFormViewModel
        {
            PurchasedAt = DateTime.Now,
            Status = TicketStatus.Active
        };

        PrepareTicketForm(model);
        return View(model);
    }

    [HttpPost("dodaj")]
    [Authorize(Roles = "Admin,Manager")]
    public IActionResult Create(TicketFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            PrepareTicketForm(model);
            return View(model);
        }

        ValidateTicketBusinessRules(model);

        if (!ModelState.IsValid)
        {
            PrepareTicketForm(model);
            return View(model);
        }

        var ticket = new Ticket { ConfirmationCode = Guid.NewGuid() };
        MapTicketForm(model, ticket);

        _dbContext.Tickets.Add(ticket);
        try
        {
            _dbContext.SaveChanges();
        }
        catch (DbUpdateException) when (IsSeatAlreadyReserved(model.ScreeningId!.Value, model.SeatId))
        {
            ModelState.AddModelError(nameof(model.SeatId), "Odabrano sjedalo je već rezervirano za tu projekciju.");
            PrepareTicketForm(model);
            return View(model);
        }

        _logger.LogInformation(
            "Ticket created by MVC. TicketId={TicketId}, TicketNumber={TicketNumber}, ScreeningId={ScreeningId}, SeatId={SeatId}, CustomerId={CustomerId}, Status={Status}, UserId={UserId}",
            ticket.Id,
            ticket.TicketNumber,
            ticket.ScreeningId,
            ticket.SeatId,
            ticket.CustomerId,
            ticket.Status,
            GetCurrentUserId());

        return RedirectToAction(nameof(Index));
    }

    [Route("uredi/{id}")]
    [ActionName("Edit")]
    [Authorize(Roles = "Admin,Manager")]
    public IActionResult EditGet(int id)
    {
        var ticket = ActiveTicketsQuery().FirstOrDefault(ticket => ticket.Id == id);

        if (ticket is null)
        {
            return NotFound();
        }

        var model = ToTicketForm(ticket);
        PrepareTicketForm(model);
        return View(model);
    }

    [HttpPost("uredi/{id}")]
    [ActionName("Edit")]
    [Authorize(Roles = "Admin,Manager")]
    public IActionResult EditPost(int id, TicketFormViewModel model)
    {
        var ticket = ActiveTicketsQuery().FirstOrDefault(ticket => ticket.Id == id);

        if (ticket is null)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            model.Id = id;
            PrepareTicketForm(model);
            return View(model);
        }

        ValidateTicketBusinessRules(model, id);

        if (!ModelState.IsValid)
        {
            model.Id = id;
            PrepareTicketForm(model);
            return View(model);
        }

        MapTicketForm(model, ticket);

        try
        {
            _dbContext.SaveChanges();
        }
        catch (DbUpdateException) when (IsSeatAlreadyReserved(model.ScreeningId!.Value, model.SeatId, id))
        {
            ModelState.AddModelError(nameof(model.SeatId), "Odabrano sjedalo je već rezervirano za tu projekciju.");
            PrepareTicketForm(model);
            return View(model);
        }

        _logger.LogInformation(
            "Ticket updated by MVC. TicketId={TicketId}, TicketNumber={TicketNumber}, ScreeningId={ScreeningId}, SeatId={SeatId}, CustomerId={CustomerId}, Status={Status}, UserId={UserId}",
            ticket.Id,
            ticket.TicketNumber,
            ticket.ScreeningId,
            ticket.SeatId,
            ticket.CustomerId,
            ticket.Status,
            GetCurrentUserId());

        return RedirectToAction(nameof(Index));
    }

    [HttpPost("obrisi/{id}")]
    [Authorize(Roles = "Admin")]
    public IActionResult Delete(int id)
    {
        var ticket = ActiveTicketsQuery().FirstOrDefault(ticket => ticket.Id == id);

        if (ticket is null)
        {
            return NotFound();
        }

        ticket.DeletedAt = DateTime.UtcNow;
        _dbContext.SaveChanges();

        _logger.LogInformation(
            "Ticket soft deleted by MVC. TicketId={TicketId}, TicketNumber={TicketNumber}, UserId={UserId}",
            ticket.Id,
            ticket.TicketNumber,
            GetCurrentUserId());

        return RedirectToAction(nameof(Index));
    }

    private string GetCurrentUserId()
    {
        return User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "unknown";
    }

    private IQueryable<Ticket> ActiveTicketsQuery()
    {
        return _dbContext.Tickets
            .Include(ticket => ticket.Customer)
            .Include(ticket => ticket.Seat)
                .ThenInclude(seat => seat!.Hall)
                    .ThenInclude(hall => hall.Cinema)
            .Include(ticket => ticket.Screening)
                .ThenInclude(screening => screening.Movie)
            .Include(ticket => ticket.Screening)
                .ThenInclude(screening => screening.Hall)
                    .ThenInclude(hall => hall.Cinema)
            .Where(ticket => ticket.DeletedAt == null
                && ticket.Customer.DeletedAt == null
                && ticket.Screening.DeletedAt == null
                && ticket.Screening.Movie.DeletedAt == null
                && ticket.Screening.Hall.DeletedAt == null
                && ticket.Screening.Hall.Cinema.DeletedAt == null
                && (ticket.Seat == null
                    || (ticket.Seat.DeletedAt == null
                        && ticket.Seat.Hall.DeletedAt == null
                        && ticket.Seat.Hall.Cinema.DeletedAt == null)));
    }

    private void PrepareTicketForm(TicketFormViewModel model)
    {
        model.CustomerSelector = new AutocompleteViewModel
        {
            InputName = nameof(model.CustomerId),
            Label = "Kupac",
            Endpoint = Url.Action(nameof(CustomerController.Autocomplete), "Customer") ?? "/kupci/autocomplete",
            SearchPlaceholder = "Pretrazite kupca po imenu",
            RequiredMessage = "Kupac je obavezan.",
            EnableRemoteSearch = true,
            Items = BuildSelectedCustomerItems(model.CustomerId)
        };

        model.ScreeningSelector = new AutocompleteViewModel
        {
            InputName = nameof(model.ScreeningId),
            Label = "Projekcija",
            Endpoint = Url.Action(nameof(ScreeningController.ScreeningAutocomplete), "Screening") ?? "/projekcije/autocomplete",
            SearchPlaceholder = "Pretrazite projekciju po filmu",
            RequiredMessage = "Projekcija je obavezna.",
            EnableRemoteSearch = true,
            Items = BuildSelectedScreeningItems(model.ScreeningId)
        };

        model.SeatSelector = new AutocompleteViewModel
        {
            InputName = nameof(model.SeatId),
            Label = "Sjedalo",
            Endpoint = Url.Action(nameof(SeatController.Autocomplete), "Seat") ?? "/sjedala/autocomplete",
            SearchPlaceholder = "Pretrazite sjedalo po oznaci",
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
            .Where(customer => customer.DeletedAt == null && customer.Id == selectedCustomerId.Value)
            .Select(customer => new SelectListItem
            {
                Value = customer.Id.ToString(),
                Text = customer.FirstName + " " + customer.LastName,
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
            .Include(screening => screening.Movie)
            .Include(screening => screening.Hall)
                .ThenInclude(hall => hall.Cinema)
            .Where(screening => screening.DeletedAt == null
                && screening.Movie.DeletedAt == null
                && screening.Hall.DeletedAt == null
                && screening.Hall.Cinema.DeletedAt == null
                && screening.Id == selectedScreeningId.Value)
            .Select(screening => new SelectListItem
            {
                Value = screening.Id.ToString(),
                Text = screening.Movie.Title + " - " + screening.Hall.Cinema.Name + " / " + screening.Hall.Name + " - " + screening.StartTime.ToString("dd.MM.yyyy HH:mm"),
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
                .ThenInclude(hall => hall.Cinema)
            .Where(seat => seat.DeletedAt == null
                && seat.Hall.DeletedAt == null
                && seat.Hall.Cinema.DeletedAt == null
                && seat.Id == selectedSeatId.Value)
            .Select(seat => new SelectListItem
            {
                Value = seat.Id.ToString(),
                Text = seat.Hall.Cinema.Name + " - " + seat.Hall.Name + " - " + seat.RowLabel + seat.SeatNumber,
                Selected = true
            })
            .ToList();
    }

    private static TicketFormViewModel ToTicketForm(Ticket ticket)
    {
        return new TicketFormViewModel
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
    }

    private static void MapTicketForm(TicketFormViewModel model, Ticket ticket)
    {
        ticket.TicketNumber = model.TicketNumber;
        ticket.PurchasedAt = model.PurchasedAt;
        ticket.Price = model.Price;
        ticket.Status = model.Status;
        ticket.ScreeningId = model.ScreeningId!.Value;
        ticket.SeatId = model.SeatId;
        ticket.CustomerId = model.CustomerId!.Value;
    }

    private void ValidateTicketBusinessRules(TicketFormViewModel model, int? excludedTicketId = null)
    {
        if (!model.ScreeningId.HasValue)
        {
            return;
        }

        var screening = _dbContext.Screenings
            .Include(existing => existing.Movie)
            .Include(existing => existing.Hall)
                .ThenInclude(hall => hall.Cinema)
            .FirstOrDefault(existing => existing.Id == model.ScreeningId.Value
                && existing.DeletedAt == null
                && existing.Movie.DeletedAt == null
                && existing.Hall.DeletedAt == null
                && existing.Hall.Cinema.DeletedAt == null);

        if (screening is null)
        {
            return;
        }

        if (screening.EndTime <= DateTime.Now)
        {
            ModelState.AddModelError(nameof(model.ScreeningId), "Nije moguće kupiti kartu za završenu projekciju.");
        }

        if (!model.SeatId.HasValue)
        {
            return;
        }

        var seat = _dbContext.Seats
            .Include(existing => existing.Hall)
                .ThenInclude(hall => hall.Cinema)
            .FirstOrDefault(existing => existing.Id == model.SeatId.Value
                && existing.DeletedAt == null
                && existing.Hall.DeletedAt == null
                && existing.Hall.Cinema.DeletedAt == null);

        if (seat is not null && seat.HallId != screening.HallId)
        {
            ModelState.AddModelError(nameof(model.SeatId), "Odabrano sjedalo ne pripada dvorani projekcije.");
            return;
        }

        if (seat is not null
            && OccupiesSeat(model.Status)
            && IsSeatAlreadyReserved(model.ScreeningId.Value, model.SeatId, excludedTicketId))
        {
            ModelState.AddModelError(nameof(model.SeatId), "Odabrano sjedalo je već rezervirano za tu projekciju.");
        }
    }

    private bool IsSeatAlreadyReserved(int screeningId, int? seatId, int? excludedTicketId = null)
    {
        return seatId.HasValue && _dbContext.Tickets.Any(ticket =>
            ticket.Id != excludedTicketId
            && ticket.ScreeningId == screeningId
            && ticket.SeatId == seatId
            && ticket.DeletedAt == null
            && (ticket.Status == TicketStatus.Active || ticket.Status == TicketStatus.Used));
    }

    private static bool OccupiesSeat(TicketStatus status)
    {
        return status is TicketStatus.Active or TicketStatus.Used;
    }
}
