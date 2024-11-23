using Linka.Application.Features.ProductReservations.Commands;
using Linka.Application.Features.ProductReservations.Queries;
using Linka.Application.Features.Products.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Linka.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductReservationController(IMediator mediator) : ControllerBase
    {
        [Authorize]
        [HttpGet("organization")]
        public async Task<GetAllOrganizationReservationsResponse> GetAllOrganizationReservations
           (
           CancellationToken cancellationToken
           )
        {
            return await mediator.Send(new GetAllOrganizationReservationsRequest(), cancellationToken);
        }

        [Authorize]
        [HttpGet("volunteer")]
        public async Task<GetAllVolunteerReservationsResponse> GetAllVolunteerReservations
           (
           CancellationToken cancellationToken
           )
        {
            return await mediator.Send(new GetAllVolunteersReservationsRequest(), cancellationToken);
        }

        [Authorize]
        [HttpPost]
        public async Task<ReserveProductResponse> Reserve
          (
          [FromBody] ReserveProductRequest request,
          CancellationToken cancellationToken
          )
        {
            return await mediator.Send(request, cancellationToken);
        }

        [Authorize]
        [HttpPost("cancel/{id}")]
        public async Task<CancelProductReservationResponse> Cancel
          (
          [FromRoute] Guid id,
          CancellationToken cancellationToken
          )
        {
            return await mediator.Send(new CancelProductReservationRequest { ProductReservationId = id}, cancellationToken);
        }

        [Authorize]
        [HttpPost("withdraw/{id}")]
        public async Task<ConfirmWithdrawnResponse> Withdraw
          (
          [FromRoute] Guid id,
          CancellationToken cancellationToken
          )
        {
            return await mediator.Send(new ConfirmWithdrawnRequest { ProductReservationId = id }, cancellationToken);
        }
    }
}
