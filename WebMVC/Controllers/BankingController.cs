using Hangfire;
using Microsoft.AspNetCore.Mvc;
using WebMVC.BackgroundJobs;

namespace WebMVC.Controllers;

public class BankingController : BaseController
{
    private readonly IBackgroundJobClient _backgroundJobClient;

    public BankingController(IBackgroundJobClient backgroundJobClient)
    {
        _backgroundJobClient = backgroundJobClient;
    }
    
    [HttpGet]
    public IActionResult Banking()
    {
        return View();
    }
    
    [HttpPost]
    public IActionResult ExecuteBackgroundJob()
    {
        _backgroundJobClient.Enqueue<HangfireRemoteApiCallJob>(x => x.MakeRemoteApiCall());
        return Ok();
    }
}