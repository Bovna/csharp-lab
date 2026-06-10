namespace Vjezba.Web.DTOs;

public sealed class ScreeningWriteDTO
{
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public bool Is3D { get; set; }
    public int MovieId { get; set; }
    public int HallId { get; set; }
}