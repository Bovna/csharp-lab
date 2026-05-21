namespace Vjezba.Model.Entities;

using System.ComponentModel.DataAnnotations;

public class Cinema
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Street { get; set; } = string.Empty;
    public string HouseNumber { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public virtual ICollection<Hall> Halls { get; set; } = new HashSet<Hall>();
    public DateTime? DeletedAt { get; set; }
}