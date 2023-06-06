using Application.Interfaces;
using MediatR;

namespace Application.FinancialGoal.Commands.CreateFinancialGoal;

public class CreateFinancialGoalHandler : IRequestHandler<CreateFinancialGoal, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateFinancialGoalHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<Guid> Handle(CreateFinancialGoal request, CancellationToken cancellationToken)
    {
        var financialGoal = new Domain.FinancialGoal()
        {
            Name = request.Name,
            Description = request.Description,
            TargetAmount = request.TargetAmount,
            TargetDate = request.TargetDate,
            BudgetId = request.BudgetId,
            CategoryId = request.CategoryId,
            CurrentAmount = request.CurrentAmount
        };
        
        await _context.FinancialGoals.AddAsync(financialGoal, cancellationToken);
        
        await _context.SaveChangesAsync(cancellationToken);
        
        return financialGoal.Id;
    }
}