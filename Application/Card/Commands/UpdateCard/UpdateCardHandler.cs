using System.Text.Json;
using Application.Card.Queries.GetCards;
using Application.Common.Dtos;
using MediatR;
using StackExchange.Redis;

namespace Application.Card.Commands.UpdateCard;

public class UpdateCardHandler : IRequestHandler<UpdateCard, CardDto?>
{
    private readonly IConnectionMultiplexer _redisConnection;
    private readonly IMediator _mediator;

    public UpdateCardHandler(IConnectionMultiplexer redisConnection, IMediator mediator)
    {
        _redisConnection = redisConnection;
        _mediator = mediator;
    }
    
    public async Task<CardDto?> Handle(UpdateCard request, CancellationToken cancellationToken)
    {
        // Add new amount for each card to redis
        var db = _redisConnection.GetDatabase();
        //     
        // if (!db.KeyExists("cards"))
        // {
        //     db.StringSet("cards", "[]");
        // }
        // var cards = await db.StringGetAsync("cards");
        //     
        // var newCards = JsonSerializer.Deserialize<List<CardDto>>(cards!);
        
        var cards = await _mediator.Send(new GetCards(), cancellationToken);

        if (cards.FirstOrDefault(c => c.Id == request.Id || c.StripeId.Substring(c.StripeId.Length - 4)
                == request.StripeId.Substring(request.StripeId.Length - 4)) is null)
        {
            cards.Add(new CardDto{Id = request.Id, StripeId = request.StripeId});
        }

        //var card = newCards?.FirstOrDefault(c => c.Id == request.Id) is not null 
        //     ? newCards.FirstOrDefault(c => c.Id == request.Id): new CardDto(){Id = request.Id};

        var card = cards?.FirstOrDefault(c => c.Id == request.Id || c.StripeId.Substring(c.StripeId.Length - 4) 
            == request.StripeId.Substring(request.StripeId.Length - 4));

        if (!string.IsNullOrEmpty(request.StripeId) && card is not null && string.IsNullOrEmpty(card.StripeId))
        {
            card.StripeId = request.StripeId;
        }
            
        if (request.UpdateAmount > 0)
        {
            card!.UpdateAmount += request.UpdateAmount;
        }
        
        if(request.InitialAmount > 0)
        {
            card!.InitialAmount = request.InitialAmount;
        }
            
        var result = await db.StringSetAsync("cards", JsonSerializer.Serialize(cards));

        return result ? card! : null;
    }
}