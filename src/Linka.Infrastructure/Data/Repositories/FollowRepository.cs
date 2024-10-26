using Linka.Application.Repositories;
using Linka.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Linka.Infrastructure.Data.Repositories
{
    public class FollowRepository(Context context) : Repository<Follow>(context), IFollowRepository
    {
        public override Task<List<Follow>> GetAll(CancellationToken cancellationToken)
        {
            return _context.Follows
                .Include(x => x.Organization)
                .Include(x => x.Volunteer)
                .ToListAsync(cancellationToken);
        }
    }
}
