using Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Auth.Commands.Registration.Commands;

public class RegistrationHandler : IRequestHandler<Auth.Commands.Registration.Commands.Registration, bool>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMediator _mediator;

    public RegistrationHandler(UserManager<ApplicationUser> userManager, IMediator mediator)
    {
        _userManager = userManager;
        _mediator = mediator;
    }
    
    public async Task<bool> Handle(Auth.Commands.Registration.Commands.Registration request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        
        if (user is not null)
        {
            return false;
        }
        
        var createdUser = await _userManager.CreateAsync(new ApplicationUser
        {
            UserName = request.UserName,
            Email = request.Email
        }, request.Password);

        if (createdUser.Succeeded == false)
        {
            return false;
        }
        
        // Make request via mediator to login user after registration
        var loginResult = await _mediator.Send(new Auth.Commands.Login.Commands.Login
        {
            Email = request.Email,
            Password = request.Password
        }, cancellationToken);
        
        return loginResult;
    }
}