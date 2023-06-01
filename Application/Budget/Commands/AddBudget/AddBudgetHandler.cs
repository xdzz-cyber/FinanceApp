using Application.Interfaces;
using MediatR;

namespace Application.Budget.Commands.AddBudget;

public class AddBudgetHandler : IRequestHandler<AddBudget, Guid>
{
    private readonly IApplicationDbContext _context;

    public AddBudgetHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<Guid> Handle(AddBudget request, CancellationToken cancellationToken)
    {
        var budget = new Domain.Budget
        {
            Name = request.Name,
            Amount = request.Amount,
            AppUserId = request.UserId,
            StartDate = request.StartDate,
            EndDate = request.EndDate
        };
        
        await _context.Budgets.AddAsync(budget, cancellationToken);
        
        await _context.SaveChangesAsync(cancellationToken);
        
        return budget.Id;
    }
}