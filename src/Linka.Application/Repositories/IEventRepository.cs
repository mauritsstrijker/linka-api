using Linka.Application.Common;
using Linka.Domain.Entities;

namespace Linka.Application.Repositories
{
    public interface IEventRepository : IRepository<Event>
    {
        Task<Event> Get(Guid id, CancellationToken cancellationToken);
        Task<List<Event>> GetByOrganizationId(Guid organizationId, CancellationToken cancellationToken);
        Task<List<Event>> GetByVolunteerId(Guid volunteerId, CancellationToken cancellationToken);
    }
}
