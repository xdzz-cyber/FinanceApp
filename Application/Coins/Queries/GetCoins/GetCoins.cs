using Application.Common.Dtos;
using MediatR;

namespace Application.Coins.Queries.GetCoins;

public class GetCoins : IRequest<List<CoinDto>>
{
    
}