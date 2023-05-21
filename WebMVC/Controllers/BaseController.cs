using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebMVC.Controllers;

public class BaseController : Controller
{
    private IMediator _mediator;
    
    protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();
    
    internal Guid UserId => User?.Identity?.IsAuthenticated == true ? Guid.Parse(User.Identity.Name) : Guid.Empty; 
}