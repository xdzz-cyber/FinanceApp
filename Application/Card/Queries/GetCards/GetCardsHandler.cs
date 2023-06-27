using System.Text.Json;
using Application.Common.Dtos;
using MediatR;
using StackExchange.Redis;

namespace Application.Card.Queries.GetCards;

public class GetCardsHandler : IRequestHandler<GetCards, List<CardDto>>
{
    private readonly IConnectionMultiplexer _redisConnection;

    public GetCardsHandler(IConnectionMultiplexer redisConnection)
    {
        _redisConnection = redisConnection;
    }
    
    public async Task<List<CardDto>> Handle(GetCards request, CancellationToken cancellationToken)
    {
        var db = _redisConnection.GetDatabase();
            
        if (!db.KeyExists("cards"))
        {
            db.StringSet("cards", "[]");
        }
        var cards = await db.StringGetAsync("cards");
        
        var parsedCards = JsonSerializer.Deserialize<List<CardDto>>(cards!);
            
        return parsedCards ?? new List<CardDto>();
    }
}