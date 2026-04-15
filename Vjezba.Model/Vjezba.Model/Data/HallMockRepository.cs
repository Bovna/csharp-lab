using Vjezba.Model.Models.Entities;

namespace Vjezba.Model.Data;

public class HallMockRepository
{
    public List<Hall> GetAll()
    {
        return Halls.ToList();
    }

    public Hall? GetById(int id)
    {
        return Halls.FirstOrDefault(h => h.Id == id);
    }

    private static readonly List<Hall> Halls =
    [
        new Hall { Id = 101, Name = "Dvorana A", Capacity = 150, Supports3D = true, Cinema = new Cinema { Id = 1, Name = "CineStar Branimir" } },
        new Hall { Id = 102, Name = "Dvorana B", Capacity = 112, Supports3D = false, Cinema = new Cinema { Id = 1, Name = "CineStar Branimir" } },
        new Hall { Id = 103, Name = "Dvorana C", Capacity = 72, Supports3D = false, Cinema = new Cinema { Id = 1, Name = "CineStar Branimir" } },
        new Hall { Id = 201, Name = "Dvorana A", Capacity = 150, Supports3D = true, Cinema = new Cinema { Id = 2, Name = "Kino Europa" } },
        new Hall { Id = 202, Name = "Dvorana B", Capacity = 112, Supports3D = false, Cinema = new Cinema { Id = 2, Name = "Kino Europa" } },
        new Hall { Id = 203, Name = "Dvorana C", Capacity = 72, Supports3D = false, Cinema = new Cinema { Id = 2, Name = "Kino Europa" } },
        new Hall { Id = 204, Name = "Dvorana D", Capacity = 90, Supports3D = true, Cinema = new Cinema { Id = 2, Name = "Kino Europa" } },
        new Hall { Id = 205, Name = "Dvorana E", Capacity = 56, Supports3D = false, Cinema = new Cinema { Id = 2, Name = "Kino Europa" } },
        new Hall { Id = 301, Name = "Dvorana A", Capacity = 150, Supports3D = true, Cinema = new Cinema { Id = 3, Name = "Arena Cinema" } },
        new Hall { Id = 302, Name = "Dvorana B", Capacity = 112, Supports3D = false, Cinema = new Cinema { Id = 3, Name = "Arena Cinema" } },
        new Hall { Id = 303, Name = "Dvorana C", Capacity = 72, Supports3D = false, Cinema = new Cinema { Id = 3, Name = "Arena Cinema" } },
        new Hall { Id = 304, Name = "Dvorana D", Capacity = 90, Supports3D = true, Cinema = new Cinema { Id = 3, Name = "Arena Cinema" } },
        new Hall { Id = 305, Name = "Dvorana E", Capacity = 56, Supports3D = false, Cinema = new Cinema { Id = 3, Name = "Arena Cinema" } },
        new Hall { Id = 306, Name = "Dvorana F", Capacity = 80, Supports3D = true, Cinema = new Cinema { Id = 3, Name = "Arena Cinema" } },
        new Hall { Id = 401, Name = "Dvorana A", Capacity = 150, Supports3D = true, Cinema = new Cinema { Id = 4, Name = "Marina Cineplex" } },
        new Hall { Id = 402, Name = "Dvorana B", Capacity = 112, Supports3D = false, Cinema = new Cinema { Id = 4, Name = "Marina Cineplex" } },
        new Hall { Id = 403, Name = "Dvorana C", Capacity = 72, Supports3D = false, Cinema = new Cinema { Id = 4, Name = "Marina Cineplex" } },
        new Hall { Id = 501, Name = "Dvorana A", Capacity = 150, Supports3D = true, Cinema = new Cinema { Id = 5, Name = "Forum Cinema" } },
        new Hall { Id = 502, Name = "Dvorana B", Capacity = 112, Supports3D = false, Cinema = new Cinema { Id = 5, Name = "Forum Cinema" } },
        new Hall { Id = 503, Name = "Dvorana C", Capacity = 72, Supports3D = false, Cinema = new Cinema { Id = 5, Name = "Forum Cinema" } },
        new Hall { Id = 504, Name = "Dvorana D", Capacity = 90, Supports3D = true, Cinema = new Cinema { Id = 5, Name = "Forum Cinema" } },
        new Hall { Id = 505, Name = "Dvorana E", Capacity = 56, Supports3D = false, Cinema = new Cinema { Id = 5, Name = "Forum Cinema" } }
    ];
}
