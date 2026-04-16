using Vjezba.Model.Models.Entities;

namespace Vjezba.Model.Models.ViewModels;

public class HallDetailsViewModel
{
    public Hall Hall { get; set; } = new();
    public List<Seat> Seats { get; set; } = [];
    public List<Screening> Screenings { get; set; } = [];
}
