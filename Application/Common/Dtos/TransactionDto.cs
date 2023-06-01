using Application.Common.Mappings;
using AutoMapper;
using Domain;

namespace Application.Common.Dtos;

public record TransactionDto : IMapWith<Domain.Transaction>
{
    // Write properties for transaction dto and validate them
    
    public decimal Amount { get; set; }
    
    public DateTime Date { get; set; }
    
    public Guid CategoryId { get; set; }
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Domain.Transaction, TransactionDto>()
            .ForMember(m => m.Amount, opt => opt.MapFrom(s => s.Amount))
            .ForMember(m => m.Date, opt => opt.MapFrom(s => s.Date))
            .ForMember(m => m.CategoryId, opt => opt.MapFrom(s => s.CategoryId));
    }
    
}