using System.Security.Claims;
using Application.Auth.Commands.GoogleLogin.Commands;
using Application.Auth.Commands.Login.Commands;
using Application.Auth.Commands.Logout.Commands;
using Application.Auth.Commands.Registration.Commands;
using Domain;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebMVC.Models;

namespace WebMVC.Controllers;

public class AuthController : BaseController
{
    private readonly SignInManager<ApplicationUser> _signInManager;

    public AuthController(SignInManager<ApplicationUser> signInManager)
    {
        _signInManager = signInManager;
    }
    
    // Login
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> Login(LoginVm loginVm)
    {
        // Check if ModelState is valid
        if (!ModelState.IsValid)
        {
            return View(loginVm);
        }
        // var loginCommand = new Login {Email = loginVm.Email, Password = loginVm.Password};
        // var result = await Mediator.Send(loginCommand);
        //
        // if (result)
        // {
        //     return RedirectToAction("Index", "Home");
        // }
        
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
        {
            new (ClaimTypes.Name, loginVm.Email)
        }, CookieAuthenticationDefaults.AuthenticationScheme)));
        // return View();
        return RedirectToAction("Index", "Home");
    }
    
    // Logout
    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        // var logoutCommand = new Logout();
        // var result = await Mediator.Send(logoutCommand);
        //
        // return result ? RedirectToAction("Login", "Auth") : RedirectToAction("Index", "Home");
        
        await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
        //await HttpContext.ChallengeAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        
        return RedirectToAction("Login", "Auth");
    }
    
    // Register
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterVm registerVm)
    {
        // Check if ModelState is valid
        if (!ModelState.IsValid)
        {
            return View(registerVm);
        }
        var registerCommand = new Registration
        {
            UserName = registerVm.UserName,
            Email = registerVm.Email,
            Password = registerVm.Password
        };
        var result = await Mediator.Send(registerCommand);
        
        if (result)
        {
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
            {
                new (ClaimTypes.Name, registerCommand.Email)
            }, CookieAuthenticationDefaults.AuthenticationScheme)));
            return RedirectToAction("Index", "Home");
        }

        return View();
    }
    
    // [AllowAnonymous]
    public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null)
    {
        // var properties = _signInManager.ConfigureExternalAuthenticationProperties("Google", Url.Action("GoogleResponse", new { ReturnUrl = returnUrl }));
        // return new ChallengeResult("Google", properties);
        
        var properties = new AuthenticationProperties { RedirectUri = Url.Action("GoogleResponse", new { ReturnUrl = returnUrl }) };
        
        return Challenge(properties, GoogleDefaults.AuthenticationScheme);
    }
    
    // [AllowAnonymous]
    public async Task<IActionResult> GoogleResponse(string returnurl = null)
    {
        //var result = await HttpContext.AuthenticateAsync(IdentityConstants.ExternalScheme);
        var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        
        // get claims
        var externalClaims = result.Principal.Claims.ToList();
        // Select the claim information we want to use which is name and email
        var claims = externalClaims.Where(c => c.Type is ClaimTypes.Name or ClaimTypes.Email 
                or ClaimTypes.NameIdentifier or ClaimTypes.GivenName)
            .ToList();
        
        if(claims.Count > 0 && await _signInManager.UserManager.FindByEmailAsync(claims[1].Value) == null)
        {
            // var user = new ApplicationUser
            // {
            //     UserName = claims[0].Value,
            //     Email = claims[1].Value
            // };
            
            await Mediator.Send(new Registration
            {
                Email = claims[3].Value,
                UserName = claims[1].Value,
                Password = $"{claims[0].Value}{claims[2].Value}"
            });
            
        }
        
        // name comes first
        // try
        // {
        //     var signInResult = await Mediator.Send(new GoogleLogin());
        //
        //     return signInResult.Succeeded ? RedirectToAction("Index", "Home") : RedirectToAction("Login", "Auth");
        // }
        // catch (ArgumentNullException argumentNullException)
        // {
        //     RedirectToAction("Error", "Error", new {message = argumentNullException.Message});
        // }
        //
        return RedirectToAction("Index", "Home");
    }
}