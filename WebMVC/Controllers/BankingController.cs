using Microsoft.AspNetCore.Mvc;

namespace WebMVC.Controllers;

public class BankingController : BaseController
{
    [HttpGet]
    public IActionResult Banking()
    {
        return View();
    }
}