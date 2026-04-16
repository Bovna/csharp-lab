using Vjezba.Model.Models.Entities;

namespace Vjezba.Model.Models.ViewModels;

public class ScreeningDetailsViewModel
{
    public Screening Screening { get; set; } = new();
    public List<Ticket> Tickets { get; set; } = [];
}
