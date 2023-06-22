using System.Text.Json;
using Application.Common.Dtos;
using MediatR;
using StackExchange.Redis;

namespace Application.Card.Commands.UpdateCard;

public class UpdateCardHandler : IRequestHandler<UpdateCard, CardDto?>
{
    private readonly IConnectionMultiplexer _redisConnection;

    public UpdateCardHandler(IConnectionMultiplexer redisConnection)
    {
        _redisConnection = redisConnection;
    }
    
    public async Task<CardDto?> Handle(UpdateCard request, CancellationToken cancellationToken)
    {
        // Add new amount for each card to redis
        var db = _redisConnection.GetDatabase();
            
        if (!db.KeyExists("cards"))
        {
            db.StringSet("cards", "[]");
        }
        var cards = await db.StringGetAsync("cards");
            
        var newCards = JsonSerializer.Deserialize<List<CardDto>>(cards!);
            
        var card = newCards?.FirstOrDefault(c => c.Id == request.Id);
            
        if (card != null && request.UpdateAmount != 0)
        {
            card.UpdateAmount += request.UpdateAmount;
        }
        
        if(card != null && request.InitialAmount != 0)
        {
            card.InitialAmount = request.InitialAmount;
        }
            
        var result = await db.StringSetAsync("cards", JsonSerializer.Serialize(newCards));

        return result ? card! : null;
    }
}