using Application.Common.Dtos;
using MediatR;

namespace Application.Budget.Queries.GetBudgets;

public class GetBudgets : IRequest<List<BudgetDto>>
{
    // public string UserId { get; set; } = null!;
    
    public string Email { get; set; } = null!;
}
