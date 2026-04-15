using Vjezba.Model.Models.Entities;

namespace Vjezba.Model.Data;

public class CustomerMockRepository
{
    public List<Customer> GetAll()
    {
        return Customers.ToList();
    }

    public Customer? GetById(int id)
    {
        return Customers.FirstOrDefault(c => c.Id == id);
    }

    private static readonly List<Customer> Customers =
    [
        new Customer
        {
            Id = 1,
            FirstName = "Marko",
            LastName = "Ivic",
            City = "Zagreb",
            Street = "Savska",
            HouseNumber = "15",
            PostalCode = "10000",
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
            City = "Rijeka",
            Street = "Vukovarska",
            HouseNumber = "22",
            PostalCode = "51000",
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
            City = "Osijek",
            Street = "Europska avenija",
            HouseNumber = "7",
            PostalCode = "31000",
            Email = "ivana.barisic@email.hr",
            Phone = "+385 98 500 600",
            RegisteredAt = new DateTime(2024, 11, 20),
            IsLoyaltyMember = true,
            LoyaltyPoints = 420
        },
        new Customer
        {
            Id = 4,
            FirstName = "Petar",
            LastName = "Jovic",
            City = "Split",
            Street = "Poljicka",
            HouseNumber = "4",
            PostalCode = "21000",
            Email = "petar.jovic@email.hr",
            Phone = "+385 98 200 300",
            RegisteredAt = new DateTime(2025, 10, 5),
            IsLoyaltyMember = true,
            LoyaltyPoints = 95
        },
        new Customer
        {
            Id = 5,
            FirstName = "Lucija",
            LastName = "Maric",
            City = "Zadar",
            Street = "Kresimirova obala",
            HouseNumber = "19",
            PostalCode = "23000",
            Email = "lucija.maric@email.hr",
            Phone = "+385 98 700 800",
            RegisteredAt = new DateTime(2026, 1, 9),
            IsLoyaltyMember = false,
            LoyaltyPoints = 0
        },
        new Customer
        {
            Id = 6,
            FirstName = "Karlo",
            LastName = "Peric",
            City = "Varazdin",
            Street = "Kapucinski trg",
            HouseNumber = "8",
            PostalCode = "42000",
            Email = "karlo.peric@email.hr",
            Phone = "+385 98 900 111",
            RegisteredAt = new DateTime(2024, 7, 15),
            IsLoyaltyMember = true,
            LoyaltyPoints = 510
        },
        new Customer
        {
            Id = 7,
            FirstName = "Mia",
            LastName = "Novak",
            City = "Zagreb",
            Street = "Selska",
            HouseNumber = "101",
            PostalCode = "10000",
            Email = "mia.novak@email.hr",
            Phone = "+385 99 111 111",
            RegisteredAt = new DateTime(2026, 2, 14),
            IsLoyaltyMember = false,
            LoyaltyPoints = 0
        },
        new Customer
        {
            Id = 8,
            FirstName = "Dario",
            LastName = "Sokic",
            City = "Zagreb",
            Street = "Trg bana Jelacica",
            HouseNumber = "1",
            PostalCode = "10000",
            Email = "dario.sokic@email.hr",
            Phone = "+385 95 333 222",
            RegisteredAt = new DateTime(2025, 12, 1),
            IsLoyaltyMember = true,
            LoyaltyPoints = 260
        }
    ];
}
