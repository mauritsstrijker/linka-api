using Linka.Application.Common;
using Linka.Application.Data;
using Linka.Application.Repositories;
using Linka.Domain.Entities;
using MediatR;

namespace Linka.Application.Features.ProductReservations.Commands;
public class ReserveProductRequest : IRequest<ReserveProductResponse>
{
    public Guid Id { get; set; }
    public int Quantity { get; set; }
}

public class ReserveProductResponse;
public class ReserveProductHandler
    (
    IVolunteerRepository volunteerRepository,
    IProductRepository productRepository,
    IProductReservationRepository productReservationRepository,
    IJwtClaimService jwtClaimService,
    IUnitOfWork unitOfWork
    ): IRequestHandler<ReserveProductRequest, ReserveProductResponse>
{
    public async Task<ReserveProductResponse> Handle(ReserveProductRequest request, CancellationToken cancellationToken)
    {
        var product = await productRepository.Get(request.Id, cancellationToken)
             ?? throw new Exception("Produto não encontrado.");

        var volunteer = await volunteerRepository.Get(Guid.Parse(jwtClaimService.GetClaimValue("id")), cancellationToken)
            ?? throw new Exception("Voluntário não encontrado.");

        var totalCost = product.Cost * request.Quantity;
        if (volunteer.Points < totalCost)
            throw new Exception("Pontos insuficientes para o resgate da quantidade solicitada.");

        if (product.AvailableQuantity < request.Quantity)
            throw new Exception("Estoque insuficiente para a quantidade solicitada.");

        for (int i = 0; i < request.Quantity; i++)
        {
            var productReservation = ProductReservation.Create(product, volunteer);
            await productReservationRepository.Insert(productReservation, cancellationToken);
        }

        volunteer.Points -= totalCost;
        product.AvailableQuantity -= request.Quantity;

        await volunteerRepository.Update(volunteer, cancellationToken);
        await productRepository.Update(product, cancellationToken); 

        await unitOfWork.Commit(cancellationToken);

        return new ReserveProductResponse();
    }
}
