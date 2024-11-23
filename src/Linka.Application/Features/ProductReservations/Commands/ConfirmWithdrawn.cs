using Linka.Application.Data;
using Linka.Application.Repositories;
using MediatR;

namespace Linka.Application.Features.ProductReservations.Commands
{
    public class ConfirmWithdrawnRequest : IRequest<ConfirmWithdrawnResponse>
    {
        public Guid ProductReservationId { get; set; }
    }

    public class ConfirmWithdrawnResponse;

    public class ConfirmWithdrawnHandler
        (
        IProductReservationRepository productReservationRepository,
        IUnitOfWork unitOfWork
        )
        : IRequestHandler<ConfirmWithdrawnRequest, ConfirmWithdrawnResponse>
    {
        public async Task<ConfirmWithdrawnResponse> Handle(ConfirmWithdrawnRequest request, CancellationToken cancellationToken)
        {
            var reservation = await productReservationRepository.Get(request.ProductReservationId, cancellationToken) ?? throw new Exception();

            if (!reservation.Cancelled && !reservation.Withdrawn) 
            {
                reservation.Withdrawn = true;
                reservation.RedeemDate = DateTime.Now;

                await productReservationRepository.Update(reservation, cancellationToken);
                await unitOfWork.Commit(cancellationToken);
            }

            return new ConfirmWithdrawnResponse();
        }
    }
}
