namespace KinoKlik.Model.Entities;

using System.ComponentModel.DataAnnotations;

public class Movie
{
    [Key]
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int DurationMinutes { get; set; }
    public DateTime ReleaseDate { get; set; }
    public MovieGenre Genre { get; set; }
    public string Language { get; set; } = string.Empty;
    public string AgeRating { get; set; } = string.Empty;
    public virtual ICollection<Screening> Screenings { get; set; } = new HashSet<Screening>();
    public virtual ICollection<Attachment> Attachments { get; set; } = new HashSet<Attachment>();

    public virtual ICollection<CustomerFavoriteMovie> FavoritedBy { get; set; } = new HashSet<CustomerFavoriteMovie>();

    public DateTime? DeletedAt { get; set; }
}
