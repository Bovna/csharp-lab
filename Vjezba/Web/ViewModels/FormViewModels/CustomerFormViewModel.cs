namespace Vjezba.Web.ViewModels;

using System.ComponentModel.DataAnnotations;

public class CustomerFormViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Ime je obavezno.")]
    [StringLength(60, ErrorMessage = "Ime ne smije biti duže od {1} znakova.")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Prezime je obavezno.")]
    [StringLength(60, ErrorMessage = "Prezime ne smije biti duže od {1} znakova.")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Grad je obavezan.")]
    [StringLength(80, ErrorMessage = "Grad ne smije biti duži od {1} znakova.")]
    public string City { get; set; } = string.Empty;

    [Required(ErrorMessage = "Ulica je obavezna.")]
    [StringLength(120, ErrorMessage = "Ulica ne smije biti duža od {1} znakova.")]
    public string Street { get; set; } = string.Empty;

    [Required(ErrorMessage = "Kućni broj je obavezan.")]
    [StringLength(20, ErrorMessage = "Kućni broj ne smije biti duži od {1} znakova.")]
    public string HouseNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Poštanski broj je obavezan.")]
    [StringLength(12, MinimumLength = 4, ErrorMessage = "Poštanski broj mora imati između {2} i {1} znakova.")]
    public string PostalCode { get; set; } = string.Empty;

    [Required(ErrorMessage = "E-pošta je obavezna.")]
    [EmailAddress(ErrorMessage = "Unesite ispravnu e-poštu.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Telefon je obavezan.")]
    [Phone(ErrorMessage = "Unesite ispravan telefonski broj.")]
    [StringLength(30, ErrorMessage = "Telefon ne smije biti duži od {1} znakova.")]
    public string Phone { get; set; } = string.Empty;

    [Required(ErrorMessage = "Datum registracije je obavezan.")]
    [DataType(DataType.DateTime)]
    public DateTime RegisteredAt { get; set; }

    public bool IsLoyaltyMember { get; set; }

    [Range(0, 10000, ErrorMessage = "Broj loyalty bodova ne može biti negativan.")]
    public int LoyaltyPoints { get; set; }
}
