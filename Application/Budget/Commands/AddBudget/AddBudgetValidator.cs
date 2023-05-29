using System.Globalization;
using FluentValidation;

namespace Application.Budget.Commands.AddBudget;

public class AddBudgetValidator : AbstractValidator<AddBudget>
{
    public AddBudgetValidator()
    {
        // Add rules here for the AddBudget command
        RuleFor(addBudget => addBudget.Name)
            .NotEmpty()
            .MaximumLength(50)
            .WithMessage("Budget name must not be empty");
        
        RuleFor(addBudget => addBudget.Amount).GreaterThan(0)
            .LessThan(1000000)
            .WithMessage("Budget amount must be greater than 0");
        
        RuleFor(addBudget => addBudget.StartDate)
            // Start date must be in format yyyy-MM-dd
            .Must(date => DateTime.TryParseExact(date.ToString("yyyy-MM-dd"), "yyyy-MM-dd", null, DateTimeStyles.None, out _))
            .LessThan(addBudget => addBudget.EndDate)
            .WithMessage("Budget start date must be before end date");
        
        RuleFor(addBudget => addBudget.EndDate)
            // End date must be in format yyyy-MM-dd
            .Must(date => DateTime.TryParseExact(date.ToString("yyyy-MM-dd"), "yyyy-MM-dd", null, DateTimeStyles.None, out _))
            .GreaterThan(addBudget => addBudget.StartDate)
            .WithMessage("Budget end date must be after start date");
    }
}