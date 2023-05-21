using Application.Common.Mappings;
using AutoMapper;
using Domain;

namespace Application.Common.ViewModels;

public class MockVm : IMapWith<Mock>
{
    public Guid Id { get; set; }

    public void Mapping(Profile profile) => profile.CreateMap<Mock, MockVm>()
        .ForMember(m => m.Id, opt
            => opt.MapFrom(s => s.Id));
}