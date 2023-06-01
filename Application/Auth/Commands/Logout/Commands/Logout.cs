using MediatR;

namespace Application.Auth.Commands.Logout.Commands;

public class Logout : IRequest<bool>
{
    public string Id { get; set; }
}