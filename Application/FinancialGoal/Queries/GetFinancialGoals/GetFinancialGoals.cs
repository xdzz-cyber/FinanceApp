using Application.Common.Dtos;
using MediatR;

namespace Application.FinancialGoal.Queries.GetFinancialGoals;

public class GetFinancialGoals : IRequest<List<FinancialGoalDto>>
{
    // public Guid BudgetId { get; set; }
}