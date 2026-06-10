using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

public class AppUser : IdentityUser
{
    [Required(ErrorMessage = "OIB je obavezan.")]
    [StringLength(11, MinimumLength = 11, ErrorMessage = "OIB mora imati točno {1} znamenki.")]
    [RegularExpression("^[0-9]*$", ErrorMessage = "OIB smije sadržavati samo brojeve.")]
    public string OIB { get; set; } = string.Empty;

    [Required(ErrorMessage = "JMBAG je obavezan.")]
    [StringLength(13, MinimumLength = 13, ErrorMessage = "JMBAG mora imati točno {1} znamenki.")]
    [RegularExpression("^[0-9]*$", ErrorMessage = "JMBAG smije sadržavati samo brojeve.")]
    public string JMBAG { get; set; } = string.Empty;
}
