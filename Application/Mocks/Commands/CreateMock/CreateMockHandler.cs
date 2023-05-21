using Application.Interfaces;
using MediatR;

namespace Application.Mocks.Commands.CreateMock;

public class CreateMockHandler : IRequestHandler<CreateMock, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateMockHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateMock request, CancellationToken cancellationToken)
    {
        var mock = new Domain.Mock
        {
            Id = request.Id
        };

        await _context.Mocks.AddAsync(mock, cancellationToken);
        
        await _context.SaveChangesAsync(cancellationToken);

        return mock.Id;
    }
}