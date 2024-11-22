using Linka.Application.Common;
using Linka.Application.Data;
using Linka.Application.Repositories;
using Linka.Domain.Entities;
using MediatR;

namespace Linka.Application.Features.Products.Commands;
public class CreateProductRequest : IRequest<CreateProductResponse>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int Cost { get; set; }
    public int AvailableQuantity { get; set; }
    public string? ImageBase64 { get; set; }
}

public class CreateProductResponse
{
    public Guid Id { get; set; }
}

public class CreateProductHandler
    (
    IProductRepository productRepository,
    IJwtClaimService jwtClaimService,
    IOrganizationRepository organizationRepository,
    IUnitOfWork unitOfWork
    ): IRequestHandler<CreateProductRequest, CreateProductResponse>
{
    public async Task<CreateProductResponse> Handle(CreateProductRequest request, CancellationToken cancellationToken)
    {
        var currentOrganizationId = Guid.Parse(jwtClaimService.GetClaimValue("id"));

        var currentOrganization = await organizationRepository.Get(currentOrganizationId, cancellationToken) ?? throw new Exception();

        byte[]? imageByte = null;

        if (request.ImageBase64 != null) {
            imageByte = Convert.FromBase64String(request.ImageBase64);
        }

        var newProduct = Product.Create(request.Name, request.Description, request.Cost, request.AvailableQuantity, imageByte, currentOrganization);

        await productRepository.Insert(newProduct, cancellationToken);
        await unitOfWork.Commit(cancellationToken);

        return new CreateProductResponse { Id = newProduct.Id };
    }
}
