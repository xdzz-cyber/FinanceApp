using FluentValidation;

namespace Application.FinancialGoal.Queries.GetFinancialGoals;

public class GetFinancialGoalsValidator : AbstractValidator<GetFinancialGoals>
{
    public GetFinancialGoalsValidator()
    {
        // RuleFor(x => x.BudgetId).NotEmpty();
        RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required to get financial goals");
    }
}