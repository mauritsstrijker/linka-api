using Linka.Application.Common;
using Linka.Application.Dtos;
using Linka.Application.Repositories;
using Linka.Domain.Entities;
using MediatR;

namespace Linka.Application.Features.Products.Queries
{
    public class GetVolunteerStoreRequest : IRequest<GetVolunteerStoreResponse>
    {
    }

    public class GetVolunteerStoreResponse
    {
        public List<OrganizationStoreDto> Stores { get; set; }
    }

    public class GetVolunteerStoreHandler
        (
        IOrganizationRepository organizationRepository,
        IProductRepository productRepository,
        IJwtClaimService jwtClaimService
        )
        : IRequestHandler<GetVolunteerStoreRequest, GetVolunteerStoreResponse>
    {
        public async Task<GetVolunteerStoreResponse> Handle(GetVolunteerStoreRequest request, CancellationToken cancellationToken)
        {
            var organizations = await organizationRepository.GetAllOrganizationsVolunteerHasEventInteraction(Guid.Parse(jwtClaimService.GetClaimValue("id")), cancellationToken);

            List<OrganizationStoreDto> stores = [];

            foreach (var organization in organizations)
            {

                var products = await productRepository.GetAllOrganizationProducts(organization.Id, cancellationToken);

                var store = new OrganizationStoreDto { OrganizationId = organization.Id, OrganizationName = organization.TradingName, Products = MapProductDtos(products) };

                stores.Add(store);
            }

            return new GetVolunteerStoreResponse { Stores = stores };
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

    public class OrganizationStoreDto
    {
        public Guid OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public List<ProductDto> Products { get; set; }
    }
}
