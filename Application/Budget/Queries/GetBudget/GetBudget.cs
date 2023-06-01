using Application.Common.Dtos;
using MediatR;

namespace Application.Budget.Queries.GetBudget;

public class GetBudget : IRequest<BudgetDto>
{
    public Guid Id { get; set; }
}