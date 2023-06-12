using Microsoft.AspNetCore.Mvc;

namespace WebMVC.Controllers;

public class CartController : BaseController
{
    // [HttpPost]
    // public async Task<IActionResult> AddToCart([FromBody] string recipeIds)
    // {
    //     return Content("Add to cart");
    // }
    
    [HttpGet]
    public IActionResult Cart([FromQuery] string coinsIds)
    {
        return View();
    }
}