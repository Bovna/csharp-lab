using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using KinoKlik.Model.Entities;

namespace KinoKlik.Web.Areas.Identity.Pages.Account;

public class RegisterModel : PageModel
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly ILogger<RegisterModel> _logger;

    public RegisterModel(
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        ILogger<RegisterModel> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _logger = logger;
    }

    [BindProperty]
    public InputModel Input { get; set; } = new();

    [BindProperty(SupportsGet = true)]
    public string? ReturnUrl { get; set; }

    public class InputModel
    {
        [Required(ErrorMessage = "E-pošta je obavezna.")]
        [EmailAddress(ErrorMessage = "Unesite ispravnu adresu e-pošte.")]
        [Display(Name = "E-pošta")]
        public string Email { get; set; } = string.Empty;

        [RegularExpression("^(?:[0-9]{11})?$", ErrorMessage = "OIB mora imati točno 11 znamenki kada je unesen.")]
        [Display(Name = "OIB (opcionalno)")]
        public string? OIB { get; set; }

        [RegularExpression("^(?:[0-9]{13})?$", ErrorMessage = "JMBAG mora imati točno 13 znamenki kada je unesen.")]
        [Display(Name = "JMBAG (opcionalno)")]
        public string? JMBAG { get; set; }

        [Required(ErrorMessage = "Lozinka je obavezna.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Lozinka mora imati najmanje {2} znakova.")]
        [DataType(DataType.Password)]
        [Display(Name = "Lozinka")]
        public string Password { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Lozinke se ne podudaraju.")]
        [Display(Name = "Potvrda lozinke")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }

    public void OnGet(string? returnUrl = null)
    {
        ReturnUrl = returnUrl ?? Url.Content("~/");
    }

    public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
    {
        ReturnUrl = returnUrl ?? Url.Content("~/");

        if (!ModelState.IsValid)
        {
            return Page();
        }

        var user = new AppUser
        {
            UserName = Input.Email,
            Email = Input.Email,
            OIB = NormalizeOptionalIdentifier(Input.OIB),
            JMBAG = NormalizeOptionalIdentifier(Input.JMBAG)
        };

        var result = await _userManager.CreateAsync(user, Input.Password);
        if (result.Succeeded)
        {
            _logger.LogInformation("New user account created.");
            await _signInManager.SignInAsync(user, isPersistent: false);
            return LocalRedirect(ReturnUrl);
        }

        foreach (var error in result.Errors)
        {
            var message = error.Code switch
            {
                "PasswordRequiresNonAlphanumeric" => "Lozinka mora sadržavati barem jedan poseban znak.",
                "PasswordRequiresUpper" => "Lozinka mora sadržavati barem jedno veliko slovo.",
                "PasswordRequiresLower" => "Lozinka mora sadržavati barem jedno malo slovo.",
                "PasswordRequiresDigit" => "Lozinka mora sadržavati barem jednu znamenku.",
                "PasswordTooShort" => "Lozinka mora imati najmanje 6 znakova.",
                "PasswordRequiresUniqueChars" => "Lozinka mora sadržavati dovoljan broj različitih znakova.",
                "DuplicateUserName" => "Korisnik s ovom e-poštom već postoji.",
                "DuplicateEmail" => "Korisnik s ovom e-poštom već postoji.",
                "InvalidEmail" => "Unesite ispravnu adresu e-pošte.",
                "InvalidUserName" => "Korisničko ime nije ispravno.",
                _ => error.Description
            };

            var key = error.Code.StartsWith("Password", StringComparison.Ordinal)
                ? "Input.Password"
                : error.Code.Contains("Email", StringComparison.Ordinal)
                    || error.Code.Contains("UserName", StringComparison.Ordinal)
                    ? "Input.Email"
                    : string.Empty;

            ModelState.AddModelError(key, message);
        }

        return Page();
    }

    private static string NormalizeOptionalIdentifier(string? value)
    {
        return value?.Trim() ?? string.Empty;
    }
}
