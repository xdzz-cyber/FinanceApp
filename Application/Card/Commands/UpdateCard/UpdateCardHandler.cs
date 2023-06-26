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

        if (newCards is not null && newCards.FirstOrDefault(c => c.Id == request.Id || c.StripeId.Substring(c.StripeId.Length - 4)
                == request.StripeId.Substring(request.StripeId.Length - 4)) is null)
        {
            newCards.Add(new CardDto{Id = request.Id, StripeId = request.StripeId});
        }

        //var card = newCards?.FirstOrDefault(c => c.Id == request.Id) is not null 
        //     ? newCards.FirstOrDefault(c => c.Id == request.Id): new CardDto(){Id = request.Id};

        var card = newCards?.FirstOrDefault(c => c.Id == request.Id || c.StripeId.Substring(c.StripeId.Length - 4) 
            == request.StripeId.Substring(request.StripeId.Length - 4));

        // if (!string.IsNullOrEmpty(request.StripeId) && string.IsNullOrEmpty(card!.StripeId))
        // {
        //     card.StripeId = request.StripeId;
        // }
            
        if (request.UpdateAmount > 0)
        {
            card!.UpdateAmount += request.UpdateAmount;
        }
        
        if(request.InitialAmount > 0)
        {
            card!.InitialAmount = request.InitialAmount;
        }
            
        var result = await db.StringSetAsync("cards", JsonSerializer.Serialize(newCards));

        return result ? card! : null;
    }
}