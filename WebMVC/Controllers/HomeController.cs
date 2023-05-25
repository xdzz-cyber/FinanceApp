using System.Diagnostics;
using Application.Common.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebMVC.Models;

namespace WebMVC.Controllers;

[Authorize]
public class HomeController : BaseController
{

    public HomeController()
    {
     
    }
    
    [HttpGet]
    public async Task<ActionResult<MockVm>> Index()
    {
        return View();
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<MockVm>> Details(Guid id)
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }
}