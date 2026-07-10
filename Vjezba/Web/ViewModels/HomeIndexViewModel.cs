using Vjezba.Model.Entities;

namespace Vjezba.Web.ViewModels;

public sealed class HomeIndexViewModel
{
    public HomeMovieCardViewModel? FeaturedMovie { get; set; }
    public List<HomeMovieCardViewModel> MoviesNowShowing { get; set; } = new();
    public List<HomeScreeningSummaryViewModel> UpcomingScreenings { get; set; } = new();
    public List<HomeCinemaSummaryViewModel> Cinemas { get; set; } = new();
}

public sealed class HomeMovieCardViewModel
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int DurationMinutes { get; set; }
    public DateTime ReleaseDate { get; set; }
    public MovieGenre Genre { get; set; }
    public string Language { get; set; } = string.Empty;
    public string AgeRating { get; set; } = string.Empty;
    public string? PosterPath { get; set; }
    public HomeScreeningSummaryViewModel? NextScreening { get; set; }
}

public sealed class HomeScreeningSummaryViewModel
{
    public int Id { get; set; }
    public int MovieId { get; set; }
    public int CinemaId { get; set; }
    public string MovieTitle { get; set; } = string.Empty;
    public string CinemaName { get; set; } = string.Empty;
    public string HallName { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public bool Is3D { get; set; }
}

public sealed class HomeCinemaSummaryViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public int HallCount { get; set; }
    public int SeatCount { get; set; }
    public bool Has3D { get; set; }
}
