using Vjezba.Model.Models.Entities;

namespace Vjezba.Model.Data;

public class SeatMockRepository
{
    public List<Seat> GetAll()
    {
        return Seats.ToList();
    }

    public Seat? GetById(int id)
    {
        return Seats.FirstOrDefault(s => s.Id == id);
    }

    private static readonly List<HallLayout> HallLayouts =
    [
        new HallLayout(101, "Dvorana A", 1, "CineStar Branimir", 'J', 15),
        new HallLayout(102, "Dvorana B", 1, "CineStar Branimir", 'H', 14),
        new HallLayout(103, "Dvorana C", 1, "CineStar Branimir", 'F', 12),

        new HallLayout(201, "Dvorana A", 2, "Kino Europa", 'J', 15),
        new HallLayout(202, "Dvorana B", 2, "Kino Europa", 'H', 14),
        new HallLayout(203, "Dvorana C", 2, "Kino Europa", 'F', 12),
        new HallLayout(204, "Dvorana D", 2, "Kino Europa", 'I', 10),
        new HallLayout(205, "Dvorana E", 2, "Kino Europa", 'D', 14),

        new HallLayout(301, "Dvorana A", 3, "Arena Cinema", 'J', 15),
        new HallLayout(302, "Dvorana B", 3, "Arena Cinema", 'H', 14),
        new HallLayout(303, "Dvorana C", 3, "Arena Cinema", 'F', 12),
        new HallLayout(304, "Dvorana D", 3, "Arena Cinema", 'I', 10),
        new HallLayout(305, "Dvorana E", 3, "Arena Cinema", 'D', 14),
        new HallLayout(306, "Dvorana F", 3, "Arena Cinema", 'H', 10),

        new HallLayout(401, "Dvorana A", 4, "Marina Cineplex", 'J', 15),
        new HallLayout(402, "Dvorana B", 4, "Marina Cineplex", 'H', 14),
        new HallLayout(403, "Dvorana C", 4, "Marina Cineplex", 'F', 12),

        new HallLayout(501, "Dvorana A", 5, "Forum Cinema", 'J', 15),
        new HallLayout(502, "Dvorana B", 5, "Forum Cinema", 'H', 14),
        new HallLayout(503, "Dvorana C", 5, "Forum Cinema", 'F', 12),
        new HallLayout(504, "Dvorana D", 5, "Forum Cinema", 'I', 10),
        new HallLayout(505, "Dvorana E", 5, "Forum Cinema", 'D', 14)
    ];

    private static readonly List<Seat> Seats = CreateSeats();

    private static List<Seat> CreateSeats()
    {
        var seats = new List<Seat>();

        foreach (var hall in HallLayouts)
        {
            for (var row = 'A'; row <= hall.MaxRow; row++)
            {
                for (var seatNumber = 1; seatNumber <= hall.MaxSeatNumber; seatNumber++)
                {
                    var rowIndex = row - 'A' + 1;
                    var id = hall.Id * 1000 + rowIndex * 100 + seatNumber;

                    var seatType = seatNumber switch
                    {
                        <= 2 => SeatType.Vip,
                        >= 14 => SeatType.Couple,
                        _ => SeatType.Standard
                    };

                    seats.Add(new Seat
                    {
                        Id = id,
                        RowLabel = row.ToString(),
                        SeatNumber = seatNumber,
                        SeatType = seatType,
                        Hall = new Hall
                        {
                            Id = hall.Id,
                            Name = hall.Name,
                            Capacity = GetCapacity(hall),
                            Cinema = new Cinema { Id = hall.CinemaId, Name = hall.CinemaName }
                        }
                    });
                }
            }
        }

        return seats;
    }

    private static int GetCapacity(HallLayout layout)
    {
        return (layout.MaxRow - 'A' + 1) * layout.MaxSeatNumber;
    }

    private sealed record HallLayout(int Id, string Name, int CinemaId, string CinemaName, char MaxRow, int MaxSeatNumber);
}
