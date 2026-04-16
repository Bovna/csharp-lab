using Vjezba.Model.Models.Entities;

namespace Vjezba.Model.Data;

public class ScreeningMockRepository
{
    public List<Screening> GetAll()
    {
        return Screenings.ToList();
    }

    public Screening? GetById(int id)
    {
        return Screenings.FirstOrDefault(s => s.Id == id);
    }

    private static readonly List<Screening> Screenings =
    [
        new Screening { Id = 1001, StartTime = new DateTime(2026, 4, 18, 18, 0, 0), EndTime = new DateTime(2026, 4, 18, 20, 35, 0), Is3D = true, Movie = new Movie { Id = 1, Title = "Galactic Run" }, Hall = new Hall { Id = 101, Name = "Dvorana A", Cinema = new Cinema { Id = 1, Name = "CineStar Branimir" } } },
        new Screening { Id = 1004, StartTime = new DateTime(2026, 4, 18, 22, 30, 0), EndTime = new DateTime(2026, 4, 19, 1, 5, 0), Is3D = true, Movie = new Movie { Id = 1, Title = "Galactic Run" }, Hall = new Hall { Id = 101, Name = "Dvorana A", Cinema = new Cinema { Id = 1, Name = "CineStar Branimir" } } },
        new Screening { Id = 1002, StartTime = new DateTime(2026, 4, 18, 21, 0, 0), EndTime = new DateTime(2026, 4, 18, 22, 50, 0), Is3D = false, Movie = new Movie { Id = 2, Title = "Tiha Ulica" }, Hall = new Hall { Id = 102, Name = "Dvorana B", Cinema = new Cinema { Id = 1, Name = "CineStar Branimir" } } },
        new Screening { Id = 1005, StartTime = new DateTime(2026, 4, 19, 19, 20, 0), EndTime = new DateTime(2026, 4, 19, 21, 10, 0), Is3D = false, Movie = new Movie { Id = 2, Title = "Tiha Ulica" }, Hall = new Hall { Id = 102, Name = "Dvorana B", Cinema = new Cinema { Id = 1, Name = "CineStar Branimir" } } },
        new Screening { Id = 1003, StartTime = new DateTime(2026, 4, 19, 16, 30, 0), EndTime = new DateTime(2026, 4, 19, 18, 35, 0), Is3D = false, Movie = new Movie { Id = 5, Title = "Brzina Noci" }, Hall = new Hall { Id = 103, Name = "Dvorana C", Cinema = new Cinema { Id = 1, Name = "CineStar Branimir" } } },
        new Screening { Id = 2001, StartTime = new DateTime(2026, 4, 19, 17, 30, 0), EndTime = new DateTime(2026, 4, 19, 19, 0, 0), Is3D = true, Movie = new Movie { Id = 3, Title = "Mali Izumitelj" }, Hall = new Hall { Id = 201, Name = "Dvorana A", Cinema = new Cinema { Id = 2, Name = "Kino Europa" } } },
        new Screening { Id = 2004, StartTime = new DateTime(2026, 4, 20, 11, 0, 0), EndTime = new DateTime(2026, 4, 20, 12, 30, 0), Is3D = true, Movie = new Movie { Id = 3, Title = "Mali Izumitelj" }, Hall = new Hall { Id = 201, Name = "Dvorana A", Cinema = new Cinema { Id = 2, Name = "Kino Europa" } } },
        new Screening { Id = 2002, StartTime = new DateTime(2026, 4, 19, 20, 0, 0), EndTime = new DateTime(2026, 4, 19, 21, 40, 0), Is3D = false, Movie = new Movie { Id = 6, Title = "Ljeto u Puli" }, Hall = new Hall { Id = 202, Name = "Dvorana B", Cinema = new Cinema { Id = 2, Name = "Kino Europa" } } },
        new Screening { Id = 2003, StartTime = new DateTime(2026, 4, 20, 19, 10, 0), EndTime = new DateTime(2026, 4, 20, 20, 55, 0), Is3D = false, Movie = new Movie { Id = 7, Title = "Sjene Istine" }, Hall = new Hall { Id = 205, Name = "Dvorana E", Cinema = new Cinema { Id = 2, Name = "Kino Europa" } } },
        new Screening { Id = 3001, StartTime = new DateTime(2026, 4, 20, 20, 0, 0), EndTime = new DateTime(2026, 4, 20, 22, 10, 0), Is3D = true, Movie = new Movie { Id = 4, Title = "Planina Straha" }, Hall = new Hall { Id = 301, Name = "Dvorana A", Cinema = new Cinema { Id = 3, Name = "Arena Cinema" } } },
        new Screening { Id = 3004, StartTime = new DateTime(2026, 4, 21, 23, 15, 0), EndTime = new DateTime(2026, 4, 22, 1, 25, 0), Is3D = true, Movie = new Movie { Id = 4, Title = "Planina Straha" }, Hall = new Hall { Id = 301, Name = "Dvorana A", Cinema = new Cinema { Id = 3, Name = "Arena Cinema" } } },
        new Screening { Id = 3002, StartTime = new DateTime(2026, 4, 20, 21, 15, 0), EndTime = new DateTime(2026, 4, 20, 22, 45, 0), Is3D = false, Movie = new Movie { Id = 10, Title = "Ledena Dubina" }, Hall = new Hall { Id = 302, Name = "Dvorana B", Cinema = new Cinema { Id = 3, Name = "Arena Cinema" } } },
        new Screening { Id = 3003, StartTime = new DateTime(2026, 4, 21, 18, 50, 0), EndTime = new DateTime(2026, 4, 21, 20, 55, 0), Is3D = true, Movie = new Movie { Id = 8, Title = "Kraljevstvo Vjetra" }, Hall = new Hall { Id = 306, Name = "Dvorana F", Cinema = new Cinema { Id = 3, Name = "Arena Cinema" } } },
        new Screening { Id = 4001, StartTime = new DateTime(2026, 4, 21, 18, 30, 0), EndTime = new DateTime(2026, 4, 21, 20, 22, 0), Is3D = true, Movie = new Movie { Id = 11, Title = "Zvuk Tisine" }, Hall = new Hall { Id = 401, Name = "Dvorana A", Cinema = new Cinema { Id = 4, Name = "Marina Cineplex" } } },
        new Screening { Id = 4004, StartTime = new DateTime(2026, 4, 22, 16, 15, 0), EndTime = new DateTime(2026, 4, 22, 18, 7, 0), Is3D = true, Movie = new Movie { Id = 11, Title = "Zvuk Tisine" }, Hall = new Hall { Id = 401, Name = "Dvorana A", Cinema = new Cinema { Id = 4, Name = "Marina Cineplex" } } },
        new Screening { Id = 4002, StartTime = new DateTime(2026, 4, 21, 21, 0, 0), EndTime = new DateTime(2026, 4, 21, 22, 58, 0), Is3D = false, Movie = new Movie { Id = 12, Title = "Put Bez Mape" }, Hall = new Hall { Id = 402, Name = "Dvorana B", Cinema = new Cinema { Id = 4, Name = "Marina Cineplex" } } },
        new Screening { Id = 4005, StartTime = new DateTime(2026, 4, 23, 20, 40, 0), EndTime = new DateTime(2026, 4, 23, 22, 38, 0), Is3D = false, Movie = new Movie { Id = 12, Title = "Put Bez Mape" }, Hall = new Hall { Id = 402, Name = "Dvorana B", Cinema = new Cinema { Id = 4, Name = "Marina Cineplex" } } },
        new Screening { Id = 4003, StartTime = new DateTime(2026, 4, 22, 19, 30, 0), EndTime = new DateTime(2026, 4, 22, 21, 10, 0), Is3D = false, Movie = new Movie { Id = 9, Title = "Smijeh do Suza" }, Hall = new Hall { Id = 403, Name = "Dvorana C", Cinema = new Cinema { Id = 4, Name = "Marina Cineplex" } } },
        new Screening { Id = 5001, StartTime = new DateTime(2026, 4, 22, 20, 10, 0), EndTime = new DateTime(2026, 4, 22, 22, 13, 0), Is3D = true, Movie = new Movie { Id = 13, Title = "Nocturna" }, Hall = new Hall { Id = 501, Name = "Dvorana A", Cinema = new Cinema { Id = 5, Name = "Forum Cinema" } } },
        new Screening { Id = 5006, StartTime = new DateTime(2026, 4, 23, 22, 25, 0), EndTime = new DateTime(2026, 4, 24, 0, 28, 0), Is3D = true, Movie = new Movie { Id = 13, Title = "Nocturna" }, Hall = new Hall { Id = 501, Name = "Dvorana A", Cinema = new Cinema { Id = 5, Name = "Forum Cinema" } } },
        new Screening { Id = 5002, StartTime = new DateTime(2026, 4, 23, 19, 15, 0), EndTime = new DateTime(2026, 4, 23, 21, 3, 0), Is3D = false, Movie = new Movie { Id = 14, Title = "Signal 404" }, Hall = new Hall { Id = 502, Name = "Dvorana B", Cinema = new Cinema { Id = 5, Name = "Forum Cinema" } } },
        new Screening { Id = 5007, StartTime = new DateTime(2026, 4, 24, 14, 0, 0), EndTime = new DateTime(2026, 4, 24, 15, 48, 0), Is3D = false, Movie = new Movie { Id = 14, Title = "Signal 404" }, Hall = new Hall { Id = 502, Name = "Dvorana B", Cinema = new Cinema { Id = 5, Name = "Forum Cinema" } } },
        new Screening { Id = 5003, StartTime = new DateTime(2026, 4, 24, 17, 45, 0), EndTime = new DateTime(2026, 4, 24, 19, 21, 0), Is3D = true, Movie = new Movie { Id = 15, Title = "Dnevnik Sjevera" }, Hall = new Hall { Id = 503, Name = "Dvorana C", Cinema = new Cinema { Id = 5, Name = "Forum Cinema" } } },
        new Screening { Id = 5004, StartTime = new DateTime(2026, 4, 24, 20, 15, 0), EndTime = new DateTime(2026, 4, 24, 22, 9, 0), Is3D = false, Movie = new Movie { Id = 16, Title = "Juzni Vjetar" }, Hall = new Hall { Id = 504, Name = "Dvorana D", Cinema = new Cinema { Id = 5, Name = "Forum Cinema" } } },
        new Screening { Id = 5005, StartTime = new DateTime(2026, 4, 25, 18, 0, 0), EndTime = new DateTime(2026, 4, 25, 20, 5, 0), Is3D = false, Movie = new Movie { Id = 5, Title = "Brzina Noci" }, Hall = new Hall { Id = 505, Name = "Dvorana E", Cinema = new Cinema { Id = 5, Name = "Forum Cinema" } } }
    ];
}
