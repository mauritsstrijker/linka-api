using Linka.Domain.Common;
using System.Linq.Expressions;

namespace Linka.Application.Common;

public interface IRepository<T> where T : BaseEntity
{
    Task Insert(T entity, CancellationToken cancellationToken);
    Task Update(T entity, CancellationToken cancellationToken);
    Task Delete(T? entity, CancellationToken cancellationToken);
    Task<T?> Get(Guid id, CancellationToken cancellationToken);
    Task<List<T>> GetAll(CancellationToken cancellationToken);
    Task<bool> Exists(Guid id, CancellationToken cancellationToken);
}