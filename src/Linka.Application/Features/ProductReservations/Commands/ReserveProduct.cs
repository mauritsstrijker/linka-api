using Linka.Application.Common;
using Linka.Application.Repositories;
using MediatR;

namespace Linka.Application.Features.ProductReservations.Commands;
public class ReserveProductRequest : IRequest
{
    public Guid Id { get; set; }
}

public class ReserveProductHandler
    (
    IVolunteerRepository volunteerRepository,
    IProductRepository productRepository,
    IProductReservationRepository productReservationRepository,
    IJwtClaimService jwtClaimService
    ): IRequestHandler<ReserveProductRequest>
{
    public async Task Handle(ReserveProductRequest request, CancellationToken cancellationToken)
    {
        var product = await productRepository.Get(request.Id, cancellationToken) ?? throw new Exception();

        var volunteer = await volunteerRepository.Get(Guid.Parse(jwtClaimService.GetClaimValue("id")), cancellationToken) ?? throw new Exception();

        if (volunteer.Points < product.Cost)
            throw new Exception("Pontos insuficientes para o resgate.");

        //Criar reserva e descontar da quantidade total do produto
    }
}
