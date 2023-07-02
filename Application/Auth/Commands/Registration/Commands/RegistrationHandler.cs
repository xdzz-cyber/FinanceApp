using Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Auth.Commands.Registration.Commands;

public class RegistrationHandler : IRequestHandler<Auth.Commands.Registration.Commands.Registration, bool>
{
    private readonly UserManager<Domain.ApplicationUser> _userManager;
    private readonly IMediator _mediator;

    public RegistrationHandler(UserManager<Domain.ApplicationUser> userManager, IMediator mediator)
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
        
        var createdUser = await _userManager.CreateAsync(new Domain.ApplicationUser
        {
            UserName = request.UserName.Replace(" ", string.Empty),
            Email = request.Email
        }, request.Password);

        return createdUser.Succeeded;
        // // Make request via mediator to login user after registration
        // var loginResult = await _mediator.Send(new Auth.Commands.Login.Commands.Login
        // {
        //     Email = request.Email,
        //     Password = request.Password
        // }, cancellationToken);
    }
}