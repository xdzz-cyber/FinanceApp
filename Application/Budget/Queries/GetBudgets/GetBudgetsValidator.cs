using FluentValidation;

namespace Application.Budget.Queries.GetBudgets;

public class GetBudgetsValidator : AbstractValidator<GetBudgets>
{
    public GetBudgetsValidator()
    {
        RuleFor(x => x.Email).NotEmpty()
            .WithMessage("Email is required to get budgets");
    }
}