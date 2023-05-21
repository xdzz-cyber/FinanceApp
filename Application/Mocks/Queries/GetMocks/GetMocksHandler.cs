using Application.Common.Exceptions;
using Application.Common.ViewModels;
using Application.Interfaces;
using Application.Mocks.Queries.GetMock;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Mocks.Queries.GetMocks;

public class GetMocksHandler : IRequestHandler<GetMocks, List<MockVm>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetMocksHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<List<MockVm>> Handle(GetMocks request, CancellationToken cancellationToken)
    {
        var _ = await _context.Mocks.ToListAsync(cancellationToken);
        var mocks = await _context.Mocks.ProjectTo<MockVm>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return mocks;
    }
}