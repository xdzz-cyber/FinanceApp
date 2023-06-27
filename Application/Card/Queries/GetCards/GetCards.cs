using Application.Common.Dtos;
using MediatR;

namespace Application.Card.Queries.GetCards;

public class GetCards : IRequest<List<CardDto>>
{
    
}