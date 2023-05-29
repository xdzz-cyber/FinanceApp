using MediatR;

namespace Application.Auth.Commands.Login.Commands;

public class Login : IRequest<string>
{
    public string Email { get; set; }

    public string Password { get; set; }
}