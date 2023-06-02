using Application.Common.Dtos;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Budget.Queries.GetBudgets;

public class GetBudgetsHandler : IRequestHandler<GetBudgets, List<BudgetDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetBudgetsHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<List<BudgetDto>> Handle(GetBudgets request, CancellationToken cancellationToken)
    {
        var budgets = await _context.Budgets.AsNoTracking().Where(b => b.AppUserId == request.UserId)
            .ToListAsync(cancellationToken: cancellationToken);
        
        return await Task.FromResult(_mapper.Map<List<BudgetDto>>(budgets));
    }
}