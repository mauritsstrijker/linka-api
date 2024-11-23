using Linka.Application.Common;
using Linka.Domain.Entities;

namespace Linka.Application.Repositories;
public interface IProductRepository : IRepository<Product>
{
    Task<List<Product>> GetAllOrganizationProducts(Guid organizationId, CancellationToken cancellationToken);
}
