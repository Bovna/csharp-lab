namespace Vjezba.Web.ViewModels;

public class HomeIndexViewModel
{
    public List<HomeFeaturedMovieViewModel> FeaturedMovies { get; set; } = new();
    public List<HomeStatViewModel> Stats { get; set; } = new();
}

public class HomeFeaturedMovieViewModel
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string GenreLabel { get; set; } = string.Empty;
    public int DurationMinutes { get; set; }
    public string AgeRating { get; set; } = string.Empty;
    public int ReleaseYear { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Language { get; set; } = string.Empty;
    public string ThemeClass { get; set; } = string.Empty;
}

public class HomeStatViewModel
{
    public string Label { get; set; } = string.Empty;
    public int Value { get; set; }
}