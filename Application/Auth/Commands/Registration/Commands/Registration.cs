using MediatR;

namespace Application.Auth.Commands.Registration.Commands;

public class Registration : IRequest<bool>
{
    public string UserName { get; set; }
    
    public string Email { get; set; }

    public string Password { get; set; }
}