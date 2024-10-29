using Linka.Application.Common;
using Linka.Domain.Entities;

namespace Linka.Application.Repositories
{
    public interface IConnectionRequestRepository : IRepository<ConnectionRequest>
    {
        Task<bool> HasPendingConnectionRequestAsync(Guid volunteerId1, Guid volunteerId2, CancellationToken cancellationToken);
        Task<ConnectionRequest> GetPendingRequestAsync(Guid requesterId, Guid targetId, CancellationToken cancellationToken);
    }
}
