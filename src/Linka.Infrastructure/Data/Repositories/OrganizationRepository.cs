using Linka.Application.Repositories;
using Linka.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Linka.Infrastructure.Data.Repositories
{
    public class OrganizationRepository(Context context) : Repository<Organization>(context), IOrganizationRepository
    {
        public override Task<Organization> Get(Guid id, CancellationToken cancellationToken)
        {
            return _context.Organizations
                .Include(x => x.Address)
                .Include(x => x.User)
                .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        }

        public async Task<List<Organization>> GetAllFollowing(Guid volunteerId, CancellationToken cancellationToken)
        {
            return await _context.Follows
            .Where(f => f.Volunteer.Id == volunteerId)
            .Include(f => f.Organization.User)
            .Include(f => f.Organization.Address)
            .Select(f => f.Organization)
            .ToListAsync();
        }

        public Task<Organization> GetByCNPJ(string cnpj, CancellationToken cancellationToken)
        {
            return _context.Organizations.FirstOrDefaultAsync(o => o.CNPJ == cnpj, cancellationToken);
        }

        public Task<Organization> GetByUserId(Guid userId, CancellationToken cancellationToken)
        {
            return _context.Organizations.FirstOrDefaultAsync(o => o.User.Id == userId, cancellationToken);
        }
    }
}
