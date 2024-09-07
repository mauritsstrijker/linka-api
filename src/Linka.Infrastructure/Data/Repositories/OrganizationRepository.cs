using Linka.Application.Repositories;
using Linka.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Linka.Infrastructure.Data.Repositories
{
    public class OrganizationRepository(Context context) : Repository<Organization>(context), IOrganizationRepository
    {
        public Task<Organization> GetByCNPJ(string cnpj, CancellationToken cancellationToken)
        {
            return _context.Organizations.FirstOrDefaultAsync(v => v.CNPJ == cnpj, cancellationToken);
        }
    }
}
