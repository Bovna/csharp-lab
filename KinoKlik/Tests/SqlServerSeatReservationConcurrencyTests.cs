using FluentAssertions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using KinoKlik.DAL;
using KinoKlik.Model.Entities;

namespace KinoKlik.Tests;

public sealed class SqlServerSeatReservationConcurrencyTests
{
    [SqlServerFact]
    public async Task FilteredUniqueIndex_AllowsOnlyOneConcurrentActiveSeatReservation()
    {
        var baseConnectionString = Environment.GetEnvironmentVariable("TEST_SQL_CONNECTION_STRING")!;
        var databaseName = $"KinoKlikConcurrency_{Guid.NewGuid():N}";
        var connectionString = new SqlConnectionStringBuilder(baseConnectionString)
        {
            InitialCatalog = databaseName,
            Encrypt = true,
            TrustServerCertificate = true
        }.ConnectionString;
        var options = new DbContextOptionsBuilder<CinemaDbContext>()
            .UseSqlServer(connectionString, sql =>
                sql.MigrationsAssembly(typeof(CinemaDbContext).Assembly.GetName().Name))
            .Options;

        try
        {
            await using (var migrationContext = new CinemaDbContext(options))
            {
                await migrationContext.Database.MigrateAsync();
            }

            var setup = await CreateReservationSetupAsync(options);
            await using var firstContext = new CinemaDbContext(options);
            await using var secondContext = new CinemaDbContext(options);

            firstContext.Tickets.Add(CreateTicket(
                "CONCURRENT-A",
                TicketStatus.Active,
                setup.ScreeningId,
                setup.SeatId,
                setup.FirstCustomerId));
            secondContext.Tickets.Add(CreateTicket(
                "CONCURRENT-B",
                TicketStatus.Used,
                setup.ScreeningId,
                setup.SeatId,
                setup.SecondCustomerId));

            var start = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
            var firstSave = TrySaveAsync(firstContext, start.Task);
            var secondSave = TrySaveAsync(secondContext, start.Task);
            start.SetResult(true);

            var results = await Task.WhenAll(firstSave, secondSave);

            results.Should().ContainSingle(result => result.Succeeded);
            var rejected = results.Single(result => !result.Succeeded);
            rejected.SqlErrorNumber.Should().BeOneOf(2601, 2627);

            await using var verificationContext = new CinemaDbContext(options);
            var occupyingTickets = await verificationContext.Tickets.CountAsync(ticket =>
                ticket.ScreeningId == setup.ScreeningId
                && ticket.SeatId == setup.SeatId
                && ticket.DeletedAt == null
                && (ticket.Status == TicketStatus.Active || ticket.Status == TicketStatus.Used));
            occupyingTickets.Should().Be(1);
        }
        finally
        {
            await using var cleanupContext = new CinemaDbContext(options);
            await cleanupContext.Database.EnsureDeletedAsync();
        }
    }

    private static async Task<ReservationSetup> CreateReservationSetupAsync(
        DbContextOptions<CinemaDbContext> options)
    {
        await using var context = new CinemaDbContext(options);
        var cinema = new Cinema
        {
            Name = "Concurrency Cinema",
            City = "Zagreb",
            Street = "Testna ulica",
            HouseNumber = "1",
            PostalCode = "10000",
            Email = "concurrency-cinema@example.com",
            Phone = "+385 1 555 0199"
        };
        var hall = new Hall
        {
            Name = "Concurrency Hall",
            Capacity = 20,
            Supports3D = false,
            Cinema = cinema
        };
        var movie = new Movie
        {
            Title = "Concurrency Movie",
            Description = "Integration-test movie.",
            DurationMinutes = 90,
            ReleaseDate = DateTime.UtcNow.Date,
            Genre = MovieGenre.Drama,
            Language = "HR",
            AgeRating = "12+"
        };
        var screening = new Screening
        {
            StartTime = DateTime.UtcNow.AddDays(1),
            EndTime = DateTime.UtcNow.AddDays(1).AddMinutes(90),
            Hall = hall,
            Movie = movie
        };
        var seat = new Seat
        {
            RowLabel = "A",
            SeatNumber = 1,
            SeatType = SeatType.Standard,
            Hall = hall
        };
        var firstCustomer = CreateCustomer("first.concurrent@example.com");
        var secondCustomer = CreateCustomer("second.concurrent@example.com");

        context.AddRange(screening, seat, firstCustomer, secondCustomer);
        await context.SaveChangesAsync();

        return new ReservationSetup(
            screening.Id,
            seat.Id,
            firstCustomer.Id,
            secondCustomer.Id);
    }

    private static Customer CreateCustomer(string email)
    {
        return new Customer
        {
            FirstName = "Test",
            LastName = "Customer",
            City = "Zagreb",
            Street = "Testna ulica",
            HouseNumber = "2",
            PostalCode = "10000",
            Email = email,
            Phone = "+385 91 555 0199",
            RegisteredAt = DateTime.UtcNow
        };
    }

    private static Ticket CreateTicket(
        string ticketNumber,
        TicketStatus status,
        int screeningId,
        int seatId,
        int customerId)
    {
        return new Ticket
        {
            TicketNumber = ticketNumber,
            ConfirmationCode = Guid.NewGuid(),
            PurchasedAt = DateTime.UtcNow,
            Price = 9.99m,
            Status = status,
            ScreeningId = screeningId,
            SeatId = seatId,
            CustomerId = customerId
        };
    }

    private static async Task<SaveResult> TrySaveAsync(CinemaDbContext context, Task start)
    {
        await start;

        try
        {
            await context.SaveChangesAsync();
            return new SaveResult(true, null);
        }
        catch (DbUpdateException exception)
            when (exception.GetBaseException() is SqlException sqlException)
        {
            return new SaveResult(false, sqlException.Number);
        }
    }

    private sealed record ReservationSetup(
        int ScreeningId,
        int SeatId,
        int FirstCustomerId,
        int SecondCustomerId);

    private sealed record SaveResult(bool Succeeded, int? SqlErrorNumber);
}

[AttributeUsage(AttributeTargets.Method)]
public sealed class SqlServerFactAttribute : FactAttribute
{
    public SqlServerFactAttribute()
    {
        if (string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("TEST_SQL_CONNECTION_STRING")))
        {
            Skip = "Set TEST_SQL_CONNECTION_STRING to run the SQL Server integration test.";
        }
    }
}
