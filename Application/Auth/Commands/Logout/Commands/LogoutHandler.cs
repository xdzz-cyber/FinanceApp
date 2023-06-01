﻿using Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Auth.Commands.Logout.Commands;

public class LogoutHandler : IRequestHandler<Auth.Commands.Logout.Commands.Logout, bool>
{
    private readonly SignInManager<ApplicationUser> _signInManager;

    public LogoutHandler(SignInManager<ApplicationUser> signInManager)
    {
        _signInManager = signInManager;
    }
    
    public async Task<bool> Handle(Auth.Commands.Logout.Commands.Logout request, CancellationToken cancellationToken)
    {
        await _signInManager.SignOutAsync();
        
        return true;
    }
}