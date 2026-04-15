namespace Vjezba.Model.Models.Entities;

public class Cinema
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Street { get; set; } = string.Empty;
    public string HouseNumber { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public List<Hall> Halls { get; set; } = new List<Hall>();
}