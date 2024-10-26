using Linka.Application.Features.Organizations.Commands;
using Linka.Application.Features.Organizations.Queries;
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

        [Authorize]
        [HttpGet("{Id}")]
        public async Task<GetOrganizationByIdResponse> Get
            (
            [FromRoute] Guid Id,
            CancellationToken cancellationToken
            )
        {
            return await mediator.Send(new GetOrganizationByIdRequest(Id), cancellationToken);
        }

        [Authorize]
        [HttpPost("{id}/follow")]
        public async Task<FollowOrganizationResponse> Follow
        (
        [FromRoute] Guid id,
        CancellationToken cancellationToken
        )
        {
            return await mediator.Send(new FollowOrganizationRequest { OrganizationId = id }, cancellationToken);
        }

        [Authorize]
        [HttpPost("{id}/unfollow")]
        public async Task<UnfollowOrganizationResponse> Unfollow
        (
        [FromRoute] Guid id,
        CancellationToken cancellationToken
        )
        {
            return await mediator.Send(new UnfollowOrganizationRequest { OrganizationId = id }, cancellationToken);
        }
    }
}
