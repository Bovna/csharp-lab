using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Vjezba.DAL;
using Vjezba.Model.Entities;
using Vjezba.Web.ViewModels;

namespace Vjezba.Web.Controllers;

[Route("projekcije")]
public class ScreeningController : Controller
{
    private readonly CinemaDbContext _dbContext;

    public ScreeningController(CinemaDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [Route("pretraga")]
    public IActionResult Index(int? dayOfWeek, string? search, bool partial = false)
    {
        var normalizedSearch = (search ?? string.Empty).Trim();
        var query = _dbContext.Screenings
            .Where(screening => screening.DeletedAt == null
                && screening.Movie.DeletedAt == null
                && screening.Hall.DeletedAt == null
                && screening.Hall.Cinema.DeletedAt == null)
            .Include(s => s.Movie)
            .Include(s => s.Hall)
                .ThenInclude(h => h.Cinema)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(normalizedSearch))
        {
            query = query.Where(screening => EF.Functions.Like(screening.Movie.Title, $"%{normalizedSearch}%"));
        }

        var screenings = query
            .OrderBy(screening => screening.Id)
            .ToList();

        if (dayOfWeek.HasValue)
        {
            var targetDayOfWeek = (DayOfWeek)(dayOfWeek.Value % 7);

            screenings = screenings
                .Where(screening => screening.StartTime.DayOfWeek == targetDayOfWeek)
                .ToList();
        }

        ViewBag.SelectedDayOfWeek = dayOfWeek;
        ViewBag.Search = search;

        if (partial)
        {
            return PartialView("_IndexResults", screenings);
        }

        return View(screenings);
    }

    public IActionResult Search(string? query)
    {
        var normalizedQuery = (query ?? string.Empty).Trim();

        var halls = _dbContext.Halls
            .Include(hall => hall.Cinema)
            .Where(hall => string.IsNullOrEmpty(normalizedQuery)
                || EF.Functions.Like(hall.Name, $"%{normalizedQuery}%")
                || EF.Functions.Like(hall.Cinema.Name + " - " + hall.Name, $"%{normalizedQuery}%"))
            .Where(hall => hall.DeletedAt == null && hall.Cinema.DeletedAt == null)
            .OrderBy(hall => hall.Cinema.Name)
            .ThenBy(hall => hall.Name)
            .Take(12)
            .Select(hall => new
            {
                value = hall.Id,
                text = hall.Cinema.Name + " - " + hall.Name
            })
            .ToList();

        return Json(halls);
    }

    [Route("detalji/{id}")]
    public IActionResult Details(int id)
    {
        var screening = _dbContext.Screenings
            .Where(s => s.DeletedAt == null
                && s.Movie.DeletedAt == null
                && s.Hall.DeletedAt == null
                && s.Hall.Cinema.DeletedAt == null)
            .Include(s => s.Movie)
            .Include(s => s.Hall)
                .ThenInclude(h => h.Cinema)
            .FirstOrDefault(s => s.Id == id);

        if (screening is null)
        {
            return NotFound();
        }

        var tickets = _dbContext.Tickets
            .Where(t => t.ScreeningId == screening.Id
                && t.DeletedAt == null
                && t.Customer.DeletedAt == null
                && (t.Seat == null || t.Seat.DeletedAt == null))
            .Include(t => t.Customer)
            .Include(t => t.Seat)
            .OrderByDescending(t => t.PurchasedAt)
            .ToList();

        var viewModel = new ScreeningDetailsViewModel
        {
            Screening = screening,
            Tickets = tickets
        };

        return View(viewModel);
    }

    [Route("dodaj")]
    public IActionResult Create()
    {
        var model = new ScreeningFormViewModel
        {
            StartTime = DateTime.Now,
            EndTime = DateTime.Now.AddHours(2)
        };

        PrepareScreeningForm(model, isCreate: true);
        return View(model);
    }

    [HttpPost("dodaj")]
    public IActionResult Create(ScreeningFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            PrepareScreeningForm(model, isCreate: true);
            return View(model);
        }

        var screening = new Screening
        {
            StartTime = model.StartTime,
            EndTime = model.EndTime,
            Is3D = model.Is3D,
            MovieId = model.MovieId!.Value,
            HallId = model.HallId!.Value
        };

        _dbContext.Screenings.Add(screening);
        _dbContext.SaveChanges();

        return RedirectToAction(nameof(Index));
    }

    [Route("uredi/{id}")]
    [ActionName("Edit")]
    public IActionResult EditGet(int id)
    {
        var screening = _dbContext.Screenings.FirstOrDefault(s => s.Id == id && s.DeletedAt == null);

        if (screening is null)
        {
            return NotFound();
        }

        var model = new ScreeningFormViewModel
        {
            Id = screening.Id,
            StartTime = screening.StartTime,
            EndTime = screening.EndTime,
            Is3D = screening.Is3D,
            MovieId = screening.MovieId,
            HallId = screening.HallId
        };

        PrepareScreeningForm(model, isCreate: false);
        return View(model);
    }

    [HttpPost("uredi/{id}")]
    [ActionName("Edit")]
    public async Task<IActionResult> EditPost(int id)
    {
        var screening = _dbContext.Screenings.FirstOrDefault(s => s.Id == id && s.DeletedAt == null);

        if (screening is null)
        {
            return NotFound();
        }

        var ok = await TryUpdateModelAsync(screening, string.Empty,
            s => s.StartTime,
            s => s.EndTime,
            s => s.Is3D,
            s => s.MovieId,
            s => s.HallId);

        if (ok && ModelState.IsValid)
        {
            _dbContext.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        var model = new ScreeningFormViewModel
        {
            Id = screening.Id,
            StartTime = screening.StartTime,
            EndTime = screening.EndTime,
            Is3D = screening.Is3D,
            MovieId = screening.MovieId,
            HallId = screening.HallId
        };

        PrepareScreeningForm(model, isCreate: false);

        return View(model);
    }

    [HttpPost("obrisi/{id}")]
    public IActionResult Delete(int id)
    {
        var screening = _dbContext.Screenings.FirstOrDefault(s => s.Id == id && s.DeletedAt == null);

        if (screening is null)
        {
            return NotFound();
        }

        screening.DeletedAt = DateTime.UtcNow;
        SoftDeleteScreening(screening);
        _dbContext.SaveChanges();

        return RedirectToAction(nameof(Index));
    }

    private void SoftDeleteScreening(Screening screening)
    {
        var deletedAt = screening.DeletedAt ?? DateTime.UtcNow;
        screening.DeletedAt = deletedAt;

        var tickets = _dbContext.Tickets
            .Where(ticket => ticket.ScreeningId == screening.Id && ticket.DeletedAt == null)
            .ToList();

        foreach (var ticket in tickets)
        {
            ticket.DeletedAt = deletedAt;
        }
    }

    private void PrepareScreeningForm(ScreeningFormViewModel model, bool isCreate = false)
    {
        model.MovieSelector = new AutocompleteViewModel
        {
            InputName = nameof(model.MovieId),
            Label = "Film",
            Endpoint = Url.Action(nameof(MovieController.Search), "Movie") ?? "/filmovi/search",
            SearchPlaceholder = "Pretražite film po naslovu",
            RequiredMessage = "Film je obavezan.",
            Items = BuildSelectItems(
                _dbContext.Movies
                    .Where(movie => movie.DeletedAt == null)
                    .OrderBy(movie => movie.Title)
                    .Select(movie => new SelectListItem
                    {
                        Value = movie.Id.ToString(),
                        Text = movie.Title,
                        Selected = model.MovieId.HasValue && movie.Id == model.MovieId.Value
                    })
                    .ToList(),
                model.MovieId,
                isCreate)
        };

        model.HallSelector = new AutocompleteViewModel
        {
            InputName = nameof(model.HallId),
            Label = "Dvorana",
            Endpoint = Url.Action(nameof(Search), "Screening") ?? "/projekcije/search",
            SearchPlaceholder = "Pretražite dvoranu po kinu ili nazivu",
            RequiredMessage = "Dvorana je obavezna.",
            Items = BuildSelectItems(
                _dbContext.Halls
                    .Include(hall => hall.Cinema)
                    .Where(hall => hall.DeletedAt == null && hall.Cinema.DeletedAt == null)
                    .OrderBy(hall => hall.Cinema.Name)
                    .ThenBy(hall => hall.Name)
                    .Select(hall => new SelectListItem
                    {
                        Value = hall.Id.ToString(),
                        Text = hall.Cinema.Name + " - " + hall.Name,
                        Selected = model.HallId.HasValue && hall.Id == model.HallId.Value
                    })
                    .ToList(),
                model.HallId,
                isCreate)
        };
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
