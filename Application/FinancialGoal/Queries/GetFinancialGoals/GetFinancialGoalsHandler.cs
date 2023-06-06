using Application.Budget.Queries.GetBudgets;
using Application.Common.Dtos;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.FinancialGoal.Queries.GetFinancialGoals;

public class GetFinancialGoalsHandler : IRequestHandler<GetFinancialGoals, List<FinancialGoalDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public GetFinancialGoalsHandler(IApplicationDbContext context, IMapper mapper, IMediator mediator)
    {
        _context = context;
        _mapper = mapper;
        _mediator = mediator;
    }
    
    public async Task<List<FinancialGoalDto>> Handle(GetFinancialGoals request, CancellationToken cancellationToken)
    {
        var userBudgets = await _mediator.Send(new GetBudgets {UserId = request.UserId}, cancellationToken);
        var budgetIds = userBudgets.Select(userBudget => userBudget.Id).ToList();

        var financialGoals = await _context.FinancialGoals
            .AsNoTracking()
            .Where(financialGoal => budgetIds.Contains(financialGoal.BudgetId))
            .ProjectTo<FinancialGoalDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return financialGoals;
    }
}