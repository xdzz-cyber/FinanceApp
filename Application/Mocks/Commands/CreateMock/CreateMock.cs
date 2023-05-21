using MediatR;

namespace Application.Mocks.Commands.CreateMock;

public class CreateMock : IRequest<Guid>
{
    public Guid Id { get; set; }
}