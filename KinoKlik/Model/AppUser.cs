using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

public class AppUser : IdentityUser
{
    [RegularExpression("^(?:[0-9]{11})?$", ErrorMessage = "OIB mora imati točno 11 znamenki kada je unesen.")]
    public string OIB { get; set; } = string.Empty;

    [RegularExpression("^(?:[0-9]{13})?$", ErrorMessage = "JMBAG mora imati točno 13 znamenki kada je unesen.")]
    public string JMBAG { get; set; } = string.Empty;
}
