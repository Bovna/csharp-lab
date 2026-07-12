using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using KinoKlik.Model.Entities;

namespace KinoKlik.Web.Areas.Identity.Pages.Account;

public class ExternalLoginModel : PageModel
{
    private readonly SignInManager<AppUser> _signInManager;
    private readonly UserManager<AppUser> _userManager;
    private readonly ILogger<ExternalLoginModel> _logger;

    public ExternalLoginModel(
        SignInManager<AppUser> signInManager,
        UserManager<AppUser> userManager,
        ILogger<ExternalLoginModel> logger)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _logger = logger;
    }

    [BindProperty]
    public InputModel Input { get; set; } = new();

    public string? ProviderDisplayName { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? ReturnUrl { get; set; }

    [TempData]
    public string? ErrorMessage { get; set; }

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
    }

    public IActionResult OnPost(string provider, string? returnUrl = null)
    {
        var redirectUrl = Url.Page("./ExternalLogin", pageHandler: "Callback", values: new { returnUrl });
        var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
        return new ChallengeResult(provider, properties);
    }

    public async Task<IActionResult> OnGetCallbackAsync(string? returnUrl = null, string? remoteError = null)
    {
        ReturnUrl = returnUrl ?? Url.Content("~/");

        if (!string.IsNullOrWhiteSpace(remoteError))
        {
            ErrorMessage = $"Greška pri vanjskoj prijavi: {remoteError}";
            return RedirectToPage("./Login", new { ReturnUrl = ReturnUrl });
        }

        var info = await _signInManager.GetExternalLoginInfoAsync();
        if (info == null)
        {
            ErrorMessage = "Nije bilo moguće učitati informacije vanjske prijave.";
            return RedirectToPage("./Login", new { ReturnUrl = ReturnUrl });
        }

        var signInResult = await _signInManager.ExternalLoginSignInAsync(
            info.LoginProvider,
            info.ProviderKey,
            isPersistent: false,
            bypassTwoFactor: true);

        if (signInResult.Succeeded)
        {
            _logger.LogInformation("User logged in with {Provider}.", info.LoginProvider);
            return LocalRedirect(ReturnUrl);
        }

        ProviderDisplayName = info.ProviderDisplayName;
        Input.Email = info.Principal.FindFirstValue(ClaimTypes.Email) ?? string.Empty;
        return Page();
    }

    public async Task<IActionResult> OnPostConfirmationAsync(string? returnUrl = null)
    {
        ReturnUrl = returnUrl ?? Url.Content("~/");

        var info = await _signInManager.GetExternalLoginInfoAsync();
        if (info == null)
        {
            ErrorMessage = "Nije bilo moguće učitati informacije vanjske prijave.";
            return RedirectToPage("./Login", new { ReturnUrl = ReturnUrl });
        }

        ProviderDisplayName = info.ProviderDisplayName;

        if (!ModelState.IsValid)
        {
            return Page();
        }

        var user = new AppUser
        {
            UserName = Input.Email,
            Email = Input.Email,
            EmailConfirmed = true,
            OIB = NormalizeOptionalIdentifier(Input.OIB),
            JMBAG = NormalizeOptionalIdentifier(Input.JMBAG)
        };

        var createResult = await _userManager.CreateAsync(user);
        if (!createResult.Succeeded)
        {
            foreach (var error in createResult.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return Page();
        }

        var addLoginResult = await _userManager.AddLoginAsync(user, info);
        if (!addLoginResult.Succeeded)
        {
            foreach (var error in addLoginResult.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return Page();
        }

        await _signInManager.SignInAsync(user, isPersistent: false);
        return LocalRedirect(ReturnUrl);
    }

    private static string NormalizeOptionalIdentifier(string? value)
    {
        return value?.Trim() ?? string.Empty;
    }
}
