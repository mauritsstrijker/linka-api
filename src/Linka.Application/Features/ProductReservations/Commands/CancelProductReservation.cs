using Linka.Application.Data;
using Linka.Application.Repositories;
using MediatR;

namespace Linka.Application.Features.ProductReservations.Commands
{
    public class CancelProductReservationRequest : IRequest<CancelProductReservationResponse>
    {
        public Guid ProductReservationId { get; set; }
    }

    public class CancelProductReservationResponse;

    public class CancelProductReservationHandler
        (
        IProductReservationRepository productReservationRepository,
        IVolunteerRepository volunteerRepository,
        IProductRepository productRepository,
        IUnitOfWork unitOfWork
        )
        : IRequestHandler<CancelProductReservationRequest, CancelProductReservationResponse>
    {
        public async Task<CancelProductReservationResponse> Handle(CancelProductReservationRequest request, CancellationToken cancellationToken)
        {
            var reservation = await productReservationRepository.Get(request.ProductReservationId, cancellationToken) ?? throw new Exception();

            reservation.Cancelled = true;

            await productReservationRepository.Update(reservation, cancellationToken);

            var volunteer = await volunteerRepository.Get(reservation.Volunteer.Id, cancellationToken) ?? throw new Exception();

            volunteer.Points += reservation.Product.Cost;

            var product = await productRepository.Get(reservation.Product.Id, cancellationToken) ?? throw new Exception();

            product.AvailableQuantity += 1;

            await productRepository.Update(product, cancellationToken);

            await volunteerRepository.Update(volunteer, cancellationToken);

            await unitOfWork.Commit(cancellationToken);
            return new CancelProductReservationResponse();
        }
    }
}
