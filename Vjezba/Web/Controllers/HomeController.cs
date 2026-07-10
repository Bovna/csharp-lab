using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Vjezba.DAL;
using Vjezba.Model.Entities;
using Vjezba.Web.Models;
using Vjezba.Web.ViewModels;

namespace Vjezba.Web.Controllers
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

        public IActionResult Index()
        {
            var now = DateTime.Now;
            var upcomingScreenings = GetUpcomingScreenings(now, take: 8);
            var nextScreeningByMovie = upcomingScreenings
                .GroupBy(screening => screening.MovieId)
                .ToDictionary(group => group.Key, group => group.First());

            var movies = _dbContext.Movies
                .AsNoTracking()
                .Include(movie => movie.Attachments)
                .Where(movie => movie.DeletedAt == null && movie.Title != "Signal 404")
                .ToList()
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
                Cinemas = GetCinemaSummaries()
            };

            return View(model);
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

        private List<HomeScreeningSummaryViewModel> GetUpcomingScreenings(DateTime now, int take)
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
                .ToList();
        }

        private List<HomeCinemaSummaryViewModel> GetCinemaSummaries()
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
                .ToList();
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
