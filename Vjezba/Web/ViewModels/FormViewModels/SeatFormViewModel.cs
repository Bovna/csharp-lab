namespace Vjezba.Web.ViewModels;

using System.ComponentModel.DataAnnotations;
using Vjezba.Model.Entities;

public class SeatFormViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Oznaka reda je obavezna.")]
    [StringLength(5, MinimumLength = 1, ErrorMessage = "Oznaka reda mora imati izmedu {2} i {1} znakova.")]
    public string RowLabel { get; set; } = string.Empty;

    [Required(ErrorMessage = "Broj sjedala je obavezan.")]
    [Range(1, 500, ErrorMessage = "Broj sjedala mora biti izmedu {1} i {2}.")]
    public int SeatNumber { get; set; }

    [Required(ErrorMessage = "Tip sjedala je obavezan.")]
    public SeatType SeatType { get; set; }

    [Required(ErrorMessage = "Dvorana je obavezna.")]
    [Range(1, int.MaxValue, ErrorMessage = "Dvorana je obavezna.")]
    public int? HallId { get; set; }

    public AutocompleteViewModel HallSelector { get; set; } = new();
}
