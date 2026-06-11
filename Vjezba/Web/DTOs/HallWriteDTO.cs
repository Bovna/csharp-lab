namespace Vjezba.Web.DTOs;

using System.ComponentModel.DataAnnotations;

public sealed class HallWriteDTO
{
    [Required(ErrorMessage = "Naziv dvorane je obavezan.")]
    [StringLength(80, ErrorMessage = "Naziv dvorane ne smije biti duzi od {1} znakova.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Kapacitet je obavezan.")]
    [Range(1, 500, ErrorMessage = "Kapacitet mora biti izmedu {1} i {2}.")]
    public int Capacity { get; set; }

    public bool Supports3D { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Kino je obavezno.")]
    public int CinemaId { get; set; }
}
