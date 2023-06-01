using FluentValidation;

namespace Application.Transaction.Commands.CreateTransaction;

public class CreateTransactionValidator : AbstractValidator<CreateTransaction>
{
    public CreateTransactionValidator()
    {
        // Add rules for the properties in the CreateTransaction command
        RuleFor(x => x.Amount)
            .NotEmpty()
            .LessThan(1000000)
            .GreaterThan(0);
        
        RuleFor(x => x.Date)
            .NotEmpty()
            // Add a rule to ensure the date is in correct format: yyyy-MM-dd
            .Must(x => DateTime.TryParse(x.ToString("yyyy-MM-dd"), out _))
            .LessThan(DateTime.Now);
        
        RuleFor(x => x.CategoryId)
            .NotEmpty();
        
        RuleFor(x => x.BudgetId)
            .NotEmpty();
        
        RuleFor(x => x.AppUserId)
            .NotEmpty();
    }
}