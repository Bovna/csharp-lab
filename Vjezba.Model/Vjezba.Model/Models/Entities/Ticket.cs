namespace Vjezba.Model.Models.Entities;

public class Ticket
{
    public int Id { get; set; }
    public string TicketNumber { get; set; } = string.Empty;
    public DateTime PurchasedAt { get; set; }
    public decimal Price { get; set; }
    public TicketStatus Status { get; set; } = TicketStatus.Active;
    public Screening? Screening { get; set; }
    public Seat? Seat { get; set; }
    public Customer? Customer { get; set; }

}