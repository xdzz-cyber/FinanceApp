using MediatR;

namespace Application.Auth.Commands.Logout.Commands;

public class Logout : IRequest<string>
{
    public string Id { get; set; }
}