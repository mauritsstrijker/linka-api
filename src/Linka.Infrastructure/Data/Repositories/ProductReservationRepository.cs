using Linka.Application.Repositories;
using Linka.Domain.Entities;

namespace Linka.Infrastructure.Data.Repositories;
public class ProductReservationRepository(Context context) : Repository<ProductReservation>(context), IProductReservationRepository
{
}