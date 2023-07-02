using Application.Common.Dtos;
using MediatR;

namespace Application.FinancialGoal.Queries.GetFinancialGoals;

public class GetFinancialGoals : IRequest<List<FinancialGoalDto>>
{
    // public Guid BudgetId { get; set; }

    // public string UserId { get; set; } = null!;
    
    // email
    public string Email { get; set; } = null!;
}