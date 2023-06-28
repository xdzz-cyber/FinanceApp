using Application.Common.Exceptions;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

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
        var budget = await _context.Budgets.AsNoTracking()
            .Where(b => b.AppUserId == request.UserId && b.Name == request.Name)
            .FirstOrDefaultAsync(cancellationToken);
        
        if(budget is not null)
        {
            throw new AlreadyExistsException("Budget", request.Name);
        }
        
        var createdBudget = new Domain.Budget
        {
            Name = request.Name,
            Amount = request.Amount,
            AppUserId = request.UserId,
            StartDate = request.StartDate,
            EndDate = request.EndDate
        };
        
        await _context.Budgets.AddAsync(createdBudget, cancellationToken);
        
        await _context.SaveChangesAsync(cancellationToken);
        
        return createdBudget.Id;
    }
}