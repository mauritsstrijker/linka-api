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
                .Include(x => x.Organization)
                .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        }

        public override Task<List<Event>> GetAll(CancellationToken cancellationToken)
        {
            return _context.Events
                .Include(x => x.Address)
                .Include(x => x.Organization)
                .ToListAsync(cancellationToken);
        }

        public Task<List<Event>> GetByOrganizationId(Guid organizationId, CancellationToken cancellationToken)
        {
            return _context.Events
                .Include(x => x.Address)
                .Where(x => x.Organization.Id == organizationId)
                .ToListAsync(cancellationToken);
        }

        public Task<List<Event>> GetByVolunteerId(Guid volunteerId, CancellationToken cancellationToken)
        {
            return _context.Events
                .Include(x => x.Address) 
                .Include(x => x.Jobs) 
                    .ThenInclude(job => job.Volunteers)
                .Where(x => x.Jobs.Any(job => job.Volunteers.Any(v => v.Id == volunteerId)))
                .ToListAsync(cancellationToken);
        }
    }
}
