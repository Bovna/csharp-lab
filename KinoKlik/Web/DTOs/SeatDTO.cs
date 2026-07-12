namespace KinoKlik.Web.DTOs;

using KinoKlik.Model.Entities;

public sealed class SeatDTO
{
    public int Id { get; set; }
    public string RowLabel { get; set; } = string.Empty;
    public int SeatNumber { get; set; }
    public SeatType SeatType { get; set; }
    public HallDTO Hall { get; set; } = new();
}
