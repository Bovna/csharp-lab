namespace KinoKlik.Web.DTOs;

using KinoKlik.Model.Entities;

public sealed class TicketDTO
{
    public int Id { get; set; }
    public string TicketNumber { get; set; } = string.Empty;
    public DateTime PurchasedAt { get; set; }
    public decimal Price { get; set; }
    public TicketStatus Status { get; set; }
    public CustomerDTO Customer { get; set; } = new();
    public ScreeningDTO Screening { get; set; } = new();
    public MovieDTO Movie { get; set; } = new();
    public HallDTO Hall { get; set; } = new();
    public SeatDTO? Seat { get; set; }
}
