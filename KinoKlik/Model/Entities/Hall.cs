namespace KinoKlik.Model.Entities;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Hall
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public bool Supports3D { get; set; }

    [ForeignKey("Cinema")]
    public int CinemaId { get; set; }
    public virtual Cinema Cinema { get; set; } = null!;
    public virtual ICollection<Seat> Seats { get; set; } = new HashSet<Seat>();
    public virtual ICollection<Screening> Screenings { get; set; } = new HashSet<Screening>();
    public DateTime? DeletedAt { get; set; }
}