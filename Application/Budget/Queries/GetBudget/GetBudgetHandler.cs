using Application.Common.Dtos;
using Application.Common.Exceptions;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Budget.Queries.GetBudget;

public class GetBudgetHandler : IRequestHandler<GetBudget, BudgetDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetBudgetHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<BudgetDto> Handle(GetBudget request, CancellationToken cancellationToken)
    {
        var budget = await _context.Budgets.Include(b => b.Transactions)
            .FirstOrDefaultAsync(b => b.Id == request.Id, cancellationToken: cancellationToken);

        if (budget is null)
        {
            throw new NotFoundException(nameof(Domain.Budget), request.Id);
        }

        return await Task.FromResult(_mapper.Map<BudgetDto>(budget));
    }
}
