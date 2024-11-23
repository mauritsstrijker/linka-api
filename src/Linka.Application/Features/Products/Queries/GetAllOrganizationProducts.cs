using Linka.Application.Common;
using Linka.Application.Dtos;
using Linka.Application.Repositories;
using Linka.Domain.Entities;
using MediatR;

namespace Linka.Application.Features.Products.Queries
{
    public class GetAllOrganizationProductsRequest : IRequest<GetAllOrganizationProductsResponse>
    {
    }

    public class GetAllOrganizationProductsResponse
    {
        public List<ProductDto> Products { get; set; }
    }

    public class GetAllOrganizationProductsHandler
        (
        IProductRepository repository,
        IJwtClaimService jwt
        ): IRequestHandler<GetAllOrganizationProductsRequest, GetAllOrganizationProductsResponse>
    {
        public async Task<GetAllOrganizationProductsResponse> Handle(GetAllOrganizationProductsRequest request, CancellationToken cancellationToken)
        {
            var products = await repository.GetAllOrganizationProducts(Guid.Parse(jwt.GetClaimValue("id")), cancellationToken);

            return new GetAllOrganizationProductsResponse { Products = MapProductDtos(products) };
        }
        private List<ProductDto> MapProductDtos(List<Product> products)
        {
            return products.Select(product => new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Cost = product.Cost,
                AvailableQuantity = product.AvailableQuantity,
                ImageBase64 = product.Image != null ? Convert.ToBase64String(product.Image) : null
            }).ToList();
        }
    }
}
