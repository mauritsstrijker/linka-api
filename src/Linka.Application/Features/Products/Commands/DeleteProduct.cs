using Linka.Application.Data;
using Linka.Application.Repositories;
using MediatR;

namespace Linka.Application.Features.Products.Commands;
public class DeleteProductRequest : IRequest<DeleteProductResponse>
{
    public Guid Id { get; set; }
}

public class DeleteProductResponse;

public class DeleteProductHandler
    (
    IProductRepository productRepository,
    IUnitOfWork unitOfWork,
    IProductReservationRepository productReservationRepository,
    IVolunteerRepository volunteerRepository
    ) : IRequestHandler<DeleteProductRequest, DeleteProductResponse>
{
    public async Task<DeleteProductResponse> Handle(DeleteProductRequest request, CancellationToken cancellationToken)
    {
        var product = await productRepository.Get(request.Id, cancellationToken) ?? throw new Exception();

        product.IsDeleted = true;

        await productRepository.Update(product, cancellationToken);

        var pendingReservations = await productReservationRepository.GetAllPendingByProductId(product.Id, cancellationToken);

        foreach (var reservation in pendingReservations)
        {
            reservation.Cancelled = true;
            var volunteer = await volunteerRepository.Get(reservation.Volunteer.Id, cancellationToken) ?? throw new Exception();
            volunteer.Points += reservation.Cost;

            await volunteerRepository.Update(volunteer, cancellationToken);
            await productReservationRepository.Update(reservation, cancellationToken);
        }

        await unitOfWork.Commit(cancellationToken);

        return new DeleteProductResponse();
    }
}
