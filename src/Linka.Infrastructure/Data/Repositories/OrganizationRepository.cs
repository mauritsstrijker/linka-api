using Linka.Application.Repositories;
using Linka.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Linka.Infrastructure.Data.Repositories
{
    public class OrganizationRepository(Context context) : Repository<Organization>(context), IOrganizationRepository
    {
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
