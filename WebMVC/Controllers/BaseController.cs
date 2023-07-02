using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebMVC.Controllers;

public class BaseController : Controller
{
    private IMediator _mediator;
    
    protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();
    
    //internal Guid UserId => User?.Identity?.IsAuthenticated == true ? Guid.Parse(User.Identity.Name) : Guid.Empty; 

    internal string Email => User?.Identity?.IsAuthenticated == true
        ? User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email) is not null
            ? User.Claims.First(c => c.Type == ClaimTypes.Email).Value
            : User.Identity.Name : string.Empty;
    // User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value : string.Empty;
}