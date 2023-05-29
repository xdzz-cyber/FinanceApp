using Application.Common.Mappings;
using AutoMapper;

namespace Application.Common.Dtos;

public class FinancialGoalDto : IMapWith<Domain.FinancialGoal>
{
    public string Name { get; set; } = null!;
    
    public string Description { get; set; } = null!;
    
    public decimal TargetAmount { get; set; }
    
    public DateTime TargetDate { get; set; }

    public Guid BudgetId { get; set; }
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Domain.FinancialGoal, FinancialGoalDto>()
            .ForMember(d => d.Name, opt => opt.MapFrom(s => s.Name))
            .ForMember(d => d.Description, opt => opt.MapFrom(s => s.Description))
            .ForMember(d => d.TargetAmount, opt => opt.MapFrom(s => s.TargetAmount))
            .ForMember(d => d.TargetDate, opt => opt.MapFrom(s => s.TargetDate))
            .ForMember(d => d.BudgetId, opt => opt.MapFrom(s => s.BudgetId));
    }
}