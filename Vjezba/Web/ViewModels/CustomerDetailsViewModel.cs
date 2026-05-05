using Vjezba.Model.Entities;

namespace Vjezba.Web.ViewModels;

public class CustomerDetailsViewModel
{
    public Customer Customer { get; set; } = new();
    public List<Ticket> Tickets { get; set; } = [];
}
