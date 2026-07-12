namespace KinoKlik.Web.DTOs;

public sealed class CustomerDTO
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public int TicketCount { get; set; }
}
