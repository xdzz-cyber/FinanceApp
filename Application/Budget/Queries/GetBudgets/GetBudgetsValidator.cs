using FluentValidation;

namespace Application.Budget.Queries.GetBudgets;

public class GetBudgetsValidator : AbstractValidator<GetBudgets>
{
    public GetBudgetsValidator()
    {
        RuleFor(x => x.UserId).NotEmpty()
            .WithMessage("UserId is required to get budgets");
    }
}