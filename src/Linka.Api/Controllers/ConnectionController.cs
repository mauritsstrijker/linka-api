using Linka.Application.Features.ConnectionRequests.Commands;
using Linka.Application.Features.ConnectionRequests.Queries;
using Linka.Application.Features.Connections;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Linka.Api.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class ConnectionController(IMediator mediator) : ControllerBase
    {
        [Authorize]
        [HttpPost("request")]
        public async Task<AskForConnectionResponse> AskForConnection
            (
            [FromBody] AskForConnectionRequest request,
            CancellationToken cancellationToken
            )
        {
            return await mediator.Send(request, cancellationToken);
        }

        [Authorize]
        [HttpGet("check-pending/{volunteerId}")]
        public async Task<CheckPendingRequestResponse> CheckPending
            (
            [FromRoute] Guid volunteerId,
            CancellationToken cancellationToken
            )
        {
            return await mediator.Send(new CheckPendingRequestRequest{ VolunteerId = volunteerId }, cancellationToken);
        }

        [Authorize]
        [HttpGet("check/{volunteerId}")]
        public async Task<CheckConnectionResponse> Check
            (
            [FromRoute] Guid volunteerId,
            CancellationToken cancellationToken
            )
        {
            return await mediator.Send(new CheckConnectionRequest { VolunteerId = volunteerId }, cancellationToken);
        }

        [Authorize]
        [HttpPost("accept")]
        public async Task<AcceptConnectionResponse> AcceptConnectionRequest
            (
            [FromBody] AcceptConnectionRequest request,
            CancellationToken cancellationToken
            )
        {
            return await mediator.Send(request, cancellationToken);
        }

        [Authorize]
        [HttpPost("reject")]
        public async Task<RejectConnectionResponse> RejectConnectionRequest
           (
           [FromBody] RejectConnectionRequest request,
           CancellationToken cancellationToken
           )
        {
            return await mediator.Send(request, cancellationToken);
        }

        [Authorize]
        [HttpGet("pending")]
        public async Task<GetAllConnectionRequestResponse> GetAllPendingRequests
            (
            CancellationToken cancellationToken
            )
        {
            return await mediator.Send(new GetAllConnectionRequest(), cancellationToken);
        }
        [Authorize]
        [HttpPost("break-up")]
        public async Task<BreakUpConnectionResponse> BreakUpConnection
           (
           [FromBody] BreakUpConnectionRequest request,
           CancellationToken cancellationToken
           )
        {
            return await mediator.Send(request, cancellationToken);
        }
    }
}
