using FluentValidation;

namespace Application.FinancialGoal.Commands.CreateFinancialGoal;

public class CreateFinancialGoalValidator : AbstractValidator<CreateFinancialGoal>
{
    public CreateFinancialGoalValidator()
    {
        // Create rule for Name
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required.")
            .MaximumLength(50)
            .WithMessage("Name must not exceed 50 characters.");
        
        // Create rule for Description
        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Description is required.")
            .MaximumLength(500)
            .WithMessage("Description must not exceed 500 characters.");
        
        // Create rule for TargetAmount
        RuleFor(x => x.TargetAmount)
            .NotEmpty()
            .WithMessage("TargetAmount is required.")
            .GreaterThan(0)
            .WithMessage("TargetAmount must be greater than 0.");
        
        // Create rule for TargetDate
        RuleFor(x => x.TargetDate)
            .NotEmpty()
            .WithMessage("TargetDate is required.")
            .GreaterThan(DateTime.Now)
            .WithMessage("TargetDate must be greater than today.");
        
        // Create rule for BudgetId
        RuleFor(x => x.BudgetId)
            .NotEmpty()
            .WithMessage("BudgetId is required.");
        
        // Create rule for CategoryId
        RuleFor(x => x.CategoryId)
            .NotEmpty()
            .WithMessage("CategoryId is required.");
        
        // Create rule for CurrentAmount
        RuleFor(x => x.CurrentAmount)
            .NotEmpty()
            .WithMessage("CurrentAmount is required.")
            .GreaterThanOrEqualTo(0)
            .WithMessage("CurrentAmount must be greater than or equal to 0.");
    }
}