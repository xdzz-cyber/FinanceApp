using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Auth.Commands.GoogleLogin.Commands;

public class GoogleLogin : IRequest<SignInResult>
{
    
}