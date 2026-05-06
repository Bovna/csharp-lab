namespace Vjezba.Model.Entities;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Ticket
{
    [Key]
    public int Id { get; set; }
    public string TicketNumber { get; set; } = string.Empty;
    public DateTime PurchasedAt { get; set; }
    public decimal Price { get; set; }
    public TicketStatus Status { get; set; } = TicketStatus.Active;

    [ForeignKey("Screening")]
    public int ScreeningId { get; set; }
    public virtual Screening Screening { get; set; } = null!;

    [ForeignKey("Seat")]
    public int? SeatId { get; set; }
    public virtual Seat? Seat { get; set; }

    [ForeignKey("Customer")]
    public int CustomerId { get; set; }
    public virtual Customer Customer { get; set; } = null!;
}