using Linka.Application.Data;
using Linka.Application.Repositories;
using MediatR;

namespace Linka.Application.Features.Products.Commands;
public class DeleteProductRequest : IRequest
{
    public Guid Id { get; set; }
}

public class DeleteProductHandler
    (
    IProductRepository productRepository,
    IUnitOfWork unitOfWork,
    IProductReservationRepository productReservationRepository
    ) : IRequestHandler<DeleteProductRequest>
{
    public async Task Handle(DeleteProductRequest request, CancellationToken cancellationToken)
    {
        //Softdelete true
        //Devolver pontos dos qeu tiverem pendentes
        var product = await productRepository.Get(request.Id, cancellationToken) ?? throw new Exception();

        product.IsDeleted = true;
        await productRepository.Update(product, cancellationToken);

        //Metodo repositorio buscar reservas deste produto especifico, cancelar e retornar aos usuarios os pontos


        await unitOfWork.Commit(cancellationToken);
    }
}
