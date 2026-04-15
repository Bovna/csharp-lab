using Vjezba.Model.Models.Entities;

namespace Vjezba.Model;

public static class Main
{
    public static readonly List<Cinema> Cinemas =
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
                new Hall
                {
                    Id = 101,
                    Name = "Dvorana A",
                    Capacity = 120,
                    Supports3D = true,
                    Seats =
                    [
                        new Seat { Id = 10101, RowLabel = "A", SeatNumber = 1, SeatType = SeatType.Standard },
                        new Seat { Id = 10102, RowLabel = "A", SeatNumber = 2, SeatType = SeatType.Standard },
                        new Seat { Id = 10103, RowLabel = "B", SeatNumber = 1, SeatType = SeatType.Vip },
                        new Seat { Id = 10104, RowLabel = "B", SeatNumber = 2, SeatType = SeatType.Vip }
                    ],
                    Screenings =
                    [
                        new Screening
                        {
                            Id = 1001,
                            StartTime = new DateTime(2026, 4, 18, 18, 0, 0),
                            EndTime = new DateTime(2026, 4, 18, 20, 35, 0),
                            Is3D = true,
                            Movie = new Movie
                            {
                                Id = 1,
                                Title = "Galactic Run",
                                Description = "Sci-fi akcija o bijegu kroz galaksiju.",
                                DurationMinutes = 155,
                                ReleaseDate = new DateTime(2025, 12, 12),
                                Genre = MovieGenre.SciFi,
                                Language = "EN",
                                AgeRating = "12+"
                            }
                        },
                        new Screening
                        {
                            Id = 1002,
                            StartTime = new DateTime(2026, 4, 18, 21, 0, 0),
                            EndTime = new DateTime(2026, 4, 18, 22, 50, 0),
                            Is3D = false,
                            Movie = new Movie
                            {
                                Id = 2,
                                Title = "Tiha Ulica",
                                Description = "Kriminalisticka drama smjestena u Zagrebu.",
                                DurationMinutes = 110,
                                ReleaseDate = new DateTime(2025, 10, 1),
                                Genre = MovieGenre.Crime,
                                Language = "HR",
                                AgeRating = "15+"
                            }
                        }
                    ]
                },
                new Hall
                {
                    Id = 102,
                    Name = "Dvorana B",
                    Capacity = 90,
                    Supports3D = false,
                    Seats =
                    [
                        new Seat { Id = 10201, RowLabel = "A", SeatNumber = 1, SeatType = SeatType.Standard },
                        new Seat { Id = 10202, RowLabel = "A", SeatNumber = 2, SeatType = SeatType.Standard },
                        new Seat { Id = 10203, RowLabel = "C", SeatNumber = 5, SeatType = SeatType.Couple },
                        new Seat { Id = 10204, RowLabel = "C", SeatNumber = 6, SeatType = SeatType.Couple }
                    ],
                    Screenings =
                    [
                        new Screening
                        {
                            Id = 1003,
                            StartTime = new DateTime(2026, 4, 19, 17, 30, 0),
                            EndTime = new DateTime(2026, 4, 19, 19, 0, 0),
                            Is3D = false,
                            Movie = new Movie
                            {
                                Id = 3,
                                Title = "Mali Izumitelj",
                                Description = "Animirana avantura za cijelu obitelj.",
                                DurationMinutes = 90,
                                ReleaseDate = new DateTime(2026, 2, 20),
                                Genre = MovieGenre.Animation,
                                Language = "HR",
                                AgeRating = "U"
                            }
                        }
                    ]
                },
                new Hall
                {
                    Id = 103,
                    Name = "Dvorana C",
                    Capacity = 70,
                    Supports3D = true,
                    Seats =
                    [
                        new Seat { Id = 10301, RowLabel = "D", SeatNumber = 3, SeatType = SeatType.Standard },
                        new Seat { Id = 10302, RowLabel = "D", SeatNumber = 4, SeatType = SeatType.Standard },
                        new Seat { Id = 10303, RowLabel = "E", SeatNumber = 1, SeatType = SeatType.Vip }
                    ],
                    Screenings =
                    [
                        new Screening
                        {
                            Id = 1004,
                            StartTime = new DateTime(2026, 4, 20, 20, 0, 0),
                            EndTime = new DateTime(2026, 4, 20, 22, 10, 0),
                            Is3D = true,
                            Movie = new Movie
                            {
                                Id = 4,
                                Title = "Planina Straha",
                                Description = "Horror triler o ekspediciji koja krene po zlu.",
                                DurationMinutes = 130,
                                ReleaseDate = new DateTime(2025, 9, 15),
                                Genre = MovieGenre.Horror,
                                Language = "EN",
                                AgeRating = "16+"
                            }
                        }
                    ]
                }
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
                new Hall
                {
                    Id = 201,
                    Name = "Dvorana A",
                    Capacity = 140,
                    Supports3D = true,
                    Seats =
                    [
                        new Seat { Id = 20101, RowLabel = "A", SeatNumber = 7, SeatType = SeatType.Standard },
                        new Seat { Id = 20102, RowLabel = "A", SeatNumber = 8, SeatType = SeatType.Standard },
                        new Seat { Id = 20103, RowLabel = "B", SeatNumber = 10, SeatType = SeatType.Vip }
                    ],
                    Screenings =
                    [
                        new Screening
                        {
                            Id = 2001,
                            StartTime = new DateTime(2026, 4, 18, 19, 0, 0),
                            EndTime = new DateTime(2026, 4, 18, 21, 5, 0),
                            Is3D = true,
                            Movie = new Movie
                            {
                                Id = 5,
                                Title = "Brzina Noci",
                                Description = "Akcijski film o ilegalnim utrkama.",
                                DurationMinutes = 125,
                                ReleaseDate = new DateTime(2026, 1, 10),
                                Genre = MovieGenre.Action,
                                Language = "EN",
                                AgeRating = "12+"
                            }
                        }
                    ]
                },
                new Hall
                {
                    Id = 202,
                     Name = "Dvorana B",
                    Capacity = 60,
                    Supports3D = false,
                    Seats =
                    [
                        new Seat { Id = 20201, RowLabel = "A", SeatNumber = 3, SeatType = SeatType.Standard },
                        new Seat { Id = 20202, RowLabel = "A", SeatNumber = 4, SeatType = SeatType.Standard },
                        new Seat { Id = 20203, RowLabel = "B", SeatNumber = 1, SeatType = SeatType.Couple }
                    ],
                    Screenings =
                    [
                        new Screening
                        {
                            Id = 2002,
                            StartTime = new DateTime(2026, 4, 19, 20, 0, 0),
                            EndTime = new DateTime(2026, 4, 19, 21, 40, 0),
                            Is3D = false,
                            Movie = new Movie
                            {
                                Id = 6,
                                Title = "Ljeto u Puli",
                                Description = "Romanticna drama snimljena na obali.",
                                DurationMinutes = 100,
                                ReleaseDate = new DateTime(2025, 6, 25),
                                Genre = MovieGenre.Romance,
                                Language = "HR",
                                AgeRating = "12+"
                            }
                        }
                    ]
                },
                new Hall
                {
                    Id = 203,
                    Name = "Dvorana C",
                    Capacity = 45,
                    Supports3D = false,
                    Seats =
                    [
                        new Seat { Id = 20301, RowLabel = "C", SeatNumber = 2, SeatType = SeatType.Standard },
                        new Seat { Id = 20302, RowLabel = "C", SeatNumber = 3, SeatType = SeatType.Standard },
                        new Seat { Id = 20303, RowLabel = "D", SeatNumber = 1, SeatType = SeatType.Vip }
                    ],
                    Screenings =
                    [
                        new Screening
                        {
                            Id = 2003,
                            StartTime = new DateTime(2026, 4, 20, 18, 15, 0),
                            EndTime = new DateTime(2026, 4, 20, 20, 0, 0),
                            Is3D = false,
                            Movie = new Movie
                            {
                                Id = 7,
                                Title = "Sjene Istine",
                                Description = "Napeti triler o novinarskoj istrazi.",
                                DurationMinutes = 105,
                                ReleaseDate = new DateTime(2025, 11, 30),
                                Genre = MovieGenre.Thriller,
                                Language = "EN",
                                AgeRating = "15+"
                            }
                        }
                    ]
                }
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
                new Hall
                {
                    Id = 301,
                    Name = "Dvorana 1",
                    Capacity = 100,
                    Supports3D = true,
                    Seats =
                    [
                        new Seat { Id = 30101, RowLabel = "A", SeatNumber = 9, SeatType = SeatType.Standard },
                        new Seat { Id = 30102, RowLabel = "A", SeatNumber = 10, SeatType = SeatType.Standard },
                        new Seat { Id = 30103, RowLabel = "B", SeatNumber = 4, SeatType = SeatType.Vip }
                    ],
                    Screenings =
                    [
                        new Screening
                        {
                            Id = 3001,
                            StartTime = new DateTime(2026, 4, 18, 16, 45, 0),
                            EndTime = new DateTime(2026, 4, 18, 18, 50, 0),
                            Is3D = true,
                            Movie = new Movie
                            {
                                Id = 8,
                                Title = "Kraljevstvo Vjetra",
                                Description = "Fantasy pustolovina kroz tri svijeta.",
                                DurationMinutes = 125,
                                ReleaseDate = new DateTime(2026, 3, 8),
                                Genre = MovieGenre.Fantasy,
                                Language = "EN",
                                AgeRating = "10+"
                            }
                        }
                    ]
                },
                new Hall
                {
                    Id = 302,
                    Name = "Dvorana 2",
                    Capacity = 80,
                    Supports3D = false,
                    Seats =
                    [
                        new Seat { Id = 30201, RowLabel = "B", SeatNumber = 7, SeatType = SeatType.Standard },
                        new Seat { Id = 30202, RowLabel = "B", SeatNumber = 8, SeatType = SeatType.Standard },
                        new Seat { Id = 30203, RowLabel = "C", SeatNumber = 4, SeatType = SeatType.Couple }
                    ],
                    Screenings =
                    [
                        new Screening
                        {
                            Id = 3002,
                            StartTime = new DateTime(2026, 4, 19, 19, 30, 0),
                            EndTime = new DateTime(2026, 4, 19, 21, 10, 0),
                            Is3D = false,
                            Movie = new Movie
                            {
                                Id = 9,
                                Title = "Smijeh do Suza",
                                Description = "Komedija o tri prijatelja i jednoj svadbi.",
                                DurationMinutes = 100,
                                ReleaseDate = new DateTime(2025, 8, 18),
                                Genre = MovieGenre.Comedy,
                                Language = "HR",
                                AgeRating = "U"
                            }
                        }
                    ]
                },
                new Hall
                {
                    Id = 303,
                    Name = "Dvorana 3",
                    Capacity = 50,
                    Supports3D = false,
                    Seats =
                    [
                        new Seat { Id = 30301, RowLabel = "D", SeatNumber = 5, SeatType = SeatType.Standard },
                        new Seat { Id = 30302, RowLabel = "D", SeatNumber = 6, SeatType = SeatType.Standard },
                        new Seat { Id = 30303, RowLabel = "E", SeatNumber = 2, SeatType = SeatType.Vip }
                    ],
                    Screenings =
                    [
                        new Screening
                        {
                            Id = 3003,
                            StartTime = new DateTime(2026, 4, 20, 21, 15, 0),
                            EndTime = new DateTime(2026, 4, 20, 22, 45, 0),
                            Is3D = false,
                            Movie = new Movie
                            {
                                Id = 10,
                                Title = "Ledena Dubina",
                                Description = "Dokumentarac o zivotu ispod antarktickog leda.",
                                DurationMinutes = 90,
                                ReleaseDate = new DateTime(2024, 12, 5),
                                Genre = MovieGenre.Documentary,
                                Language = "EN",
                                AgeRating = "U"
                            }
                        }
                    ]
                }
            ]
        }
    ];

    public static readonly List<Customer> Customers =
    [
        new Customer
        {
            Id = 1,
            FirstName = "Marko",
            LastName = "Ivic",
            Email = "marko.ivic@email.hr",
            Phone = "+385 98 100 200",
            RegisteredAt = new DateTime(2025, 5, 3),
            IsLoyaltyMember = true,
            LoyaltyPoints = 180
        },
        new Customer
        {
            Id = 2,
            FirstName = "Ana",
            LastName = "Kovac",
            Email = "ana.kovac@email.hr",
            Phone = "+385 98 300 400",
            RegisteredAt = new DateTime(2025, 8, 12),
            IsLoyaltyMember = false,
            LoyaltyPoints = 0
        },
        new Customer
        {
            Id = 3,
            FirstName = "Ivana",
            LastName = "Barisic",
            Email = "ivana.barisic@email.hr",
            Phone = "+385 98 500 600",
            RegisteredAt = new DateTime(2024, 11, 20),
            IsLoyaltyMember = true,
            LoyaltyPoints = 420
        }
    ];

    public static readonly List<Ticket> Tickets =
    [
        new Ticket
        {
            Id = 1,
            TicketNumber = "ZG-2026-0001",
            PurchasedAt = new DateTime(2026, 4, 14, 10, 25, 0),
            Price = 7.50m,
            Status = TicketStatus.Active,
            Screening = Cinemas[0].Halls[0].Screenings[0],
            Seat = Cinemas[0].Halls[0].Seats[0],
            Customer = Customers[0]
        },
        new Ticket
        {
            Id = 2,
            TicketNumber = "ZG-2026-0002",
            PurchasedAt = new DateTime(2026, 4, 14, 11, 40, 0),
            Price = 9.00m,
            Status = TicketStatus.Used,
            Screening = Cinemas[0].Halls[0].Screenings[1],
            Seat = Cinemas[0].Halls[0].Seats[2],
            Customer = Customers[1]
        },
        new Ticket
        {
            Id = 3,
            TicketNumber = "RI-2026-0101",
            PurchasedAt = new DateTime(2026, 4, 14, 12, 5, 0),
            Price = 6.50m,
            Status = TicketStatus.Active,
            Screening = Cinemas[1].Halls[1].Screenings[0],
            Seat = Cinemas[1].Halls[1].Seats[0],
            Customer = Customers[2]
        },
        new Ticket
        {
            Id = 4,
            TicketNumber = "RI-2026-0102",
            PurchasedAt = new DateTime(2026, 4, 14, 13, 15, 0),
            Price = 8.00m,
            Status = TicketStatus.Cancelled,
            Screening = Cinemas[1].Halls[0].Screenings[0],
            Seat = Cinemas[1].Halls[0].Seats[2],
            Customer = Customers[0]
        },
        new Ticket
        {
            Id = 5,
            TicketNumber = "OS-2026-0201",
            PurchasedAt = new DateTime(2026, 4, 14, 14, 30, 0),
            Price = 7.00m,
            Status = TicketStatus.Active,
            Screening = Cinemas[2].Halls[2].Screenings[0],
            Seat = Cinemas[2].Halls[2].Seats[1],
            Customer = Customers[1]
        },
        new Ticket
        {
            Id = 6,
            TicketNumber = "OS-2026-0202",
            PurchasedAt = new DateTime(2026, 4, 14, 15, 5, 0),
            Price = 10.00m,
            Status = TicketStatus.Used,
            Screening = Cinemas[2].Halls[0].Screenings[0],
            Seat = Cinemas[2].Halls[0].Seats[2],
            Customer = Customers[2]
        }
    ];

    public static void initialOutput()
    {
        Console.WriteLine("=== Detaljni ispis kina, dvorana i projekcija ===");
        foreach (var cinema in Cinemas)
        {
            Console.WriteLine($"Kino: {cinema.Name} ({cinema.City})");
            Console.WriteLine($"Adresa: {cinema.Street} {cinema.HouseNumber}, {cinema.PostalCode}");
            Console.WriteLine($"Dvorana: {cinema.Halls.Count}");

            foreach (var hall in cinema.Halls)
            {
                Console.WriteLine($"  - {hall.Name} | Kapacitet: {hall.Capacity} | 3D: {(hall.Supports3D ? "DA" : "NE")}");

                foreach (var screening in hall.Screenings)
                {
                    var movieTitle = screening.Movie?.Title ?? "Nepoznat film";
                    Console.WriteLine($"      Projekcija: {movieTitle} | {screening.StartTime:dd.MM.yyyy HH:mm} | 3D: {(screening.Is3D ? "DA" : "NE")}");
                }
            }

            Console.WriteLine();
        }

        Console.WriteLine("=== Detaljni ispis kupaca i ulaznica ===");
        foreach (var customer in Customers)
        {
            Console.WriteLine($"Kupac: {customer.FirstName} {customer.LastName} | Loyalty: {(customer.IsLoyaltyMember ? "DA" : "NE")} | Bodovi: {customer.LoyaltyPoints}");
        }

        Console.WriteLine();
        foreach (var ticket in Tickets)
        {
            var movieTitle = ticket.Screening?.Movie?.Title ?? "Nepoznat film";
            var customerName = ticket.Customer is null
                ? "Nepoznat kupac"
                : $"{ticket.Customer.FirstName} {ticket.Customer.LastName}";
            Console.WriteLine($"Ulaznica {ticket.TicketNumber} | Film: {movieTitle} | Kupac: {customerName} | Cijena: {ticket.Price} EUR | Status: {ticket.Status}");
        }

        Console.WriteLine();
        Console.WriteLine("=== Jednostavni LINQ upiti ===");

        var cinemasWith3DHall = Cinemas
            .Where(c => c.Halls.Any(h => h.Supports3D))
            .Select(c => c.Name);

        var maxTicketPrice = 8.00m;
        var ticketsBelowPrice = Tickets
            .Where(t => t.Price < maxTicketPrice)
            .Select(t => $"{t.TicketNumber} ({t.Price} EUR)");

        var longGenreMovies = Cinemas
            .SelectMany(c => c.Halls)
            .SelectMany(h => h.Screenings)
            .Where(s => s.Movie is not null
                && s.Movie.DurationMinutes > 120
                && (s.Movie.Genre == MovieGenre.Action
                    || s.Movie.Genre == MovieGenre.Horror
                    || s.Movie.Genre == MovieGenre.Thriller))
            .Select(s => s.Movie!)
            .GroupBy(m => m.Id)
            .Select(g => g.First())
            .Select(m => $"{m.Title} ({m.DurationMinutes} min, {m.Genre})");

        var screeningsAfter19 = Cinemas
            .SelectMany(c => c.Halls)
            .SelectMany(h => h.Screenings)
            .Where(s => s.StartTime.Hour >= 19)
            .Select(s => $"{s.Movie?.Title} ({s.StartTime:dd.MM HH:mm})");

        var topLoyaltyCustomer = Customers
            .OrderByDescending(c => c.LoyaltyPoints)
            .FirstOrDefault();

        Console.WriteLine("Kina koja imaju barem jednu 3D dvoranu:");
        foreach (var cinemaName in cinemasWith3DHall)
        {
            Console.WriteLine($"- {cinemaName}");
        }
        Console.WriteLine();

        Console.WriteLine($"Ulaznice ispod cijene {maxTicketPrice} EUR:");
        foreach (var ticketInfo in ticketsBelowPrice)
        {
            Console.WriteLine($"- {ticketInfo}");
        }
        Console.WriteLine();

        Console.WriteLine("Filmovi duzi od 120 minuta (akcija, horor ili triler):");
        foreach (var movieInfo in longGenreMovies)
        {
            Console.WriteLine($"- {movieInfo}");
        }
        Console.WriteLine();

        Console.WriteLine("Projekcije koje pocinju nakon 19:00:");
        foreach (var screening in screeningsAfter19)
        {
            Console.WriteLine($"- {screening}");
        }
        Console.WriteLine();

        if (topLoyaltyCustomer != null)
        {
            Console.WriteLine($"Kupac s najvise bodova: {topLoyaltyCustomer.FirstName} {topLoyaltyCustomer.LastName} ({topLoyaltyCustomer.LoyaltyPoints})");
        }
        Console.WriteLine();
    }
}