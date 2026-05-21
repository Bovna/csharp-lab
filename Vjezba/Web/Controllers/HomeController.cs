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

        public async Task<IActionResult> Index()
        {
            var featuredMovies = await _dbContext.Movies
                .AsNoTracking()
                .Where(movie => movie.DeletedAt == null)
                .OrderByDescending(movie => movie.ReleaseDate)
                .ThenBy(movie => movie.Title)
                .Take(5)
                .Select(movie => new HomeFeaturedMovieViewModel
                {
                    Id = movie.Id,
                    Title = movie.Title,
                    GenreLabel = GetGenreLabel(movie.Genre),
                    DurationMinutes = movie.DurationMinutes,
                    AgeRating = movie.AgeRating,
                    ReleaseYear = movie.ReleaseDate.Year,
                    Description = movie.Description,
                    Language = movie.Language,
                    ThemeClass = GetThemeClass(movie.Genre)
                })
                .ToListAsync();

            var homeViewModel = new HomeIndexViewModel
            {
                FeaturedMovies = featuredMovies,
                Stats = new List<HomeStatViewModel>
                {
                    new() { Label = "kina", Value = await _dbContext.Cinemas.AsNoTracking().CountAsync() },
                    new() { Label = "filmova", Value = await _dbContext.Movies.AsNoTracking().CountAsync(movie => movie.DeletedAt == null) },
                    new() { Label = "projekcija", Value = await _dbContext.Screenings.AsNoTracking().CountAsync(screening => screening.DeletedAt == null) },
                    new() { Label = "ulaznica u bazi", Value = await _dbContext.Tickets.AsNoTracking().CountAsync() }
                }
            };

            return View(homeViewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private static string GetGenreLabel(MovieGenre genre)
        {
            return genre switch
            {
                MovieGenre.Action => "Akcija",
                MovieGenre.Adventure => "Avantura",
                MovieGenre.Animation => "Animacija",
                MovieGenre.Comedy => "Komedija",
                MovieGenre.Crime => "Krimi",
                MovieGenre.Documentary => "Dokumentarni",
                MovieGenre.Drama => "Drama",
                MovieGenre.Fantasy => "Fantasy",
                MovieGenre.Horror => "Horor",
                MovieGenre.Romance => "Romansa",
                MovieGenre.SciFi => "Znanstvena fantastika",
                MovieGenre.Thriller => "Triler",
                _ => genre.ToString()
            };
        }

        private static string GetThemeClass(MovieGenre genre)
        {
            return genre switch
            {
                MovieGenre.Action => "is-action",
                MovieGenre.Adventure => "is-adventure",
                MovieGenre.Animation => "is-animation",
                MovieGenre.Comedy => "is-comedy",
                MovieGenre.Crime => "is-crime",
                MovieGenre.Documentary => "is-documentary",
                MovieGenre.Drama => "is-drama",
                MovieGenre.Fantasy => "is-fantasy",
                MovieGenre.Horror => "is-horror",
                MovieGenre.Romance => "is-romance",
                MovieGenre.SciFi => "is-scifi",
                MovieGenre.Thriller => "is-thriller",
                _ => "is-default"
            };
        }
    }
}
