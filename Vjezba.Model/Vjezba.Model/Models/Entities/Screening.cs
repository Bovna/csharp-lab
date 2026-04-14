namespace Vjezba.Model.Models.Entities;

public class Screening
{
    public int Id { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public bool Is3D { get; set; }

    public Movie? Movie { get; set; }
}