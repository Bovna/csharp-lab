using Vjezba.Model.Entities;

namespace Vjezba.Web.ViewModels;

public class ScreeningDetailsViewModel
{
    public Screening Screening { get; set; } = new();
    public List<Ticket> Tickets { get; set; } = [];
}
