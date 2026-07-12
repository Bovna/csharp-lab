using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using KinoKlik.DAL;
using KinoKlik.Model.Entities;
using KinoKlik.Web.Models;
using KinoKlik.Web.ViewModels;

namespace KinoKlik.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly CinemaDbContext _dbContext;
        private readonly ILogger<HomeController> _logger;

        public HomeController(CinemaDbContext dbContext, ILogger<HomeController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var now = DateTime.Now;
                var upcomingScreenings = await GetUpcomingScreeningsAsync(now, take: 8);
                var nextScreeningByMovie = upcomingScreenings
                    .GroupBy(screening => screening.MovieId)
                    .ToDictionary(group => group.Key, group => group.First());

                var movieEntities = await _dbContext.Movies
                    .AsNoTracking()
                    .Include(movie => movie.Attachments)
                    .Where(movie => movie.DeletedAt == null && movie.Title != "Signal 404")
                    .ToListAsync();

                var movies = movieEntities
                    .Select(movie => ToHomeMovieCard(movie, nextScreeningByMovie))
                    .OrderBy(movie => movie.Title != "Dnevnik Sjevera")
                    .ThenBy(movie => movie.NextScreening is null)
                    .ThenBy(movie => movie.NextScreening?.StartTime ?? DateTime.MaxValue)
                    .ThenByDescending(movie => movie.ReleaseDate)
                    .Take(6)
                    .ToList();

                var model = new HomeIndexViewModel
                {
                    FeaturedMovie = movies.FirstOrDefault(),
                    MoviesNowShowing = movies,
                    UpcomingScreenings = upcomingScreenings,
                    Cinemas = await GetCinemaSummariesAsync()
                };

                return View(model);
            }
            catch (Exception exception) when (IsDatabaseStarting(exception))
            {
                _logger.LogWarning(
                    exception,
                    "The cinema database is still resuming. TraceIdentifier={TraceIdentifier}",
                    HttpContext.TraceIdentifier);

                Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
                Response.Headers.RetryAfter = "30";

                return View("DatabaseStarting");
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = HttpContext.TraceIdentifier,
                OperationId = Activity.Current?.Id
            });
        }

        private Task<List<HomeScreeningSummaryViewModel>> GetUpcomingScreeningsAsync(DateTime now, int take)
        {
            return _dbContext.Screenings
                .AsNoTracking()
                .Where(screening => screening.DeletedAt == null
                    && screening.Movie.DeletedAt == null
                    && screening.Hall.DeletedAt == null
                    && screening.Hall.Cinema.DeletedAt == null
                    && screening.StartTime >= now)
                .OrderBy(screening => screening.StartTime)
                .Take(take)
                .Select(screening => new HomeScreeningSummaryViewModel
                {
                    Id = screening.Id,
                    MovieId = screening.MovieId,
                    CinemaId = screening.Hall.CinemaId,
                    MovieTitle = screening.Movie.Title,
                    CinemaName = screening.Hall.Cinema.Name,
                    HallName = screening.Hall.Name,
                    StartTime = screening.StartTime,
                    EndTime = screening.EndTime,
                    Is3D = screening.Is3D
                })
                .ToListAsync();
        }

        private Task<List<HomeCinemaSummaryViewModel>> GetCinemaSummariesAsync()
        {
            return _dbContext.Cinemas
                .AsNoTracking()
                .Where(cinema => cinema.DeletedAt == null)
                .OrderBy(cinema => cinema.City)
                .ThenBy(cinema => cinema.Name)
                .Select(cinema => new HomeCinemaSummaryViewModel
                {
                    Id = cinema.Id,
                    Name = cinema.Name,
                    City = cinema.City,
                    Address = cinema.Street + " " + cinema.HouseNumber,
                    HallCount = cinema.Halls.Count(hall => hall.DeletedAt == null),
                    SeatCount = cinema.Halls
                        .Where(hall => hall.DeletedAt == null)
                        .SelectMany(hall => hall.Seats)
                        .Count(seat => seat.DeletedAt == null),
                    Has3D = cinema.Halls.Any(hall => hall.DeletedAt == null && hall.Supports3D)
                })
                .Take(5)
                .ToListAsync();
        }

        private static bool IsDatabaseStarting(Exception exception)
        {
            const int connectionTimeout = -2;
            const int databaseUnavailable = 40613;

            for (Exception? current = exception; current is not null; current = current.InnerException)
            {
                if (current is SqlException sqlException
                    && sqlException.Number is connectionTimeout or databaseUnavailable)
                {
                    return true;
                }
            }

            return false;
        }

        private static HomeMovieCardViewModel ToHomeMovieCard(
            Movie movie,
            IReadOnlyDictionary<int, HomeScreeningSummaryViewModel> nextScreeningByMovie)
        {
            nextScreeningByMovie.TryGetValue(movie.Id, out var nextScreening);

            return new HomeMovieCardViewModel
            {
                Id = movie.Id,
                Title = movie.Title,
                Description = movie.Description,
                DurationMinutes = movie.DurationMinutes,
                ReleaseDate = movie.ReleaseDate,
                Genre = movie.Genre,
                Language = movie.Language,
                AgeRating = movie.AgeRating,
                PosterPath = movie.Attachments
                    .OrderByDescending(attachment => attachment.CreatedAt)
                    .Select(attachment => attachment.FilePath)
                    .FirstOrDefault(),
                NextScreening = nextScreening
            };
        }
    }
}
