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

    private static readonly List<Seat> Seats =
    [
        .. BuildSeatsForHall(101, "Dvorana A", 1, "CineStar Branimir", 'J', 15),
        .. BuildSeatsForHall(102, "Dvorana B", 1, "CineStar Branimir", 'H', 14),
        .. BuildSeatsForHall(103, "Dvorana C", 1, "CineStar Branimir", 'H', 12),

        .. BuildSeatsForHall(201, "Dvorana A", 2, "Kino Europa", 'J', 15),
        .. BuildSeatsForHall(202, "Dvorana B", 2, "Kino Europa", 'H', 14),
        .. BuildSeatsForHall(203, "Dvorana C", 2, "Kino Europa", 'J', 9),
        .. BuildSeatsForHall(204, "Dvorana D", 2, "Kino Europa", 'G', 11),
        .. BuildSeatsForHall(205, "Dvorana E", 2, "Kino Europa", 'F', 10),

        .. BuildSeatsForHall(301, "Dvorana A", 3, "Arena Cinema", 'J', 15),
        .. BuildSeatsForHall(302, "Dvorana B", 3, "Arena Cinema", 'H', 14),
        .. BuildSeatsForHall(303, "Dvorana C", 3, "Arena Cinema", 'H', 12),
        .. BuildSeatsForHall(304, "Dvorana D", 3, "Arena Cinema", 'J', 9),
        .. BuildSeatsForHall(305, "Dvorana E", 3, "Arena Cinema", 'F', 12),
        .. BuildSeatsForHall(306, "Dvorana F", 3, "Arena Cinema", 'G', 10),

        .. BuildSeatsForHall(401, "Dvorana A", 4, "Marina Cineplex", 'J', 15),
        .. BuildSeatsForHall(402, "Dvorana B", 4, "Marina Cineplex", 'J', 15),
        .. BuildSeatsForHall(403, "Dvorana C", 4, "Marina Cineplex", 'H', 12),

        .. BuildSeatsForHall(501, "Dvorana A", 5, "Forum Cinema", 'J', 15),
        .. BuildSeatsForHall(502, "Dvorana B", 5, "Forum Cinema", 'H', 14),
        .. BuildSeatsForHall(503, "Dvorana C", 5, "Forum Cinema", 'F', 12),
        .. BuildSeatsForHall(504, "Dvorana D", 5, "Forum Cinema", 'I', 10),
        .. BuildSeatsForHall(505, "Dvorana E", 5, "Forum Cinema", 'J', 9)
    ];

    private static List<Seat> BuildSeatsForHall(int hallId, string hallName, int cinemaId, string cinemaName, char maxRowLabel, int maxSeatNumber)
    {
        var seats = new List<Seat>();
        var rowCount = maxRowLabel - 'A' + 1;
        var hallCapacity = rowCount * maxSeatNumber;

        for (var rowLabel = 'A'; rowLabel <= maxRowLabel; rowLabel++)
        {
            var rowIndex = rowLabel - 'A' + 1;
            var middleSeat = (maxSeatNumber + 1) / 2;

            for (var seatNumber = 1; seatNumber <= maxSeatNumber; seatNumber++)
            {
                var isVipSeat = maxSeatNumber % 2 == 0
                    ? seatNumber == maxSeatNumber / 2 || seatNumber == maxSeatNumber / 2 + 1
                    : seatNumber >= middleSeat - 1 && seatNumber <= middleSeat + 1;

                var seatType = seatNumber <= 2
                    ? SeatType.Standard
                    : seatNumber >= maxSeatNumber - 1
                        ? SeatType.Couple
                        : isVipSeat
                            ? SeatType.Vip
                        : SeatType.Standard;

                seats.Add(new Seat
                {
                    Id = hallId * 1000 + rowIndex * 100 + seatNumber,
                    RowLabel = rowLabel.ToString(),
                    SeatNumber = seatNumber,
                    SeatType = seatType,
                    Hall = new Hall
                    {
                        Id = hallId,
                        Name = hallName,
                        Capacity = hallCapacity,
                        Cinema = new Cinema { Id = cinemaId, Name = cinemaName }
                    }
                });
            }
        }

        return seats;
    }
}
