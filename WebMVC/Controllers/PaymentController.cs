using System.Text.Json;
using Application.Card.Commands.UpdateCard;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using Stripe;
using WebMVC.Models;

namespace WebMVC.Controllers;

public class PaymentController : BaseController
{
    private readonly string stripeSecretKey;
    private readonly string stripePublicKey;
    // private readonly IConnectionMultiplexer _redisConnection;

    public PaymentController(IConfiguration configuration)
    {
        stripeSecretKey = configuration.GetSection("StripeSecretKey").Value;
        stripePublicKey = configuration.GetSection("StripePublishableKey").Value;

        StripeConfiguration.ApiKey = stripeSecretKey;
    }

    public IActionResult Payment()
    {
        ViewBag.StripePublicKey = stripePublicKey;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Charge(string stripeToken, string cardNumber ,int amount)
    {
        var options = new ChargeCreateOptions
        {
            Amount = amount * 100, // Convert to dollars
            Currency = "usd",
            Source = stripeToken,
            Description = "Test payment"
        };

        var service = new ChargeService();
        var charge = await service.CreateAsync(options);

        if (charge.Paid)
        {
            var updateResult = await Mediator.Send(new UpdateCard()
            {
                Id = cardNumber,
                UpdateAmount = amount * 100 
            });
            
            // Check if update card success
            if (updateResult == null)
            {
                return BadRequest("Update card failed");
            }
            
            //return Ok();
            // return redirect to success page
            return RedirectToAction("Banking", "Banking");
        }
        
        return BadRequest("Payment failed");
    }
}