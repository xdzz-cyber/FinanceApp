using Application.Common.Dtos;
using MediatR;

namespace Application.Card.Commands.UpdateCard;

public class UpdateCard : IRequest<CardDto?>
{
    public string Id { get; set; } = null!;

    public string StripeId { get; set; } = null!;
    public decimal UpdateAmount { get; set; }
    public decimal InitialAmount { get; set; }
}