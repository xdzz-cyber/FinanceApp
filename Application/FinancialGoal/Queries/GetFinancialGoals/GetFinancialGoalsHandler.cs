using Application.Common.Dtos;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.FinancialGoal.Queries.GetFinancialGoals;

public class GetFinancialGoalsHandler : IRequestHandler<GetFinancialGoals, List<FinancialGoalDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetFinancialGoalsHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<List<FinancialGoalDto>> Handle(GetFinancialGoals request, CancellationToken cancellationToken)
    {
        var financialGoals = await _context.FinancialGoals
            .ToListAsync(cancellationToken: cancellationToken);
        
        return _mapper.Map<List<FinancialGoalDto>>(financialGoals);
    }
}