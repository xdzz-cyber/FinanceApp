using FluentValidation;

namespace Application.FinancialGoal.Queries.GetFinancialGoals;

public class GetFinancialGoalsValidator : AbstractValidator<GetFinancialGoals>
{
    public GetFinancialGoalsValidator()
    {
        // RuleFor(x => x.BudgetId).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
    }
}