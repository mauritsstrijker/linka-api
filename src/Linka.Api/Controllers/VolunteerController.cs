using Azure.Core;
using Linka.Application.Features.Volunteers.Commands;
using Linka.Application.Features.Volunteers.Queries;
using Linka.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Linka.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VolunteerController(IMediator mediator) : ControllerBase
    {
        [AllowAnonymous]
        [HttpPost("registrar")]
        public async Task<RegisterVolunteerResponse> Register
            (
            [FromBody] RegisterVolunteerRequest request,
            CancellationToken cancellationToken
            )
        {
            return await mediator.Send(request, cancellationToken);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<GetVolunteerByIdResponse> GetVolunteerById
            (
            [FromRoute] Guid id,
            CancellationToken cancellationToken
            )
        {
            return await mediator.Send(new GetVolunteerByIdRequest(id), cancellationToken);
        }
    }
}
