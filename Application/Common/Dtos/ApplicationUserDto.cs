using Application.Common.Mappings;
using AutoMapper;

namespace Application.Common.Dtos;

public class ApplicationUserDto : IMapWith<Domain.ApplicationUser>
{
    public string Id { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string UserName { get; set; } = null!;
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Domain.ApplicationUser, ApplicationUserDto>()
            .ForMember(m => m.Id, opt => opt.MapFrom(s => s.Id))
            .ForMember(m => m.Email, opt => opt.MapFrom(s => s.Email))
            .ForMember(m => m.UserName, opt => opt.MapFrom(s => s.UserName));
    }
}