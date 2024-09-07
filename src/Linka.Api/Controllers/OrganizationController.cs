using Linka.Application.Features.Organizations.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Linka.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrganizationController(IMediator mediator) : ControllerBase
    {
        [AllowAnonymous]
        [HttpPost("registrar")]
        public async Task<RegisterOrganizationResponse> Register
            (
            [FromBody] RegisterOrganizationRequest request,
            CancellationToken cancellationToken
            )
        {
            return await mediator.Send(request, cancellationToken);
        }
    }
}
