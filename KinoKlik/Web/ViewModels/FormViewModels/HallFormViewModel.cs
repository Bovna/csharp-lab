namespace KinoKlik.Web.ViewModels;

using System.ComponentModel.DataAnnotations;

public class HallFormViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Naziv dvorane je obavezan.")]
    [StringLength(80, ErrorMessage = "Naziv dvorane ne smije biti duzi od {1} znakova.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Kapacitet je obavezan.")]
    [Range(1, 500, ErrorMessage = "Kapacitet mora biti veći od ili jednak 1.")]
    public int Capacity { get; set; }

    public bool Supports3D { get; set; }

    [Required(ErrorMessage = "Kino je obavezno.")]
    [Range(1, int.MaxValue, ErrorMessage = "Kino je obavezno.")]
    public int? CinemaId { get; set; }

    public AutocompleteViewModel CinemaSelector { get; set; } = new();
}
