using Linka.Application.Common;
using Linka.Domain.Entities;

namespace Linka.Application.Repositories;
public interface IProductReservationRepository : IRepository<ProductReservation>
{
    Task<List<ProductReservation>> GetAllPendingByProductId(Guid productId, CancellationToken cancellationToken);
    Task<List<ProductReservation>> GetAllByOrganization(Guid organizationId, CancellationToken cancellationToken);
    Task<List<ProductReservation>> GetAllByVolunteer(Guid volunteerId, CancellationToken cancellationToken);
}