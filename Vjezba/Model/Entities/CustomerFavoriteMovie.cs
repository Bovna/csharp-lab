namespace Vjezba.Model.Entities;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class CustomerFavoriteMovie
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("Customer")]
    public int CustomerId { get; set; }
    public virtual Customer Customer { get; set; } = null!;

    [ForeignKey("Movie")]
    public int MovieId { get; set; }
    public virtual Movie Movie { get; set; } = null!;
}