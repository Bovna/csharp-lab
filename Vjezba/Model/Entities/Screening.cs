namespace Vjezba.Model.Entities;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Screening
{
    [Key]
    public int Id { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public bool Is3D { get; set; }

    [ForeignKey("Movie")]
    public int MovieId { get; set; }
    public virtual Movie Movie { get; set; } = null!;

    [ForeignKey("Hall")]
    public int HallId { get; set; }
    public virtual Hall Hall { get; set; } = null!;
    public virtual ICollection<Ticket> Tickets { get; set; } = new HashSet<Ticket>();
}