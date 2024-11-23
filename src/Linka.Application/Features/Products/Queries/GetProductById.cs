using Linka.Application.Dtos;
using Linka.Application.Repositories;
using MediatR;

namespace Linka.Application.Features.Products.Queries
{
    public class GetProductByIdRequest : IRequest<GetProductByIdResponse>
    {
        public Guid Id { get; set; }
    }

    public class GetProductByIdResponse
    {
        public ProductDto Product { get; set; }
    }

    public class GetProductByIdHandler
        (
        IProductRepository repository
        ) : IRequestHandler<GetProductByIdRequest, GetProductByIdResponse>
    {
        public async Task<GetProductByIdResponse> Handle(GetProductByIdRequest request, CancellationToken cancellationToken)
        {
            var product = await repository.Get(request.Id, cancellationToken) ?? throw new Exception();

            return new GetProductByIdResponse
            {
                Product = new ProductDto
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Cost = product.Cost,
                    AvailableQuantity = product.AvailableQuantity,
                    ImageBase64 = product.Image != null ? Convert.ToBase64String(product.Image) : null
                }
            };
        }
    }
}
