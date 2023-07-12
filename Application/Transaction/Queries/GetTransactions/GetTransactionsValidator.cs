using FluentValidation;

namespace Application.Transaction.Queries.GetTransactions;

public class GetTransactionsValidator : AbstractValidator<GetTransactions>
{
    public GetTransactionsValidator()
    {
        RuleFor(x => x.BudgetId).NotEmpty().WithMessage("BudgetId is required.");
    }
}