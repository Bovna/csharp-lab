using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Vjezba.Model.Entities;

namespace Vjezba.Web.Areas.Identity.Pages.Account;

public class LogoutModel : PageModel
{
    private readonly SignInManager<AppUser> _signInManager;
    private readonly ILogger<LogoutModel> _logger;

    public LogoutModel(SignInManager<AppUser> signInManager, ILogger<LogoutModel> logger)
    {
        _signInManager = signInManager;
        _logger = logger;
    }

    public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
    {
        await _signInManager.SignOutAsync();
        _logger.LogInformation("User logged out.");

        return LocalRedirect(returnUrl ?? Url.Content("~/"));
    }
}
