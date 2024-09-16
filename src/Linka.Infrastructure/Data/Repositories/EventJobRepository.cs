using Linka.Application.Repositories;
using Linka.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Linka.Infrastructure.Data.Repositories
{
    public class EventJobRepository(Context context) : Repository<EventJob>(context), IEventJobRepository
    {
        public Task<List<EventJob>> GetAllJobsByEventId(Guid eventId, CancellationToken cancellationToken)
        {
            return _context.EventJobs
                .Include(e => e.Event)
                .Where(e => e.Event.Id.Equals(eventId))
                .ToListAsync(cancellationToken);
        }
    }
}
