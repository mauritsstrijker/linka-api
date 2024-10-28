using Linka.Application.Features.ConnectionRequests.Commands;
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
    }
}
