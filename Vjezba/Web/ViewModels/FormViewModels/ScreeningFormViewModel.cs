namespace Vjezba.Web.ViewModels;

using System.ComponentModel.DataAnnotations;

public class ScreeningFormViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Početak projekcije je obavezan.")]
    public DateTime StartTime { get; set; }

    [Required(ErrorMessage = "Završetak projekcije je obavezan.")]
    public DateTime EndTime { get; set; }

    public bool Is3D { get; set; }

    [Required(ErrorMessage = "Film je obavezan.")]
    [Range(1, int.MaxValue, ErrorMessage = "Film je obavezan.")]
    public int? MovieId { get; set; }

    [Required(ErrorMessage = "Dvorana je obavezna.")]
    [Range(1, int.MaxValue, ErrorMessage = "Dvorana je obavezna.")]
    public int? HallId { get; set; }
    public AutocompleteViewModel MovieSelector { get; set; } = new();
    public AutocompleteViewModel HallSelector { get; set; } = new();
}