namespace Vjezba.Model.Entities;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Seat
{
    [Key]
    public int Id { get; set; }
    public string RowLabel { get; set; } = string.Empty;
    public int SeatNumber { get; set; }
    public SeatType SeatType { get; set; }

    [ForeignKey("Hall")]
    public int HallId { get; set; }
    public virtual Hall Hall { get; set; } = null!;
    public DateTime? DeletedAt { get; set; }
}