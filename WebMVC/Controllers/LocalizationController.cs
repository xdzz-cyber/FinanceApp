using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace WebMVC.Controllers;

[Authorize]
public class LocalizationController : BaseController
{
    [HttpGet]
    public IActionResult SetLanguage(string culture)
    {
        Response.Cookies.Append(
            CookieRequestCultureProvider.DefaultCookieName,
            CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
            new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
        );

        return RedirectToAction("Index","Home");
    }
}