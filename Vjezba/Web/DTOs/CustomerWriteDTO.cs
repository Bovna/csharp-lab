namespace Vjezba.Web.DTOs;

using System.ComponentModel.DataAnnotations;

public sealed class CustomerWriteDTO
{
    [Required(ErrorMessage = "Ime je obavezno.")]
    [StringLength(60, ErrorMessage = "Ime ne smije biti duze od {1} znakova.")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Prezime je obavezno.")]
    [StringLength(60, ErrorMessage = "Prezime ne smije biti duze od {1} znakova.")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Grad je obavezan.")]
    [StringLength(80, ErrorMessage = "Grad ne smije biti duzi od {1} znakova.")]
    public string City { get; set; } = string.Empty;

    [Required(ErrorMessage = "Ulica je obavezna.")]
    [StringLength(120, ErrorMessage = "Ulica ne smije biti duza od {1} znakova.")]
    public string Street { get; set; } = string.Empty;

    [Required(ErrorMessage = "Kucni broj je obavezan.")]
    [StringLength(20, ErrorMessage = "Kucni broj ne smije biti duzi od {1} znakova.")]
    public string HouseNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Postanski broj je obavezan.")]
    [StringLength(12, MinimumLength = 4, ErrorMessage = "Postanski broj mora imati izmedu {2} i {1} znakova.")]
    public string PostalCode { get; set; } = string.Empty;

    [Required(ErrorMessage = "E-posta je obavezna.")]
    [EmailAddress(ErrorMessage = "Unesite ispravnu e-postu.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Telefon je obavezan.")]
    [Phone(ErrorMessage = "Unesite ispravan telefonski broj.")]
    [StringLength(30, ErrorMessage = "Telefon ne smije biti duzi od {1} znakova.")]
    public string Phone { get; set; } = string.Empty;

    [Required(ErrorMessage = "Datum registracije je obavezan.")]
    public DateTime RegisteredAt { get; set; }

    public bool IsLoyaltyMember { get; set; }

    [Range(0, 10000, ErrorMessage = "Bodovi vjernosti moraju biti izmedu {1} i {2}.")]
    public int LoyaltyPoints { get; set; }
}
