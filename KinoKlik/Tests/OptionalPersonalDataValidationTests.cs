using System.ComponentModel.DataAnnotations;
using FluentAssertions;
using KinoKlik.Web.Areas.Identity.Pages.Account;
using KinoKlik.Web.ViewModels;

namespace KinoKlik.Tests;

public sealed class OptionalPersonalDataValidationTests
{
    [Fact]
    public void RegistrationModels_AcceptMissingOibAndJmbag()
    {
        var localRegistration = new RegisterModel.InputModel
        {
            Email = "local@example.com",
            Password = "Valid1!",
            ConfirmPassword = "Valid1!"
        };
        var externalRegistration = new ExternalLoginModel.InputModel
        {
            Email = "external@example.com"
        };

        Validate(new AppUser()).Should().BeEmpty();
        Validate(localRegistration).Should().BeEmpty();
        Validate(externalRegistration).Should().BeEmpty();
    }

    [Fact]
    public void OptionalIdentifiers_StillRequireTheExpectedFormatWhenProvided()
    {
        var localRegistration = new RegisterModel.InputModel
        {
            Email = "local@example.com",
            OIB = "123",
            JMBAG = "not-a-jmbag",
            Password = "Valid1!",
            ConfirmPassword = "Valid1!"
        };

        Validate(localRegistration).Should().HaveCount(2);
        Validate(new AppUser { OIB = "123", JMBAG = "not-a-jmbag" }).Should().HaveCount(2);
    }

    [Fact]
    public void CheckoutModel_AcceptsMissingOptionalContactAndAddress()
    {
        var input = new TicketBuilderCheckoutInputModel
        {
            CinemaId = 1,
            MovieId = 1,
            ScreeningId = 1,
            SeatId = 1,
            FirstName = "Ana",
            LastName = "Anić",
            Email = "ana@example.com"
        };

        Validate(input).Should().BeEmpty();
    }

    private static IReadOnlyCollection<ValidationResult> Validate(object model)
    {
        var results = new List<ValidationResult>();
        Validator.TryValidateObject(model, new ValidationContext(model), results, validateAllProperties: true);
        return results;
    }
}
