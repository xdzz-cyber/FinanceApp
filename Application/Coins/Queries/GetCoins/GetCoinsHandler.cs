using Application.Common.Dtos;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Coins.Queries.GetCoins;

public class GetCoinsHandler : IRequestHandler<GetCoins, List<CoinDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetCoinsHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<List<CoinDto>> Handle(GetCoins request, CancellationToken cancellationToken)
    {
        // var coins = await _context.Coins.AsNoTracking()
        //     .ProjectTo<CoinDto>(_mapper.ConfigurationProvider)
        //     .ToListAsync(cancellationToken: cancellationToken);
        
        // Take coins from the database based on page and per page.
        var coins = await _context.Coins.AsNoTracking()
            .ProjectTo<CoinDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);

        return coins;
    }
}