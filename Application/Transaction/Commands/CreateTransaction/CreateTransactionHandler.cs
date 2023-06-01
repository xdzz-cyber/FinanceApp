using Application.Interfaces;
using AutoMapper;
using MediatR;

namespace Application.Transaction.Commands.CreateTransaction;

public class CreateTransactionHandler : IRequestHandler<CreateTransaction, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreateTransactionHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<Guid> Handle(CreateTransaction request, CancellationToken cancellationToken)
    {
        var transaction = new Domain.Transaction()
            {
                Amount = request.Amount,
                Date = request.Date,
                CategoryId = request.CategoryId,
                BudgetId = request.BudgetId,
                AppUserId = request.AppUserId
            };
        
        await _context.Transactions.AddAsync(transaction, cancellationToken);
        
        await _context.SaveChangesAsync(cancellationToken);

        return transaction.Id;
    }
}