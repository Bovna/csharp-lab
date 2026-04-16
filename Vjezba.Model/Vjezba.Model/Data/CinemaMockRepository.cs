using Vjezba.Model.Models.Entities;

namespace Vjezba.Model.Data;

public class CinemaMockRepository
{
    public List<Cinema> GetAll()
    {
        return Cinemas.ToList();
    }

    public Cinema? GetById(int id)
    {
        return Cinemas.FirstOrDefault(c => c.Id == id);
    }

    private static readonly List<Cinema> Cinemas =
    [
        new Cinema
        {
            Id = 1,
            Name = "CineStar Branimir",
            City = "Zagreb",
            Street = "Branimirova",
            HouseNumber = "29",
            PostalCode = "10000",
            Email = "branimir@cinestar.hr",
            Phone = "+385 1 111 222",
            Halls =
            [
                new Hall { Id = 101, Name = "Dvorana A", Capacity = 150, Supports3D = true, Cinema = new Cinema { Id = 1, Name = "CineStar Branimir" } },
                new Hall { Id = 102, Name = "Dvorana B", Capacity = 112, Supports3D = false, Cinema = new Cinema { Id = 1, Name = "CineStar Branimir" } },
                new Hall { Id = 103, Name = "Dvorana C", Capacity = 96, Supports3D = false, Cinema = new Cinema { Id = 1, Name = "CineStar Branimir" } }
            ]
        },
        new Cinema
        {
            Id = 2,
            Name = "Kino Europa",
            City = "Rijeka",
            Street = "Korzo",
            HouseNumber = "14",
            PostalCode = "51000",
            Email = "info@kinoeuropa.hr",
            Phone = "+385 51 333 444",
            Halls =
            [
                new Hall { Id = 201, Name = "Dvorana A", Capacity = 150, Supports3D = true, Cinema = new Cinema { Id = 2, Name = "Kino Europa" } },
                new Hall { Id = 202, Name = "Dvorana B", Capacity = 112, Supports3D = false, Cinema = new Cinema { Id = 2, Name = "Kino Europa" } },
                new Hall { Id = 203, Name = "Dvorana C", Capacity = 90, Supports3D = false, Cinema = new Cinema { Id = 2, Name = "Kino Europa" } },
                new Hall { Id = 204, Name = "Dvorana D", Capacity = 77, Supports3D = true, Cinema = new Cinema { Id = 2, Name = "Kino Europa" } },
                new Hall { Id = 205, Name = "Dvorana E", Capacity = 60, Supports3D = false, Cinema = new Cinema { Id = 2, Name = "Kino Europa" } }
            ]
        },
        new Cinema
        {
            Id = 3,
            Name = "Arena Cinema",
            City = "Osijek",
            Street = "Sjenjak",
            HouseNumber = "6",
            PostalCode = "31000",
            Email = "kontakt@arenacinema.hr",
            Phone = "+385 31 555 666",
            Halls =
            [
                new Hall { Id = 301, Name = "Dvorana A", Capacity = 150, Supports3D = true, Cinema = new Cinema { Id = 3, Name = "Arena Cinema" } },
                new Hall { Id = 302, Name = "Dvorana B", Capacity = 112, Supports3D = false, Cinema = new Cinema { Id = 3, Name = "Arena Cinema" } },
                new Hall { Id = 303, Name = "Dvorana C", Capacity = 96, Supports3D = false, Cinema = new Cinema { Id = 3, Name = "Arena Cinema" } },
                new Hall { Id = 304, Name = "Dvorana D", Capacity = 90, Supports3D = true, Cinema = new Cinema { Id = 3, Name = "Arena Cinema" } },
                new Hall { Id = 305, Name = "Dvorana E", Capacity = 72, Supports3D = false, Cinema = new Cinema { Id = 3, Name = "Arena Cinema" } },
                new Hall { Id = 306, Name = "Dvorana F", Capacity = 70, Supports3D = true, Cinema = new Cinema { Id = 3, Name = "Arena Cinema" } }
            ]
        },
        new Cinema
        {
            Id = 4,
            Name = "Marina Cineplex",
            City = "Split",
            Street = "Obala",
            HouseNumber = "9",
            PostalCode = "21000",
            Email = "hello@marinacineplex.hr",
            Phone = "+385 21 777 888",
            Halls =
            [
                new Hall { Id = 401, Name = "Dvorana A", Capacity = 150, Supports3D = true, Cinema = new Cinema { Id = 4, Name = "Marina Cineplex" } },
                new Hall { Id = 402, Name = "Dvorana B", Capacity = 150, Supports3D = false, Cinema = new Cinema { Id = 4, Name = "Marina Cineplex" } },
                new Hall { Id = 403, Name = "Dvorana C", Capacity = 96, Supports3D = false, Cinema = new Cinema { Id = 4, Name = "Marina Cineplex" } }
            ]
        },
        new Cinema
        {
            Id = 5,
            Name = "Forum Cinema",
            City = "Zadar",
            Street = "Siroka",
            HouseNumber = "12",
            PostalCode = "23000",
            Email = "info@forumcinema.hr",
            Phone = "+385 23 456 700",
            Halls =
            [
                new Hall { Id = 501, Name = "Dvorana A", Capacity = 150, Supports3D = true, Cinema = new Cinema { Id = 5, Name = "Forum Cinema" } },
                new Hall { Id = 502, Name = "Dvorana B", Capacity = 112, Supports3D = false, Cinema = new Cinema { Id = 5, Name = "Forum Cinema" } },
                new Hall { Id = 503, Name = "Dvorana C", Capacity = 72, Supports3D = false, Cinema = new Cinema { Id = 5, Name = "Forum Cinema" } },
                new Hall { Id = 504, Name = "Dvorana D", Capacity = 90, Supports3D = true, Cinema = new Cinema { Id = 5, Name = "Forum Cinema" } },
                new Hall { Id = 505, Name = "Dvorana E", Capacity = 90, Supports3D = false, Cinema = new Cinema { Id = 5, Name = "Forum Cinema" } }
            ]
        }
    ];
}
