using KinoKlik.Model.Entities;

namespace KinoKlik.Web.ViewModels;

public class ScreeningDetailsViewModel
{
    public Screening Screening { get; set; } = new();
    public List<Ticket> Tickets { get; set; } = [];
}
