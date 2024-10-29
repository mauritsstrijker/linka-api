﻿using Linka.Application.Repositories;
using Linka.Domain.Entities;
using Linka.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Linka.Infrastructure.Data.Repositories
{
    public class ConnectionRequestRepository(Context context) : Repository<ConnectionRequest>(context), IConnectionRequestRepository
    {
        public override Task<ConnectionRequest?> Get(Guid id, CancellationToken cancellationToken)
        {
            return _context.ConnectionRequests
                .Include(x => x.Requester)
                .Include(x => x.Target)
                .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        }
        public async Task<bool> HasPendingConnectionRequestAsync(Guid volunteerId1, Guid volunteerId2, CancellationToken cancellationToken)
        {
            return await _context.ConnectionRequests
                .AnyAsync(cr =>
                    (cr.Requester.Id == volunteerId1 && cr.Target.Id == volunteerId2 ||
                     cr.Requester.Id == volunteerId2 && cr.Target.Id == volunteerId1)
                    && cr.Status == ConnectionRequestStatus.Pending,
                    cancellationToken);
        }
        public async Task<ConnectionRequest> GetPendingRequestAsync(Guid requesterId, Guid targetId, CancellationToken cancellationToken)
        {
            return await _context.ConnectionRequests
                .FirstOrDefaultAsync(cr =>
                    (cr.Requester.Id == requesterId && cr.Target.Id == targetId) ||
                    (cr.Requester.Id == targetId && cr.Target.Id == requesterId) &&
                    cr.Status == ConnectionRequestStatus.Pending, cancellationToken);
        }
    }
}
