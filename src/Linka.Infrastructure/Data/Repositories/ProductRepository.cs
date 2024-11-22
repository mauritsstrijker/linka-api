using Linka.Application.Repositories;
using Linka.Domain.Entities;

namespace Linka.Infrastructure.Data.Repositories;
public class ProductRepository(Context context) : Repository<Product>(context), IProductRepository
{
}
