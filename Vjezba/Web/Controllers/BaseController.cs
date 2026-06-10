using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Vjezba.Model.Entities;

namespace Vjezba.Web.Controllers;

public abstract class BaseController : Controller
{
    protected readonly UserManager<AppUser> UserManager;

    protected BaseController(UserManager<AppUser> userManager)
    {
        UserManager = userManager;
    }

    protected string? UserId => UserManager.GetUserId(User);
}
