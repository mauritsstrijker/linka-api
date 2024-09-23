using Linka.Application.Repositories;
using Linka.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Linka.Infrastructure.Data.Repositories
{
    public class EventRepository(Context context) : Repository<Event>(context), IEventRepository
    {
        public override Task<Event> Get(Guid id, CancellationToken cancellationToken)
        {
            return _context.Events
                .Include(x => x.Address)
                .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        }

        public Task<List<Event>> GetByOrganizationId(Guid organizationId, CancellationToken cancellationToken)
        {
            return _context.Events
                .Include(x => x.Address)
                .Where(x => x.Organization.Id == organizationId)
                .ToListAsync(cancellationToken);
        }
    }
}
