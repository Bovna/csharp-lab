namespace Vjezba.Model.Models.Entities;

public class Hall
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public bool Supports3D { get; set; }
    public Cinema? Cinema { get; set; }
    public List<Seat> Seats { get; set; } = new List<Seat>();
    public List<Screening> Screenings { get; set; } = new List<Screening>();
}