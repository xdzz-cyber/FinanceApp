using Application.Common.Exceptions;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

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
        var financialGoalExists = await _context.FinancialGoals.AsNoTracking()
            .AnyAsync(x => x.Name == request.Name && x.Description == request.Description, cancellationToken);
        
        var financialGoalBudget = await _context.Budgets.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.BudgetId, cancellationToken);
        
        var financialGoalCategory = await _context.Categories.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.CategoryId, cancellationToken);

        if (financialGoalExists)
        {
            throw new AlreadyExistsException("Financial Goal with this name and description already exists.", nameof(request.Name));    
        }

        if (financialGoalBudget == null)
        {
            throw new ArgumentNullException(nameof(financialGoalBudget), "Budget with this id does not exist.");
        }
        
        if (financialGoalCategory == null)
        {
            throw new ArgumentNullException(nameof(financialGoalCategory), "Category with this id does not exist.");
        }

        if (financialGoalCategory.Name == "Expense" && (request.CurrentAmount > financialGoalBudget.Amount || request.TargetAmount > financialGoalBudget.Amount))
        {
            throw new ArgumentOutOfRangeException($"{nameof(request.CurrentAmount)} or(and) {nameof(request.TargetAmount)}", "Current amount and target amount cannot be greater than budget amount.");
        }

        var createdFinancialGoal = new Domain.FinancialGoal()
        {
            Name = request.Name,
            Description = request.Description,
            TargetAmount = request.TargetAmount,
            TargetDate = request.TargetDate,
            BudgetId = request.BudgetId,
            CategoryId = request.CategoryId,
            CurrentAmount = request.CurrentAmount
        };
        
        await _context.FinancialGoals.AddAsync(createdFinancialGoal, cancellationToken);
        
        await _context.SaveChangesAsync(cancellationToken);
        
        return createdFinancialGoal.Id;
    }
}