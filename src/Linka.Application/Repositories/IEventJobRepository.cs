using Linka.Application.Common;
using Linka.Domain.Entities;

namespace Linka.Application.Repositories
{
    public interface IEventJobRepository : IRepository<EventJob>
    {
        Task<List<EventJob>> GetAllJobsByEventId(Guid eventId, CancellationToken cancellationToken);
    }
}
