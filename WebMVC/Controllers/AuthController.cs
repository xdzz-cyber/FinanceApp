using Application.Auth.Commands.Login.Commands;
using Application.Auth.Commands.Logout.Commands;
using Application.Auth.Commands.Registration.Commands;
using Microsoft.AspNetCore.Mvc;
using WebMVC.Models;

namespace WebMVC.Controllers;

public class AuthController : BaseController
{
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
        var loginCommand = new Login {Email = loginVm.Email, Password = loginVm.Password};
        var result = await Mediator.Send(loginCommand);
        
        if (result)
        {
            return RedirectToAction("Index", "Home");
        }
        return View();
    }
    
    // Logout
    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        var logoutCommand = new Logout();
        var result = await Mediator.Send(logoutCommand);
        
        return result ? RedirectToAction("Login", "Auth") : RedirectToAction("Index", "Home");
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
            return RedirectToAction("Index", "Home");
        }

        return View();
    }
}