using Application.Common.Dtos;
using Application.Interfaces;
using AutoMapper;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Budget.Queries.GetBudgets;

public class GetBudgetsHandler : IRequestHandler<GetBudgets, List<BudgetDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly UserManager<Domain.ApplicationUser> _userManager;

    public GetBudgetsHandler(IApplicationDbContext context, IMapper mapper, UserManager<Domain.ApplicationUser> userManager)
    {
        _context = context;
        _mapper = mapper;
        _userManager = userManager;
    }
    
    public async Task<List<BudgetDto>> Handle(GetBudgets request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        
        var budgets = await _context.Budgets.AsNoTracking().Include(b => b.Transactions).Where(b => b.AppUserId == user.Id)
            .ToListAsync(cancellationToken: cancellationToken);
        
        return await Task.FromResult(_mapper.Map<List<BudgetDto>>(budgets));
    }
}