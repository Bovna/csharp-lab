using KinoKlik.Model.Entities;

namespace KinoKlik.Web.ViewModels;

public class HallDetailsViewModel
{
    public Hall Hall { get; set; } = new();
    public List<Seat> Seats { get; set; } = [];
    public List<Screening> Screenings { get; set; } = [];
}
