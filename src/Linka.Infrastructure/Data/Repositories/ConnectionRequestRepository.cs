using Linka.Application.Repositories;
using Linka.Domain.Entities;
using Linka.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Linka.Infrastructure.Data.Repositories
{
    public class ConnectionRequestRepository(Context context) : Repository<ConnectionRequest>(context), IConnectionRequestRepository
    {
        public async Task<bool> HasPendingConnectionRequestAsync(Guid volunteerId1, Guid volunteerId2, CancellationToken cancellationToken)
        {
            return await _context.ConnectionRequests
                .AnyAsync(cr =>
                    (cr.Requester.Id == volunteerId1 && cr.Target.Id == volunteerId2 ||
                     cr.Requester.Id == volunteerId2 && cr.Target.Id == volunteerId1)
                    && cr.Status == ConnectionRequestStatus.Pending,
                    cancellationToken);
        }
    }
}
