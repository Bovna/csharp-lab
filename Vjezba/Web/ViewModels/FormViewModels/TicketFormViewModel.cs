namespace Vjezba.Web.ViewModels;

using System.ComponentModel.DataAnnotations;
using Vjezba.Model.Entities;

public class TicketFormViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Broj ulaznice je obavezan.")]
    [StringLength(40, ErrorMessage = "Broj ulaznice ne smije biti duzi od {1} znakova.")]
    public string TicketNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vrijeme kupnje je obavezno.")]
    [DataType(DataType.DateTime)]
    public DateTime PurchasedAt { get; set; }

    [Required(ErrorMessage = "Cijena je obavezna.")]
    [Range(0.01, 9999, ErrorMessage = "Cijena mora biti izmedu {1} i {2}.")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "Status je obavezan.")]
    public TicketStatus Status { get; set; }

    [Required(ErrorMessage = "Projekcija je obavezna.")]
    [Range(1, int.MaxValue, ErrorMessage = "Projekcija je obavezna.")]
    public int? ScreeningId { get; set; }

    public int? SeatId { get; set; }

    [Required(ErrorMessage = "Kupac je obavezan.")]
    [Range(1, int.MaxValue, ErrorMessage = "Kupac je obavezan.")]
    public int? CustomerId { get; set; }

    public AutocompleteViewModel CustomerSelector { get; set; } = new();
    public AutocompleteViewModel ScreeningSelector { get; set; } = new();
    public AutocompleteViewModel SeatSelector { get; set; } = new();
}
