using AutoMapper;
using Persistence;

namespace TestsProject.Common;

public abstract class TestCommandBase
{
    protected readonly ApplicationDbContext Context;
    protected readonly IMapper Mapper;

    protected TestCommandBase()
    {
        Context = ApplicationDbContextFactory.Create();
        Mapper = MapperFactory.Create();
    }

    public void Dispose()
    {
        ApplicationDbContextFactory.Destroy(Context);
        MapperFactory.Destroy(Mapper);
    }
}