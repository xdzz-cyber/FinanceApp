using MediatR;

namespace Application.FinancialGoal.Commands.CreateFinancialGoal;

public class CreateFinancialGoal : IRequest<Guid>
{
    public string Name { get; set; } = null!;
    
    public string Description { get; set; } = null!;
    
    public decimal TargetAmount { get; set; }
    
    public decimal CurrentAmount { get; set; }
    
    public Guid CategoryId { get; set; }
    public DateTime TargetDate { get; set; }

    public Guid BudgetId { get; set; }
}