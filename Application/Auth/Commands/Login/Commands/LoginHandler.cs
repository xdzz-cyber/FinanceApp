using Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Auth.Commands.Login.Commands;

public class LoginHandler : IRequestHandler<Auth.Commands.Login.Commands.Login, bool>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public LoginHandler(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }
    
    public async Task<bool> Handle(Auth.Commands.Login.Commands.Login request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        
        if (user == null)
        {
            return false;
        }
        
        var result = await _signInManager.PasswordSignInAsync(user, request.Password, false, false);
        
        return result.Succeeded;
    }
}