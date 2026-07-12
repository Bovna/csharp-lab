namespace KinoKlik.Web.DTOs;

public sealed class ScreeningDTO
{
    public int Id { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public bool Is3D { get; set; }
    public MovieDTO Movie { get; set; } = new();
    public HallDTO Hall { get; set; } = new();
}
