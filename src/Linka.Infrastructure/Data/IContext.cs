using Microsoft.EntityFrameworkCore;

namespace Linka.Infrastructure.Data;
public interface IContext
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    DbSet<TEntity> Set<TEntity>() where TEntity : class;
}