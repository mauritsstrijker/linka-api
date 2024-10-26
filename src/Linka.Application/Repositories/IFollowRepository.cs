using Linka.Application.Common;
using Linka.Domain.Entities;

namespace Linka.Application.Repositories
{
    public interface IFollowRepository : IRepository<Follow>
    {
        Task<bool> IsFollowing(Guid organizationId, Guid volunteerId, CancellationToken cancellationToken);
        Task<Follow?> GetByOrganizationIdAndVolunteerId(Guid organizationId, Guid volunteerId, CancellationToken cancellationToken);
    }
}
