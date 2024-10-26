using Linka.Application.Repositories;
using Linka.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Linka.Infrastructure.Data.Repositories
{
    public class FollowRepository(Context context) : Repository<Follow>(context), IFollowRepository
    {
        public override Task<List<Follow>> GetAll(CancellationToken cancellationToken)
        {
            return _context.Follows
                .Include(x => x.Organization)
                .Include(x => x.Volunteer)
                .ToListAsync(cancellationToken);
        }

        public Task<Follow?> GetByOrganizationIdAndVolunteerId(Guid organizationId, Guid volunteerId, CancellationToken cancellationToken)
        {
            return _context.Follows
                .Include(x => x.Organization)
                .Include(x => x.Volunteer)
                .FirstOrDefaultAsync(f => f.Organization.Id == organizationId && f.Volunteer.Id == volunteerId, cancellationToken);
        }

        public Task<bool> IsFollowing(Guid organizationId, Guid volunteerId, CancellationToken cancellationToken)
        {
            return _context.Follows
             .AnyAsync(f => f.Organization.Id == organizationId && f.Volunteer.Id == volunteerId, cancellationToken);
        }
    }
}
