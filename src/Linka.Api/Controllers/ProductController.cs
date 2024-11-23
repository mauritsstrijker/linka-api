using Linka.Application.Features.Products.Commands;
using Linka.Application.Features.Products.Queries;
using Linka.Application.Features.Users.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Linka.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController(IMediator mediator) : ControllerBase
    {
        [Authorize]
        [HttpGet("store")]
        public async Task<GetVolunteerStoreResponse> VolunteerStore
            (
            CancellationToken cancellationToken
            )
        {
            return await mediator.Send(new GetVolunteerStoreRequest(), cancellationToken);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<GetProductByIdResponse> GetById
            (
            [FromRoute] Guid id,
            CancellationToken cancellationToken
            )
        {
            return await mediator.Send(new GetProductByIdRequest { Id = id }, cancellationToken);
        }

        [Authorize]
        [HttpGet("organization")]
        public async Task<GetAllOrganizationProductsResponse> GetAllOrganizationProducts
          (
          CancellationToken cancellationToken
          )
        {
            return await mediator.Send(new GetAllOrganizationProductsRequest(), cancellationToken);
        }

        [Authorize]
        [HttpPost]
        public async Task<CreateProductResponse> Create
          (
          [FromBody] CreateProductRequest request,
          CancellationToken cancellationToken
          )
        {
            return await mediator.Send(request, cancellationToken);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<DeleteProductResponse> Delete
         (
         [FromRoute] Guid id,
         CancellationToken cancellationToken
         )
        {
            return await mediator.Send(new DeleteProductRequest { Id = id}, cancellationToken);
        }

        [Authorize]
        [HttpPatch]
        public async Task<EditProductAvailableQuantityResponse> Update
         (
            [FromBody] EditProductAvailableQuantityRequest request,
            CancellationToken cancellationToken
         )
        {
            return await mediator.Send(request, cancellationToken);
        }
    }
}
