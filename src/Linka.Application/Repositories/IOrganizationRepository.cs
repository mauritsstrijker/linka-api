using Linka.Application.Common;
using Linka.Domain.Entities;

namespace Linka.Application.Repositories
{
    public interface IOrganizationRepository : IRepository<Organization>
    {
        Task<Organization> GetByCNPJ(string cnpj, CancellationToken cancellationToken);
        Task<Organization> GetByUserId(Guid userId, CancellationToken cancellationToken);
        Task<List<Organization>> GetAllFollowing(Guid volunteerId, CancellationToken cancellationToken);
        Task<List<Organization>> GetAllOrganizationsVolunteerHasEventInteraction(Guid volunteerId, CancellationToken cancellationToken);
    }
}
