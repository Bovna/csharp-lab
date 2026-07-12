namespace KinoKlik.Web.DTOs;

public sealed class HallDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public bool Supports3D { get; set; }
    public int CinemaId { get; set; }
    public string CinemaName { get; set; } = string.Empty;
}
