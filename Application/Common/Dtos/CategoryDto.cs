using Application.Common.Mappings;
using AutoMapper;

namespace Application.Common.Dtos;

public class CategoryDto : IMapWith<Domain.Category>
{
    public Guid Id { get; set; }
    
    public string Name { get; set; } = null!;
    
    public string Description { get; set; } = null!;
    
    public int TransactionsConnected { get; set; }
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Domain.Category, CategoryDto>()
            .ForMember(m => m.Id, opt => opt.MapFrom(s => s.Id))
            .ForMember(m => m.Name, opt => opt.MapFrom(s => s.Name))
            .ForMember(m => m.Description, opt => opt.MapFrom(s => s.Description));
    }
}