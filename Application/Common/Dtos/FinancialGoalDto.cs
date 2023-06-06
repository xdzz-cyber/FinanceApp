using System.ComponentModel.DataAnnotations;
using Application.Common.Mappings;
using AutoMapper;

namespace Application.Common.Dtos;

public class FinancialGoalDto : IMapWith<Domain.FinancialGoal>
{
    // Add validation for Name
    [Required]
    [StringLength(50, MinimumLength = 1)]
    public string Name { get; set; } = null!;

    // Add validation for Description
    [Required]
    [StringLength(50, MinimumLength = 1)]
    public string Description { get; set; } = null!;

    // Add validation for TargetAmount
    [Required]
    [Range(1, 1000000)]
    public decimal TargetAmount { get; set; }
    // Add validation for CurrentAmount
    [Required]
    [Range(1, 1000000)]
    public decimal CurrentAmount { get; set; }
    // Add validation for CategoryName
    [Required]
    [StringLength(50, MinimumLength = 1)]
    public string CategoryName { get; set; } = null!;
    
    // Add validation for TargetDate
    [Required]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime TargetDate { get; set; }
    // Add validation for BudgetId
    [Required]
    public Guid BudgetId { get; set; }
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Domain.FinancialGoal, FinancialGoalDto>()
            .ForMember(d => d.Name, opt => opt.MapFrom(s => s.Name))
            .ForMember(d => d.Description, opt => opt.MapFrom(s => s.Description))
            .ForMember(d => d.TargetAmount, opt => opt.MapFrom(s => s.TargetAmount))
            .ForMember(d => d.TargetDate, opt => opt.MapFrom(s => s.TargetDate))
            .ForMember(d => d.BudgetId, opt => opt.MapFrom(s => s.BudgetId))
            .ForMember(d => d.CategoryName, opt => opt.MapFrom(s => s.Category.Name))
            .ForMember(d => d.CurrentAmount, opt => opt.MapFrom(s => s.CurrentAmount));
    }
}