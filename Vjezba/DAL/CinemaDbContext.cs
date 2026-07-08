using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Vjezba.Model.Entities;

namespace Vjezba.DAL;

public class CinemaDbContext : IdentityDbContext<AppUser>
{
    protected CinemaDbContext()
    {
    }

    public CinemaDbContext(DbContextOptions<CinemaDbContext> options) : base(options)
    {
    }

    public DbSet<Cinema> Cinemas { get; set; } = null!;
    public DbSet<Customer> Customers { get; set; } = null!;
    public DbSet<Hall> Halls { get; set; } = null!;
    public DbSet<Movie> Movies { get; set; } = null!;
    public DbSet<Attachment> Attachments { get; set; } = null!;
    public DbSet<Screening> Screenings { get; set; } = null!;
    public DbSet<Seat> Seats { get; set; } = null!;
    public DbSet<Ticket> Tickets { get; set; } = null!;

    public DbSet<CustomerFavoriteMovie> CustomerFavoriteMovies { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Ticket>()
            .Property(ticket => ticket.Price)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Cinema>().HasData(
            new Cinema
            {
                Id = 1,
                Name = "CineStar Branimir",
                City = "Zagreb",
                Street = "Branimirova",
                HouseNumber = "29",
                PostalCode = "10000",
                Email = "branimir@cinestar.hr",
                Phone = "+385 1 111 222"
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
                Phone = "+385 51 333 444"
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
                Phone = "+385 31 555 666"
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
                Phone = "+385 21 777 888"
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
                Phone = "+385 23 456 700"
            });

        modelBuilder.Entity<Hall>().HasData(
            new Hall { Id = 101, Name = "Dvorana A", Capacity = 66, Supports3D = true, CinemaId = 1 },
            new Hall { Id = 102, Name = "Dvorana B", Capacity = 54, Supports3D = false, CinemaId = 1 },
            new Hall { Id = 103, Name = "Dvorana C", Capacity = 63, Supports3D = false, CinemaId = 1 },

            new Hall { Id = 201, Name = "Dvorana A", Capacity = 66, Supports3D = true, CinemaId = 2 },
            new Hall { Id = 202, Name = "Dvorana B", Capacity = 54, Supports3D = false, CinemaId = 2 },
            new Hall { Id = 203, Name = "Dvorana C", Capacity = 63, Supports3D = false, CinemaId = 2 },
            new Hall { Id = 204, Name = "Dvorana D", Capacity = 77, Supports3D = true, CinemaId = 2 },
            new Hall { Id = 205, Name = "Dvorana E", Capacity = 54, Supports3D = false, CinemaId = 2 },

            new Hall { Id = 301, Name = "Dvorana A", Capacity = 66, Supports3D = true, CinemaId = 3 },
            new Hall { Id = 302, Name = "Dvorana B", Capacity = 66, Supports3D = false, CinemaId = 3 },
            new Hall { Id = 303, Name = "Dvorana C", Capacity = 63, Supports3D = false, CinemaId = 3 },
            new Hall { Id = 304, Name = "Dvorana D", Capacity = 77, Supports3D = true, CinemaId = 3 },
            new Hall { Id = 305, Name = "Dvorana E", Capacity = 54, Supports3D = false, CinemaId = 3 },
            new Hall { Id = 306, Name = "Dvorana F", Capacity = 88, Supports3D = true, CinemaId = 3 },

            new Hall { Id = 401, Name = "Dvorana A", Capacity = 66, Supports3D = true, CinemaId = 4 },
            new Hall { Id = 402, Name = "Dvorana B", Capacity = 88, Supports3D = false, CinemaId = 4 },
            new Hall { Id = 403, Name = "Dvorana C", Capacity = 63, Supports3D = false, CinemaId = 4 },

            new Hall { Id = 501, Name = "Dvorana A", Capacity = 66, Supports3D = true, CinemaId = 5 },
            new Hall { Id = 502, Name = "Dvorana B", Capacity = 54, Supports3D = false, CinemaId = 5 },
            new Hall { Id = 503, Name = "Dvorana C", Capacity = 63, Supports3D = false, CinemaId = 5 },
            new Hall { Id = 504, Name = "Dvorana D", Capacity = 77, Supports3D = true, CinemaId = 5 },
            new Hall { Id = 505, Name = "Dvorana E", Capacity = 77, Supports3D = false, CinemaId = 5 });

        modelBuilder.Entity<Movie>().HasData(
            new Movie { Id = 1, Title = "Galactic Run", Description = "Sci-fi akcija o bijegu kroz galaksiju.", DurationMinutes = 155, ReleaseDate = new DateTime(2025, 12, 12), Genre = MovieGenre.SciFi, Language = "EN", AgeRating = "12+" },
            new Movie { Id = 2, Title = "Tiha Ulica", Description = "Kriminalisticka drama smjestena u Zagrebu.", DurationMinutes = 110, ReleaseDate = new DateTime(2025, 10, 1), Genre = MovieGenre.Crime, Language = "HR", AgeRating = "15+" },
            new Movie { Id = 3, Title = "Mali Izumitelj", Description = "Animirana avantura za cijelu obitelj.", DurationMinutes = 90, ReleaseDate = new DateTime(2026, 2, 20), Genre = MovieGenre.Animation, Language = "HR", AgeRating = "U" },
            new Movie { Id = 4, Title = "Planina Straha", Description = "Horror triler o ekspediciji koja krene po zlu.", DurationMinutes = 130, ReleaseDate = new DateTime(2025, 9, 15), Genre = MovieGenre.Horror, Language = "EN", AgeRating = "16+" },
            new Movie { Id = 5, Title = "Brzina Noci", Description = "Akcijski film o ilegalnim utrkama.", DurationMinutes = 125, ReleaseDate = new DateTime(2026, 1, 10), Genre = MovieGenre.Action, Language = "EN", AgeRating = "12+" },
            new Movie { Id = 6, Title = "Ljeto u Puli", Description = "Romanticna drama snimljena na obali.", DurationMinutes = 100, ReleaseDate = new DateTime(2025, 6, 25), Genre = MovieGenre.Romance, Language = "HR", AgeRating = "12+" },
            new Movie { Id = 7, Title = "Sjene Istine", Description = "Napeti triler o novinarskoj istrazi.", DurationMinutes = 105, ReleaseDate = new DateTime(2025, 11, 30), Genre = MovieGenre.Thriller, Language = "EN", AgeRating = "15+" },
            new Movie { Id = 8, Title = "Kraljevstvo Vjetra", Description = "Fantasy pustolovina kroz tri svijeta.", DurationMinutes = 125, ReleaseDate = new DateTime(2026, 3, 8), Genre = MovieGenre.Fantasy, Language = "EN", AgeRating = "10+" },
            new Movie { Id = 9, Title = "Smijeh do Suza", Description = "Komedija o tri prijatelja i jednoj svadbi.", DurationMinutes = 100, ReleaseDate = new DateTime(2025, 8, 18), Genre = MovieGenre.Comedy, Language = "HR", AgeRating = "U" },
            new Movie { Id = 10, Title = "Ledena Dubina", Description = "Dokumentarac o zivotu ispod antarktickog leda.", DurationMinutes = 90, ReleaseDate = new DateTime(2024, 12, 5), Genre = MovieGenre.Documentary, Language = "EN", AgeRating = "U" },
            new Movie { Id = 11, Title = "Zvuk Tisine", Description = "Drama o glazbenici koja se vraca na pozornicu.", DurationMinutes = 112, ReleaseDate = new DateTime(2026, 4, 5), Genre = MovieGenre.Drama, Language = "HR", AgeRating = "12+" },
            new Movie { Id = 12, Title = "Put Bez Mape", Description = "Avanturisticki road trip kroz Europu.", DurationMinutes = 118, ReleaseDate = new DateTime(2026, 2, 2), Genre = MovieGenre.Adventure, Language = "EN", AgeRating = "10+" },
            new Movie { Id = 13, Title = "Nocturna", Description = "Krimi misterij u nocnom gradu.", DurationMinutes = 123, ReleaseDate = new DateTime(2025, 12, 20), Genre = MovieGenre.Crime, Language = "EN", AgeRating = "15+" },
            new Movie { Id = 14, Title = "Signal 404", Description = "Napeta sci-fi misterija o nestalom signalu.", DurationMinutes = 108, ReleaseDate = new DateTime(2026, 4, 10), Genre = MovieGenre.SciFi, Language = "EN", AgeRating = "12+" },
            new Movie { Id = 15, Title = "Dnevnik Sjevera", Description = "Dokumentarni film o arktickim ekspedicijama.", DurationMinutes = 96, ReleaseDate = new DateTime(2026, 1, 22), Genre = MovieGenre.Documentary, Language = "HR", AgeRating = "U" },
            new Movie { Id = 16, Title = "Juzni Vjetar", Description = "Drama o povratku kuci nakon dugog putovanja.", DurationMinutes = 114, ReleaseDate = new DateTime(2026, 3, 2), Genre = MovieGenre.Drama, Language = "HR", AgeRating = "12+" });

        modelBuilder.Entity<Screening>().HasData(
            new Screening { Id = 1001, StartTime = new DateTime(2026, 9, 10, 18, 0, 0), EndTime = new DateTime(2026, 9, 10, 20, 35, 0), Is3D = true, MovieId = 1, HallId = 101 },
            new Screening { Id = 1004, StartTime = new DateTime(2026, 9, 10, 22, 30, 0), EndTime = new DateTime(2026, 9, 11, 1, 5, 0), Is3D = true, MovieId = 1, HallId = 101 },
            new Screening { Id = 1002, StartTime = new DateTime(2026, 9, 10, 21, 0, 0), EndTime = new DateTime(2026, 9, 10, 22, 50, 0), Is3D = false, MovieId = 2, HallId = 102 },
            new Screening { Id = 1005, StartTime = new DateTime(2026, 9, 11, 19, 20, 0), EndTime = new DateTime(2026, 9, 11, 21, 10, 0), Is3D = false, MovieId = 2, HallId = 102 },
            new Screening { Id = 1003, StartTime = new DateTime(2026, 9, 11, 16, 30, 0), EndTime = new DateTime(2026, 9, 11, 18, 35, 0), Is3D = false, MovieId = 5, HallId = 103 },

            new Screening { Id = 2001, StartTime = new DateTime(2026, 9, 11, 17, 30, 0), EndTime = new DateTime(2026, 9, 11, 19, 0, 0), Is3D = true, MovieId = 3, HallId = 201 },
            new Screening { Id = 2004, StartTime = new DateTime(2026, 9, 12, 11, 0, 0), EndTime = new DateTime(2026, 9, 12, 12, 30, 0), Is3D = true, MovieId = 3, HallId = 201 },
            new Screening { Id = 2002, StartTime = new DateTime(2026, 9, 11, 20, 0, 0), EndTime = new DateTime(2026, 9, 11, 21, 40, 0), Is3D = false, MovieId = 6, HallId = 202 },
            new Screening { Id = 2003, StartTime = new DateTime(2026, 9, 12, 19, 10, 0), EndTime = new DateTime(2026, 9, 12, 20, 55, 0), Is3D = false, MovieId = 7, HallId = 205 },

            new Screening { Id = 3001, StartTime = new DateTime(2026, 9, 12, 20, 0, 0), EndTime = new DateTime(2026, 9, 12, 22, 10, 0), Is3D = true, MovieId = 4, HallId = 301 },
            new Screening { Id = 3004, StartTime = new DateTime(2026, 9, 13, 23, 15, 0), EndTime = new DateTime(2026, 9, 14, 1, 25, 0), Is3D = true, MovieId = 4, HallId = 301 },
            new Screening { Id = 3002, StartTime = new DateTime(2026, 9, 12, 21, 15, 0), EndTime = new DateTime(2026, 9, 12, 22, 45, 0), Is3D = false, MovieId = 10, HallId = 302 },
            new Screening { Id = 3003, StartTime = new DateTime(2026, 9, 13, 18, 50, 0), EndTime = new DateTime(2026, 9, 13, 20, 55, 0), Is3D = true, MovieId = 8, HallId = 306 },

            new Screening { Id = 4001, StartTime = new DateTime(2026, 9, 13, 18, 30, 0), EndTime = new DateTime(2026, 9, 13, 20, 22, 0), Is3D = true, MovieId = 11, HallId = 401 },
            new Screening { Id = 4004, StartTime = new DateTime(2026, 9, 14, 16, 15, 0), EndTime = new DateTime(2026, 9, 14, 18, 7, 0), Is3D = true, MovieId = 11, HallId = 401 },
            new Screening { Id = 4002, StartTime = new DateTime(2026, 9, 13, 21, 0, 0), EndTime = new DateTime(2026, 9, 13, 22, 58, 0), Is3D = false, MovieId = 12, HallId = 402 },
            new Screening { Id = 4005, StartTime = new DateTime(2026, 9, 15, 20, 40, 0), EndTime = new DateTime(2026, 9, 15, 22, 38, 0), Is3D = false, MovieId = 12, HallId = 402 },
            new Screening { Id = 4003, StartTime = new DateTime(2026, 9, 14, 19, 30, 0), EndTime = new DateTime(2026, 9, 14, 21, 10, 0), Is3D = false, MovieId = 9, HallId = 403 },

            new Screening { Id = 5001, StartTime = new DateTime(2026, 9, 14, 20, 10, 0), EndTime = new DateTime(2026, 9, 14, 22, 13, 0), Is3D = true, MovieId = 13, HallId = 501 },
            new Screening { Id = 5006, StartTime = new DateTime(2026, 9, 15, 22, 25, 0), EndTime = new DateTime(2026, 9, 16, 0, 28, 0), Is3D = true, MovieId = 13, HallId = 501 },
            new Screening { Id = 5002, StartTime = new DateTime(2026, 9, 15, 19, 15, 0), EndTime = new DateTime(2026, 9, 15, 21, 3, 0), Is3D = false, MovieId = 14, HallId = 502 },
            new Screening { Id = 5007, StartTime = new DateTime(2026, 9, 16, 14, 0, 0), EndTime = new DateTime(2026, 9, 16, 15, 48, 0), Is3D = false, MovieId = 14, HallId = 502 },
            new Screening { Id = 5003, StartTime = new DateTime(2026, 9, 16, 17, 45, 0), EndTime = new DateTime(2026, 9, 16, 19, 21, 0), Is3D = true, MovieId = 15, HallId = 503 },
            new Screening { Id = 5004, StartTime = new DateTime(2026, 9, 16, 20, 15, 0), EndTime = new DateTime(2026, 9, 16, 22, 9, 0), Is3D = false, MovieId = 16, HallId = 504 },
            new Screening { Id = 5005, StartTime = new DateTime(2026, 9, 17, 18, 0, 0), EndTime = new DateTime(2026, 9, 17, 20, 5, 0), Is3D = false, MovieId = 5, HallId = 505 });

        modelBuilder.Entity<Customer>().HasData(
            new Customer { Id = 1, FirstName = "Marko", LastName = "Ivic", City = "Zagreb", Street = "Savska", HouseNumber = "15", PostalCode = "10000", Email = "marko.ivic@email.hr", Phone = "+385 98 100 200", RegisteredAt = new DateTime(2025, 5, 3), IsLoyaltyMember = true, LoyaltyPoints = 180 },
            new Customer { Id = 2, FirstName = "Ana", LastName = "Kovac", City = "Rijeka", Street = "Vukovarska", HouseNumber = "22", PostalCode = "51000", Email = "ana.kovac@email.hr", Phone = "+385 98 300 400", RegisteredAt = new DateTime(2025, 8, 12), IsLoyaltyMember = false, LoyaltyPoints = 0 },
            new Customer { Id = 3, FirstName = "Ivana", LastName = "Barisic", City = "Osijek", Street = "Europska avenija", HouseNumber = "7", PostalCode = "31000", Email = "ivana.barisic@email.hr", Phone = "+385 98 500 600", RegisteredAt = new DateTime(2024, 11, 20), IsLoyaltyMember = true, LoyaltyPoints = 420 },
            new Customer { Id = 4, FirstName = "Petar", LastName = "Jovic", City = "Split", Street = "Poljicka", HouseNumber = "4", PostalCode = "21000", Email = "petar.jovic@email.hr", Phone = "+385 98 200 300", RegisteredAt = new DateTime(2025, 10, 5), IsLoyaltyMember = true, LoyaltyPoints = 95 },
            new Customer { Id = 5, FirstName = "Lucija", LastName = "Maric", City = "Zadar", Street = "Kresimirova obala", HouseNumber = "19", PostalCode = "23000", Email = "lucija.maric@email.hr", Phone = "+385 98 700 800", RegisteredAt = new DateTime(2026, 1, 9), IsLoyaltyMember = false, LoyaltyPoints = 0 },
            new Customer { Id = 6, FirstName = "Karlo", LastName = "Peric", City = "Varazdin", Street = "Kapucinski trg", HouseNumber = "8", PostalCode = "42000", Email = "karlo.peric@email.hr", Phone = "+385 98 900 111", RegisteredAt = new DateTime(2024, 7, 15), IsLoyaltyMember = true, LoyaltyPoints = 510 },
            new Customer { Id = 7, FirstName = "Mia", LastName = "Novak", City = "Zagreb", Street = "Selska", HouseNumber = "101", PostalCode = "10000", Email = "mia.novak@email.hr", Phone = "+385 99 111 111", RegisteredAt = new DateTime(2026, 2, 14), IsLoyaltyMember = false, LoyaltyPoints = 0 },
            new Customer { Id = 8, FirstName = "Dario", LastName = "Sokic", City = "Zagreb", Street = "Trg bana Jelacica", HouseNumber = "1", PostalCode = "10000", Email = "dario.sokic@email.hr", Phone = "+385 95 333 222", RegisteredAt = new DateTime(2025, 12, 1), IsLoyaltyMember = true, LoyaltyPoints = 260 });

        static IEnumerable<Seat> BuildSeatsForHall(
            int hallId,
            char lastRowLabel,
            int seatCount)
        {
            var vipSeatStart = Math.Max(3, (seatCount + 1) / 2 - 1);
            var vipSeatEnd = Math.Min(seatCount - 2, vipSeatStart + 2);

            for (var rowLabel = 'A'; rowLabel <= lastRowLabel; rowLabel++)
            {
                var rowIndex = rowLabel - 'A' + 1;

                for (var seatNumber = 1; seatNumber <= seatCount; seatNumber++)
                {
                    var isCoupleSeat = rowLabel == lastRowLabel;
                    var isVipSeat = seatNumber >= vipSeatStart && seatNumber <= vipSeatEnd;

                    var seatType = isCoupleSeat
                        ? SeatType.Couple
                        : isVipSeat
                            ? SeatType.Vip
                            : SeatType.Standard;

                    yield return new Seat
                    {
                        Id = hallId * 1000 + rowIndex * 100 + seatNumber,
                        RowLabel = rowLabel.ToString(),
                        SeatNumber = seatNumber,
                        SeatType = seatType,
                        HallId = hallId
                    };
                }
            }
        }

        var seats = new List<Seat>();
        var hallLayouts = new (int HallId, char LastRowLabel, int SeatCount)[]
        {
            (101, 'F', 11),
            (102, 'F', 9),
            (103, 'G', 9),
            (201, 'F', 11),
            (202, 'F', 9),
            (203, 'G', 9),
            (204, 'G', 11),
            (205, 'F', 9),
            (301, 'F', 11),
            (302, 'F', 11),
            (303, 'G', 9),
            (304, 'G', 11),
            (305, 'F', 9),
            (306, 'H', 11),
            (401, 'F', 11),
            (402, 'H', 11),
            (403, 'G', 9),
            (501, 'F', 11),
            (502, 'F', 9),
            (503, 'G', 9),
            (504, 'G', 11),
            (505, 'G', 11)
        };

        foreach (var (hallId, lastRowLabel, seatCount) in hallLayouts)
        {
            seats.AddRange(BuildSeatsForHall(hallId, lastRowLabel, seatCount));
        }

        modelBuilder.Entity<Seat>().HasData(seats);

        modelBuilder.Entity<Ticket>().HasData(



            new Ticket { Id = 1, TicketNumber = "ZG-2026-0001", PurchasedAt = new DateTime(2026, 4, 14, 10, 25, 0), Price = 7.50m, Status = TicketStatus.Active, ScreeningId = 1001, SeatId = 101101, CustomerId = 1 },
            new Ticket { Id = 2, TicketNumber = "ZG-2026-0002", PurchasedAt = new DateTime(2026, 4, 14, 11, 40, 0), Price = 9.00m, Status = TicketStatus.Used, ScreeningId = 1002, SeatId = 102202, CustomerId = 2 },
            new Ticket { Id = 3, TicketNumber = "RI-2026-0101", PurchasedAt = new DateTime(2026, 4, 14, 12, 5, 0), Price = 6.50m, Status = TicketStatus.Active, ScreeningId = 2002, SeatId = 202305, CustomerId = 3 },
            new Ticket { Id = 4, TicketNumber = "RI-2026-0102", PurchasedAt = new DateTime(2026, 4, 14, 13, 15, 0), Price = 8.00m, Status = TicketStatus.Cancelled, ScreeningId = 2001, SeatId = 201104, CustomerId = 1 },
            new Ticket { Id = 5, TicketNumber = "OS-2026-0201", PurchasedAt = new DateTime(2026, 4, 14, 14, 30, 0), Price = 7.00m, Status = TicketStatus.Active, ScreeningId = 3002, SeatId = 302408, CustomerId = 2 },
            new Ticket { Id = 6, TicketNumber = "OS-2026-0202", PurchasedAt = new DateTime(2026, 4, 14, 15, 5, 0), Price = 10.00m, Status = TicketStatus.Used, ScreeningId = 3001, SeatId = 301206, CustomerId = 3 },
            new Ticket { Id = 7, TicketNumber = "ST-2026-0301", PurchasedAt = new DateTime(2026, 4, 15, 9, 35, 0), Price = 8.50m, Status = TicketStatus.Active, ScreeningId = 4001, SeatId = 401303, CustomerId = 4 },
            new Ticket { Id = 8, TicketNumber = "ST-2026-0302", PurchasedAt = new DateTime(2026, 4, 15, 10, 10, 0), Price = 11.00m, Status = TicketStatus.Active, ScreeningId = 4002, SeatId = 402405, CustomerId = 6 },
            new Ticket { Id = 9, TicketNumber = "ZD-2026-0501", PurchasedAt = new DateTime(2026, 4, 15, 10, 45, 0), Price = 9.50m, Status = TicketStatus.Cancelled, ScreeningId = 5001, SeatId = 501207, CustomerId = 5 },
            new Ticket { Id = 10, TicketNumber = "ZD-2026-0502", PurchasedAt = new DateTime(2026, 4, 16, 11, 20, 0), Price = 12.50m, Status = TicketStatus.Active, ScreeningId = 5002, SeatId = 502101, CustomerId = 6 },
            new Ticket { Id = 11, TicketNumber = "ZD-2026-0503", PurchasedAt = new DateTime(2026, 4, 16, 12, 10, 0), Price = 7.20m, Status = TicketStatus.Active, ScreeningId = 5003, SeatId = 503403, CustomerId = 4 },
            new Ticket { Id = 12, TicketNumber = "ZD-2026-0504", PurchasedAt = new DateTime(2026, 4, 16, 18, 25, 0), Price = 8.90m, Status = TicketStatus.Active, ScreeningId = 5004, SeatId = 504108, CustomerId = 7 },
            new Ticket { Id = 13, TicketNumber = "ZD-2026-0505", PurchasedAt = new DateTime(2026, 4, 16, 19, 40, 0), Price = 10.20m, Status = TicketStatus.Used, ScreeningId = 5001, SeatId = 501302, CustomerId = 8 },
            new Ticket { Id = 14, TicketNumber = "ZG-2026-0003", PurchasedAt = new DateTime(2026, 4, 16, 20, 5, 0), Price = 7.50m, Status = TicketStatus.Active, ScreeningId = 1001, SeatId = 101305, CustomerId = 5 },
            new Ticket { Id = 15, TicketNumber = "ZG-2026-0004", PurchasedAt = new DateTime(2026, 4, 16, 20, 20, 0), Price = 7.50m, Status = TicketStatus.Active, ScreeningId = 1004, SeatId = 101405, CustomerId = 7 },
            new Ticket { Id = 16, TicketNumber = "RI-2026-0103", PurchasedAt = new DateTime(2026, 4, 16, 20, 35, 0), Price = 6.50m, Status = TicketStatus.Active, ScreeningId = 2002, SeatId = 202404, CustomerId = 4 },
            new Ticket { Id = 17, TicketNumber = "OS-2026-0203", PurchasedAt = new DateTime(2026, 4, 16, 20, 45, 0), Price = 7.00m, Status = TicketStatus.Active, ScreeningId = 3002, SeatId = 302205, CustomerId = 1 },
            new Ticket { Id = 18, TicketNumber = "ST-2026-0303", PurchasedAt = new DateTime(2026, 4, 16, 21, 0, 0), Price = 8.50m, Status = TicketStatus.Active, ScreeningId = 4001, SeatId = 401207, CustomerId = 2 },
            new Ticket { Id = 19, TicketNumber = "ZD-2026-0506", PurchasedAt = new DateTime(2026, 4, 16, 21, 15, 0), Price = 9.50m, Status = TicketStatus.Active, ScreeningId = 5001, SeatId = 501505, CustomerId = 3 },
            new Ticket { Id = 20, TicketNumber = "ZD-2026-0507", PurchasedAt = new DateTime(2026, 4, 16, 21, 25, 0), Price = 9.50m, Status = TicketStatus.Active, ScreeningId = 5004, SeatId = 504307, CustomerId = 6 });

        modelBuilder.Entity<CustomerFavoriteMovie>().HasData(
new CustomerFavoriteMovie { Id = 1, CustomerId = 1, MovieId = 1 },
new CustomerFavoriteMovie { Id = 2, CustomerId = 1, MovieId = 5 },
new CustomerFavoriteMovie { Id = 3, CustomerId = 1, MovieId = 8 },
new CustomerFavoriteMovie { Id = 4, CustomerId = 2, MovieId = 2 },
new CustomerFavoriteMovie { Id = 5, CustomerId = 3, MovieId = 4 },
new CustomerFavoriteMovie { Id = 6, CustomerId = 3, MovieId = 10 },
new CustomerFavoriteMovie { Id = 7, CustomerId = 4, MovieId = 6 },
new CustomerFavoriteMovie { Id = 8, CustomerId = 6, MovieId = 1 });
    }

}
