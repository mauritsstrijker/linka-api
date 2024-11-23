using Linka.Application.Repositories;
using Linka.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Linka.Infrastructure.Data.Repositories;
public class ProductRepository(Context context) : Repository<Product>(context), IProductRepository
{
    public Task<List<Product>> GetAllOrganizationProducts(Guid organizationId, CancellationToken cancellationToken)
    {
        return _context.Products
            .Where(p => p.Organization.Id == organizationId)
            .Where(p => p.IsDeleted == false)
            .ToListAsync(cancellationToken);
    }
}
