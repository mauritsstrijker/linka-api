using Linka.Application.Repositories;
using Linka.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Linka.Infrastructure.Data.Repositories
{
    public class JobVolunteerActivityRepository(Context context) : Repository<JobVolunteerActivity>(context), IJobVolunteerActivityRepository
    {
        public Task<JobVolunteerActivity> GetByJobAndVolunteer(Guid jobId, Guid volunteerId, CancellationToken cancellationToken)
        {
           return _context.JobVolunterActivities
                .Include(x => x.Volunteer)
                .Include(x => x.Job)
                .FirstOrDefaultAsync(ja => ja.Job.Id == jobId && ja.Volunteer.Id == volunteerId);
        }
    }
}
