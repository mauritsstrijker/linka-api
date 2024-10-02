using Linka.Application.Repositories;
using Linka.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Linka.Infrastructure.Data.Repositories
{
    public class EventJobRepository(Context context) : Repository<EventJob>(context), IEventJobRepository
    {
        public override Task<EventJob> Get(Guid id, CancellationToken cancellationToken)
        {
            return _context.EventJobs
                .Include(x => x.Volunteers)
                .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        }

        public Task<List<EventJob>> GetAllJobsByEventId(Guid eventId, CancellationToken cancellationToken)
        {
            return _context.EventJobs
                .Include(e => e.Event)
                .Include(x => x.Volunteers)
                .Where(e => e.Event.Id.Equals(eventId))
                .ToListAsync(cancellationToken);
        }
    }
}
