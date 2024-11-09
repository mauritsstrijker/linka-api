using Linka.Application.Common;
using Linka.Domain.Entities;

namespace Linka.Application.Repositories;
public interface IConnectionRepository : IRepository<Connection>
{
    Task<bool> HasConnectionAsync(Guid volunteerId1, Guid volunteerId2, CancellationToken cancellationToken);
    Task<int> ConnectionsCountByVolunteerId(Guid volunteerId, CancellationToken cancellationToken);
    Task<Connection> GetConnection(Guid volunteerId1, Guid volunteerId2, CancellationToken cancellationToken);
}
