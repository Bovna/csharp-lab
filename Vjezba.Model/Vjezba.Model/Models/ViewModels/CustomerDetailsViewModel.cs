using Vjezba.Model.Models.Entities;

namespace Vjezba.Model.Models.ViewModels;

public class CustomerDetailsViewModel
{
    public Customer Customer { get; set; } = new();
    public List<Ticket> Tickets { get; set; } = [];
}
