using Application.Common.Exceptions;
using Application.Common.ViewModels;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;

namespace Application.Mocks.Queries.GetMock;

public class GetMockHandler : IRequestHandler<GetMock, MockVm>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetMockHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public Task<MockVm> Handle(GetMock request, CancellationToken cancellationToken)
    {
        var mock = _context.Mocks
            .ProjectTo<MockVm>(_mapper.ConfigurationProvider)
            .FirstOrDefault(m => m.Id == request.Id);

        if (mock == null)
        {
            throw new NotFoundException("Mock", request.Id);
        }
        
        return Task.FromResult(mock);
    }
}