using FluentValidation;

namespace Application.Budget.Queries.GetBudget;

public class GetBudgetValidator : AbstractValidator<GetBudget>
{
    public GetBudgetValidator()
    {
        // Add rule for budget id
        RuleFor(getBudget => getBudget.Id)
            .NotEmpty()
            .WithMessage("Budget id must not be empty");
    }
}