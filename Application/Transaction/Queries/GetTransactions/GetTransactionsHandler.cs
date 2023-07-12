using Application.Common.Dtos;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Transaction.Queries.GetTransactions;

public class GetTransactionsHandler : IRequestHandler<GetTransactions, IEnumerable<TransactionDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetTransactionsHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public Task<IEnumerable<TransactionDto>> Handle(GetTransactions request, CancellationToken cancellationToken)
    {
        var transactions = _context.Transactions.AsNoTracking()
            .Where(t => t.BudgetId == request.BudgetId)
            .ToList();
        
        return Task.FromResult(_mapper.Map<IEnumerable<TransactionDto>>(transactions));
    }
}