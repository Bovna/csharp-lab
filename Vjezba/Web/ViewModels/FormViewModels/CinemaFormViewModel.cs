namespace Vjezba.Web.ViewModels;

using System.ComponentModel.DataAnnotations;

public class CinemaFormViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Naziv kina je obavezan.")]
    [StringLength(120, ErrorMessage = "Naziv kina ne smije biti duži od {1} znakova.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Grad je obavezan.")]
    [StringLength(80, ErrorMessage = "Naziv grada ne smije biti duži od {1} znakova.")]
    public string City { get; set; } = string.Empty;

    [Required(ErrorMessage = "Ulica je obavezna.")]
    [StringLength(120, ErrorMessage = "Naziv ulice ne smije biti duži od {1} znakova.")]
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
}
