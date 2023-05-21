using System.Diagnostics;
using Application.Common.ViewModels;
using Application.Mocks.Queries.GetMock;
using Application.Mocks.Queries.GetMocks;
using Microsoft.AspNetCore.Mvc;
using WebMVC.Models;

namespace WebMVC.Controllers;

public class HomeController : BaseController
{

    public HomeController()
    {
     
    }
    
    [HttpGet]
    public async Task<ActionResult<MockVm>> Index()
    {
        var getMocksQuery = new GetMocks();
        var mocks = await Mediator.Send(getMocksQuery);
        return View(mocks);
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<MockVm>> Details(Guid id)
    {
        var getMockQuery = new GetMock {Id = id};
        var mock = await Mediator.Send(getMockQuery);
        return View(mock);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }
}