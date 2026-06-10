namespace Vjezba.Web.DTOs;

using System.ComponentModel.DataAnnotations;
using Vjezba.Model.Entities;

public sealed class MovieWriteDTO
{
    [Required(ErrorMessage = "Naslov filma je obavezan.")]
    [StringLength(120, ErrorMessage = "Naslov filma ne smije biti duži od {1} znakova.")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Opis filma je obavezan.")]
    [StringLength(2000, MinimumLength = 20, ErrorMessage = "Opis filma mora imati između {2} i {1} znakova.")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Trajanje filma je obavezno.")]
    [Range(1, 600, ErrorMessage = "Trajanje filma mora biti između {1} i {2} minuta.")]
    public int DurationMinutes { get; set; }

    [Required(ErrorMessage = "Datum izlaska je obavezan.")]
    [DataType(DataType.Date)]
    public DateTime ReleaseDate { get; set; }

    [Required(ErrorMessage = "Žanr je obavezan.")]
    public MovieGenre Genre { get; set; }

    [Required(ErrorMessage = "Jezik je obavezan.")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Jezik mora imati između {2} i {1} znakova.")]
    public string Language { get; set; } = string.Empty;

    [Required(ErrorMessage = "Dobna oznaka je obavezna.")]
    public string AgeRating { get; set; } = string.Empty;
}