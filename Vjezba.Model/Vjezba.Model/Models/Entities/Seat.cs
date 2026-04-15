namespace Vjezba.Model.Models.Entities;

public class Seat
{
    public int Id { get; set; }
    public string RowLabel { get; set; } = string.Empty;
    public int SeatNumber { get; set; }
    public SeatType SeatType { get; set; }
    public Hall? Hall { get; set; }
}