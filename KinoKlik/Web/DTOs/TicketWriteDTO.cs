namespace KinoKlik.Web.DTOs;

using System.ComponentModel.DataAnnotations;
using KinoKlik.Model.Entities;

public sealed class TicketWriteDTO
{
    [Required(ErrorMessage = "Broj ulaznice je obavezan.")]
    [StringLength(40, ErrorMessage = "Broj ulaznice ne smije biti duzi od {1} znakova.")]
    public string TicketNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vrijeme kupnje je obavezno.")]
    public DateTime PurchasedAt { get; set; }

    [Required(ErrorMessage = "Cijena je obavezna.")]
    [Range(0.01, 9999, ErrorMessage = "Cijena mora biti izmedu {1} i {2}.")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "Status je obavezan.")]
    public TicketStatus Status { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Projekcija je obavezna.")]
    public int ScreeningId { get; set; }

    public int? SeatId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Kupac je obavezan.")]
    public int CustomerId { get; set; }
}
