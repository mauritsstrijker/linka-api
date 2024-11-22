using Linka.Application.Data;
using Linka.Application.Repositories;
using MediatR;

namespace Linka.Application.Features.Products.Commands;
public class EditProductAvailableQuantityRequest : IRequest
{
    public Guid Id { get; set; }
    public int AvailableQuantity { get; set; }
}

public class EditProductAvailableQuantityHandler
    (
    IProductRepository productRepository,
    IUnitOfWork unitOfWork
    ): IRequestHandler<EditProductAvailableQuantityRequest>
{
    public async Task Handle(EditProductAvailableQuantityRequest request, CancellationToken cancellationToken)
    {
        var product = await productRepository.Get(request.Id, cancellationToken) ?? throw new Exception();

        product.AvailableQuantity = request.AvailableQuantity;

        await productRepository.Update(product, cancellationToken);
        await unitOfWork.Commit(cancellationToken);
    }
}
