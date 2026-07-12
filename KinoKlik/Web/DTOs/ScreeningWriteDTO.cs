namespace KinoKlik.Web.DTOs;

using System.ComponentModel.DataAnnotations;

public sealed class ScreeningWriteDTO
{
    [Required(ErrorMessage = "Početak projekcije je obavezan.")]
    public DateTime StartTime { get; set; }

    [Required(ErrorMessage = "Završetak projekcije je obavezan.")]
    public DateTime EndTime { get; set; }

    public bool Is3D { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Film je obavezan.")]
    public int MovieId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Dvorana je obavezna.")]
    public int HallId { get; set; }
}
