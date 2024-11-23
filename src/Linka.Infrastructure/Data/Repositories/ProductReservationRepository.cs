using Linka.Application.Repositories;
using Linka.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Linka.Infrastructure.Data.Repositories;
public class ProductReservationRepository(Context context) : Repository<ProductReservation>(context), IProductReservationRepository
{
    public override Task<ProductReservation?> Get(Guid id, CancellationToken cancellationToken)
    {
        return _context.ProductReservations
            .Include(x => x.Volunteer)
            .Include(x => x.Product)
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public Task<List<ProductReservation>> GetAllByOrganization(Guid organizationId, CancellationToken cancellationToken)
    {
        return _context.ProductReservations
            .Where(r => r.Product.Organization.Id == organizationId)
            .Include(x => x.Volunteer)
            .Include(x => x.Product)
            .ToListAsync(cancellationToken);
    }

    public Task<List<ProductReservation>> GetAllByVolunteer(Guid volunteerId, CancellationToken cancellationToken)
    {
        return _context.ProductReservations
          .Where(r => r.Volunteer.Id == volunteerId)
          .Include(x => x.Volunteer)
          .Include(x => x.Product)
          .ToListAsync(cancellationToken);
    }

    public Task<List<ProductReservation>> GetAllPendingByProductId(Guid productId, CancellationToken cancellationToken)
    {
        return _context.ProductReservations
            .Where(x => x.Product.Id == productId)
            .Where(x => x.Cancelled == false && x.Withdrawn == false)
            .Include(x => x.Volunteer)
            .Include(x => x.Product)
            .ToListAsync(cancellationToken);
    }
}