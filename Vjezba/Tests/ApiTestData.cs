using Microsoft.Extensions.DependencyInjection;
using Vjezba.DAL;
using Vjezba.Model.Entities;

namespace Vjezba.Tests;

internal static class ApiTestData
{
    public static async Task<Cinema> CreateCinemaAsync(
        CustomWebApplicationFactory factory,
        string name = "Test Cinema",
        string city = "Zagreb")
    {
        var cinema = new Cinema
        {
            Name = name,
            City = city,
            Street = "Test Street",
            HouseNumber = "1",
            PostalCode = "10000",
            Email = "cinema@example.com",
            Phone = "+385 1 234 567"
        };

        return await AddAsync(factory, cinema);
    }

    public static async Task<Customer> CreateCustomerAsync(
        CustomWebApplicationFactory factory,
        string firstName = "Test",
        string lastName = "Customer")
    {
        var customer = new Customer
        {
            FirstName = firstName,
            LastName = lastName,
            City = "Zagreb",
            Street = "Customer Street",
            HouseNumber = "2",
            PostalCode = "10000",
            Email = $"{firstName.ToLowerInvariant()}.{lastName.ToLowerInvariant()}@example.com",
            Phone = "+385 91 234 5678",
            RegisteredAt = new DateTime(2026, 1, 1),
            IsLoyaltyMember = true,
            LoyaltyPoints = 10
        };

        return await AddAsync(factory, customer);
    }

    public static async Task<Movie> CreateMovieAsync(
        CustomWebApplicationFactory factory,
        string title = "Test Movie")
    {
        var movie = new Movie
        {
            Title = title,
            Description = "A movie created for API integration testing.",
            DurationMinutes = 120,
            ReleaseDate = new DateTime(2026, 2, 1),
            Genre = MovieGenre.Action,
            Language = "EN",
            AgeRating = "12+"
        };

        return await AddAsync(factory, movie);
    }

    public static async Task<Hall> CreateHallAsync(
        CustomWebApplicationFactory factory,
        string name = "Test Hall",
        int? cinemaId = null,
        bool supports3D = true)
    {
        var cinema = cinemaId.HasValue ? null : await CreateCinemaAsync(factory);
        var hall = new Hall
        {
            Name = name,
            Capacity = 80,
            Supports3D = supports3D,
            CinemaId = cinemaId ?? cinema!.Id
        };

        return await AddAsync(factory, hall);
    }

    public static async Task<Screening> CreateScreeningAsync(
        CustomWebApplicationFactory factory,
        int? movieId = null,
        int? hallId = null,
        DateTime? startTime = null,
        DateTime? endTime = null)
    {
        var movie = movieId.HasValue ? null : await CreateMovieAsync(factory);
        var hall = hallId.HasValue ? null : await CreateHallAsync(factory);
        var screening = new Screening
        {
            StartTime = startTime ?? new DateTime(2026, 3, 1, 18, 0, 0),
            EndTime = endTime ?? new DateTime(2026, 3, 1, 20, 0, 0),
            Is3D = false,
            MovieId = movieId ?? movie!.Id,
            HallId = hallId ?? hall!.Id
        };

        return await AddAsync(factory, screening);
    }

    public static async Task<Seat> CreateSeatAsync(
        CustomWebApplicationFactory factory,
        int? hallId = null,
        string rowLabel = "A",
        int seatNumber = 1)
    {
        var hall = hallId.HasValue ? null : await CreateHallAsync(factory);
        var seat = new Seat
        {
            RowLabel = rowLabel,
            SeatNumber = seatNumber,
            SeatType = SeatType.Standard,
            HallId = hallId ?? hall!.Id
        };

        return await AddAsync(factory, seat);
    }

    public static async Task<Ticket> CreateTicketAsync(
        CustomWebApplicationFactory factory,
        int? screeningId = null,
        int? seatId = null,
        int? customerId = null)
    {
        Screening? screening = null;
        Seat? seat = null;

        if (!screeningId.HasValue)
        {
            var cinema = await CreateCinemaAsync(factory);
            var hall = await CreateHallAsync(factory, cinemaId: cinema.Id);
            var movie = await CreateMovieAsync(factory);
            screening = await CreateScreeningAsync(factory, movie.Id, hall.Id);
            seat = seatId.HasValue ? null : await CreateSeatAsync(factory, hall.Id);
        }

        var customer = customerId.HasValue ? null : await CreateCustomerAsync(factory);
        var ticket = new Ticket
        {
            TicketNumber = $"TICKET-{Guid.NewGuid():N}"[..20],
            PurchasedAt = new DateTime(2026, 3, 1, 12, 0, 0),
            Price = 9.99m,
            Status = TicketStatus.Active,
            ScreeningId = screeningId ?? screening!.Id,
            SeatId = seatId ?? seat?.Id,
            CustomerId = customerId ?? customer!.Id
        };

        return await AddAsync(factory, ticket);
    }

    private static async Task<T> AddAsync<T>(CustomWebApplicationFactory factory, T entity)
        where T : class
    {
        using var scope = factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<CinemaDbContext>();

        dbContext.Set<T>().Add(entity);
        await dbContext.SaveChangesAsync();

        return entity;
    }
}
