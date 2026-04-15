using Vjezba.Model.Models.Entities;

namespace Vjezba.Model.Data;

public class TicketMockRepository
{
    public List<Ticket> GetAll()
    {
        return Tickets.ToList();
    }

    public Ticket? GetById(int id)
    {
        return Tickets.FirstOrDefault(t => t.Id == id);
    }

    private static readonly List<Ticket> Tickets =
    [
        new Ticket
        {
            Id = 1,
            TicketNumber = "ZG-2026-0001",
            PurchasedAt = new DateTime(2026, 4, 14, 10, 25, 0),
            Price = 7.50m,
            Status = TicketStatus.Active,
            Screening = new Screening { Id = 1001, Movie = new Movie { Id = 1, Title = "Galactic Run" }, Hall = new Hall { Id = 101, Name = "Dvorana A", Cinema = new Cinema { Id = 1, Name = "CineStar Branimir" } } },
            Seat = new Seat { Id = 101101, RowLabel = "A", SeatNumber = 1, SeatType = SeatType.Vip, Hall = new Hall { Id = 101, Name = "Dvorana A" } },
            Customer = new Customer { Id = 1, FirstName = "Marko", LastName = "Ivic" }
        },
        new Ticket
        {
            Id = 2,
            TicketNumber = "ZG-2026-0002",
            PurchasedAt = new DateTime(2026, 4, 14, 11, 40, 0),
            Price = 9.00m,
            Status = TicketStatus.Used,
            Screening = new Screening { Id = 1002, Movie = new Movie { Id = 2, Title = "Tiha Ulica" }, Hall = new Hall { Id = 102, Name = "Dvorana B", Cinema = new Cinema { Id = 1, Name = "CineStar Branimir" } } },
            Seat = new Seat { Id = 102202, RowLabel = "B", SeatNumber = 2, SeatType = SeatType.Vip, Hall = new Hall { Id = 102, Name = "Dvorana B" } },
            Customer = new Customer { Id = 2, FirstName = "Ana", LastName = "Kovac" }
        },
        new Ticket
        {
            Id = 3,
            TicketNumber = "RI-2026-0101",
            PurchasedAt = new DateTime(2026, 4, 14, 12, 5, 0),
            Price = 6.50m,
            Status = TicketStatus.Active,
            Screening = new Screening { Id = 2002, Movie = new Movie { Id = 6, Title = "Ljeto u Puli" }, Hall = new Hall { Id = 202, Name = "Dvorana B", Cinema = new Cinema { Id = 2, Name = "Kino Europa" } } },
            Seat = new Seat { Id = 202305, RowLabel = "C", SeatNumber = 5, SeatType = SeatType.Standard, Hall = new Hall { Id = 202, Name = "Dvorana B" } },
            Customer = new Customer { Id = 3, FirstName = "Ivana", LastName = "Barisic" }
        },
        new Ticket
        {
            Id = 4,
            TicketNumber = "RI-2026-0102",
            PurchasedAt = new DateTime(2026, 4, 14, 13, 15, 0),
            Price = 8.00m,
            Status = TicketStatus.Cancelled,
            Screening = new Screening { Id = 2001, Movie = new Movie { Id = 3, Title = "Mali Izumitelj" }, Hall = new Hall { Id = 201, Name = "Dvorana A", Cinema = new Cinema { Id = 2, Name = "Kino Europa" } } },
            Seat = new Seat { Id = 201110, RowLabel = "A", SeatNumber = 10, SeatType = SeatType.Standard, Hall = new Hall { Id = 201, Name = "Dvorana A" } },
            Customer = new Customer { Id = 1, FirstName = "Marko", LastName = "Ivic" }
        },
        new Ticket
        {
            Id = 5,
            TicketNumber = "OS-2026-0201",
            PurchasedAt = new DateTime(2026, 4, 14, 14, 30, 0),
            Price = 7.00m,
            Status = TicketStatus.Active,
            Screening = new Screening { Id = 3002, Movie = new Movie { Id = 10, Title = "Ledena Dubina" }, Hall = new Hall { Id = 302, Name = "Dvorana B", Cinema = new Cinema { Id = 3, Name = "Arena Cinema" } } },
            Seat = new Seat { Id = 302608, RowLabel = "F", SeatNumber = 8, SeatType = SeatType.Standard, Hall = new Hall { Id = 302, Name = "Dvorana B" } },
            Customer = new Customer { Id = 2, FirstName = "Ana", LastName = "Kovac" }
        },
        new Ticket
        {
            Id = 6,
            TicketNumber = "OS-2026-0202",
            PurchasedAt = new DateTime(2026, 4, 14, 15, 5, 0),
            Price = 10.00m,
            Status = TicketStatus.Used,
            Screening = new Screening { Id = 3001, Movie = new Movie { Id = 4, Title = "Planina Straha" }, Hall = new Hall { Id = 301, Name = "Dvorana A", Cinema = new Cinema { Id = 3, Name = "Arena Cinema" } } },
            Seat = new Seat { Id = 301214, RowLabel = "B", SeatNumber = 14, SeatType = SeatType.Couple, Hall = new Hall { Id = 301, Name = "Dvorana A" } },
            Customer = new Customer { Id = 3, FirstName = "Ivana", LastName = "Barisic" }
        },
        new Ticket
        {
            Id = 7,
            TicketNumber = "ST-2026-0301",
            PurchasedAt = new DateTime(2026, 4, 15, 9, 35, 0),
            Price = 8.50m,
            Status = TicketStatus.Active,
            Screening = new Screening { Id = 4001, Movie = new Movie { Id = 11, Title = "Zvuk Tisine" }, Hall = new Hall { Id = 401, Name = "Dvorana A", Cinema = new Cinema { Id = 4, Name = "Marina Cineplex" } } },
            Seat = new Seat { Id = 401703, RowLabel = "G", SeatNumber = 3, SeatType = SeatType.Standard, Hall = new Hall { Id = 401, Name = "Dvorana A" } },
            Customer = new Customer { Id = 4, FirstName = "Petar", LastName = "Jovic" }
        },
        new Ticket
        {
            Id = 8,
            TicketNumber = "ST-2026-0302",
            PurchasedAt = new DateTime(2026, 4, 15, 10, 10, 0),
            Price = 11.00m,
            Status = TicketStatus.Active,
            Screening = new Screening { Id = 4002, Movie = new Movie { Id = 12, Title = "Put Bez Mape" }, Hall = new Hall { Id = 402, Name = "Dvorana B", Cinema = new Cinema { Id = 4, Name = "Marina Cineplex" } } },
            Seat = new Seat { Id = 402115, RowLabel = "A", SeatNumber = 15, SeatType = SeatType.Couple, Hall = new Hall { Id = 402, Name = "Dvorana B" } },
            Customer = new Customer { Id = 6, FirstName = "Karlo", LastName = "Peric" }
        },
        new Ticket
        {
            Id = 9,
            TicketNumber = "ST-2026-0303",
            PurchasedAt = new DateTime(2026, 4, 15, 10, 45, 0),
            Price = 9.50m,
            Status = TicketStatus.Cancelled,
            Screening = new Screening { Id = 5001, Movie = new Movie { Id = 13, Title = "Nocturna" }, Hall = new Hall { Id = 501, Name = "Dvorana A", Cinema = new Cinema { Id = 5, Name = "Forum Cinema" } } },
            Seat = new Seat { Id = 501915, RowLabel = "I", SeatNumber = 15, SeatType = SeatType.Couple, Hall = new Hall { Id = 501, Name = "Dvorana A" } },
            Customer = new Customer { Id = 5, FirstName = "Lucija", LastName = "Maric" }
        },
        new Ticket
        {
            Id = 10,
            TicketNumber = "ZG-2026-0401",
            PurchasedAt = new DateTime(2026, 4, 16, 11, 20, 0),
            Price = 12.50m,
            Status = TicketStatus.Active,
            Screening = new Screening { Id = 5002, Movie = new Movie { Id = 14, Title = "Signal 404" }, Hall = new Hall { Id = 502, Name = "Dvorana B", Cinema = new Cinema { Id = 5, Name = "Forum Cinema" } } },
            Seat = new Seat { Id = 502101, RowLabel = "A", SeatNumber = 1, SeatType = SeatType.Vip, Hall = new Hall { Id = 502, Name = "Dvorana B" } },
            Customer = new Customer { Id = 6, FirstName = "Karlo", LastName = "Peric" }
        },
        new Ticket
        {
            Id = 11,
            TicketNumber = "ZG-2026-0402",
            PurchasedAt = new DateTime(2026, 4, 16, 12, 10, 0),
            Price = 7.20m,
            Status = TicketStatus.Active,
            Screening = new Screening { Id = 5003, Movie = new Movie { Id = 15, Title = "Dnevnik Sjevera" }, Hall = new Hall { Id = 503, Name = "Dvorana C", Cinema = new Cinema { Id = 5, Name = "Forum Cinema" } } },
            Seat = new Seat { Id = 503510, RowLabel = "E", SeatNumber = 10, SeatType = SeatType.Standard, Hall = new Hall { Id = 503, Name = "Dvorana C" } },
            Customer = new Customer { Id = 4, FirstName = "Petar", LastName = "Jovic" }
        },
        new Ticket
        {
            Id = 12,
            TicketNumber = "VZ-2026-0501",
            PurchasedAt = new DateTime(2026, 4, 16, 18, 25, 0),
            Price = 8.90m,
            Status = TicketStatus.Active,
            Screening = new Screening { Id = 5004, Movie = new Movie { Id = 16, Title = "Juzni Vjetar" }, Hall = new Hall { Id = 504, Name = "Dvorana D", Cinema = new Cinema { Id = 5, Name = "Forum Cinema" } } },
            Seat = new Seat { Id = 504312, RowLabel = "C", SeatNumber = 12, SeatType = SeatType.Standard, Hall = new Hall { Id = 504, Name = "Dvorana D" } },
            Customer = new Customer { Id = 7, FirstName = "Mia", LastName = "Novak" }
        },
        new Ticket
        {
            Id = 13,
            TicketNumber = "ZD-2026-0502",
            PurchasedAt = new DateTime(2026, 4, 16, 19, 40, 0),
            Price = 10.20m,
            Status = TicketStatus.Used,
            Screening = new Screening { Id = 5001, Movie = new Movie { Id = 13, Title = "Nocturna" }, Hall = new Hall { Id = 501, Name = "Dvorana A", Cinema = new Cinema { Id = 5, Name = "Forum Cinema" } } },
            Seat = new Seat { Id = 501704, RowLabel = "G", SeatNumber = 4, SeatType = SeatType.Standard, Hall = new Hall { Id = 501, Name = "Dvorana A" } },
            Customer = new Customer { Id = 8, FirstName = "Dario", LastName = "Sokic" }
        }
    ];
}
