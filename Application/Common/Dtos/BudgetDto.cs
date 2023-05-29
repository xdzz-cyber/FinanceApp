using Application.Common.Mappings;
using AutoMapper;
using Domain;

namespace Application.Common.Dtos;

//public record BudgetDto(string Name, decimal Amount, DateTime StartDate, DateTime EndDate) : IMapWith<Domain.Budget>;

// Write record in class format for budget dto

public record BudgetDto : IMapWith<Domain.Budget>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public decimal Amount { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public IEnumerable<TransactionDto>? Transactions { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Domain.Budget, BudgetDto>()
            .ForMember(m => m.Id, opt => opt.MapFrom(s => s.Id))
            .ForMember(m => m.Name, opt => opt.MapFrom(s => s.Name))
            .ForMember(m => m.Amount, opt => opt.MapFrom(s => s.Amount))
            .ForMember(m => m.StartDate, opt => opt.MapFrom(s => s.StartDate))
            .ForMember(m => m.EndDate, opt => opt.MapFrom(s => s.EndDate))
            .ForMember(m => m.Transactions, opt => opt.MapFrom(s => s.Transactions));
    }
}