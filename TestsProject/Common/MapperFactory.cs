using Application.Common.Mappings;
using Application.Interfaces;
using AutoMapper;

namespace TestsProject.Common;

public class MapperFactory
{
    public static IMapper Create()
    {
        var config = new MapperConfiguration(cfg =>
        {
            //cfg.AddMaps(typeof(MapperFactory).Assembly);
            cfg.AddProfile(new AssemblyMappingProfile(typeof(IApplicationDbContext).Assembly));
        });

        return config.CreateMapper();
    }
    
    public static void Destroy(IMapper mapper)
    {
        mapper = null!;
    }
}