namespace Vjezba.Model.Models.Entities;

public class Customer
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public DateTime RegisteredAt { get; set; }
    public bool IsLoyaltyMember { get; set; }
    public int LoyaltyPoints { get; set; }
}