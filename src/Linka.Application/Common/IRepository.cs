using Linka.Domain.Common;
using System.Linq.Expressions;

namespace Linka.Application.Common;

public interface IRepository<T> where T : BaseEntity
{
    Task<IEnumerable<T>> GetAsync(CancellationToken cancellationToken, params Expression<Func<T, object>>[] joins);
    Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> lambda, CancellationToken cancellationToken, params Expression<Func<T, object>>[] joins);
    Task<T> GetOrderedFirstAsync(Expression<Func<T, bool>> lambda, Expression<Func<T, object>> orderByDescending, CancellationToken cancellationToken, params Expression<Func<T, object>>[] joins);
    IAsyncEnumerable<T> GetPagingAsync(Expression<Func<T, bool>> lambda, int take = 1000, CancellationToken cancellationToken = default, params Expression<Func<T, object>>[] joins);
    Task<T> GetFirstAsync(Expression<Func<T, bool>> lambda, CancellationToken cancellationToken, params Expression<Func<T, object>>[] joins);
    Task<bool> ExistsAsync(Expression<Func<T, bool>> lambda, CancellationToken cancellationToken);
    Task AddAsync(T entity, CancellationToken cancellationToken);
    Task AddCollectionAsync(IEnumerable<T> entities, CancellationToken cancellationToken);
    Task UpdateAsync(T entity, CancellationToken cancellationToken);
    Task RemoveAsync(T entity, CancellationToken cancellationToken);
    Task RemoveAsyncByLambda(Expression<Func<T, bool>> lambda, CancellationToken cancellationToken);
    Task<long> GetCountAsync(CancellationToken cancellationToken);
    Task<long> GetCountAsync(Expression<Func<T, bool>> lambda, CancellationToken cancellationToken);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}