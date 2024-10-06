using Linka.Application.Common;
using Linka.Domain.Entities;

namespace Linka.Application.Repositories
{
    public interface IJobVolunteerActivityRepository : IRepository<JobVolunteerActivity>
    {
        Task<JobVolunteerActivity> GetByJobAndVolunteer(Guid jobId, Guid volunteerId, CancellationToken cancellationToken);
    }
}
