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
    public async Task<IActionResult> Charge(string stripeToken ,int amount)
    {
        var tokenService = new TokenService();
        
        var stripeTokenObject = await tokenService.GetAsync(stripeToken);
    
        var cardNumberCardLast4 = stripeTokenObject.Card?.Last4; // Retrieve the last 4 digits of the card number
        
        var options = new ChargeCreateOptions
        {
            Amount = amount * 100, // Convert to dollars
            Currency = "usd",
            Source = stripeToken,
            Description = "Test payment",
        };

        var service = new ChargeService();
        
        var charge = await service.CreateAsync(options);

        if (charge.Paid)
        {
            var updateResult = await Mediator.Send(new UpdateCard()
            {
                StripeId = cardNumberCardLast4!,
                UpdateAmount = amount * 100 
            });
            
            // Check if update card success
            if (updateResult == null)
            {
                return BadRequest("Update card failed");
            }
            
            return RedirectToAction("Banking", "Banking");
        }
        
        return BadRequest("Payment failed");
    }
}