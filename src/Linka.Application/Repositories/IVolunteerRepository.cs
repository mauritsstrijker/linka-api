using Linka.Application.Common;
using Linka.Domain.Entities;

namespace Linka.Application.Repositories
{
    public interface IVolunteerRepository : IRepository<Volunteer>
    {
        Task<Volunteer> GetByCPF(string cpf, CancellationToken cancellationToken);
        Task<Volunteer> GetByUserId(Guid userId, CancellationToken cancellationToken);
    }
}
