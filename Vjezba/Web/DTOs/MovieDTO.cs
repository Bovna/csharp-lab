namespace Vjezba.Web.DTOs;

using Vjezba.Model.Entities;

public sealed class MovieDTO
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int DurationMinutes { get; set; }
    public DateTime ReleaseDate { get; set; }
    public MovieGenre Genre { get; set; }
    public string Language { get; set; } = string.Empty;
    public string AgeRating { get; set; } = string.Empty;
}