using Microsoft.EntityFrameworkCore;

namespace Application.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Domain.Mock> Mocks { get; set; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}