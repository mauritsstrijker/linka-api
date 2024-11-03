using Azure.Core;
using Linka.Application.Features.Events.Queries;
using Linka.Application.Features.Organizations.Commands;
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

        [Authorize]
        [HttpGet("{id}/events")]
        public async Task<List<GetAllEventByVolunteerIdResponse>> GetEventsByVolunteerId
            (
            CancellationToken cancellationToken,
            [FromRoute] Guid id    
            )
        {
            return await mediator.Send(new GetAllEventByVolunteerIdRequest(id), cancellationToken);
        }

        [Authorize]
        [HttpPatch("update")]
        public async Task<UpdateVolunteerResponse> UpdateVolunteer
            (
            CancellationToken cancellationToken,
            [FromBody] UpdateVolunteerRequest request
            )
        {
            return await mediator.Send(request, cancellationToken);
        }


        [Authorize]
        [HttpPost("remove-avatar")]
        public async Task<RemoveVolunteerAvatarResponse> RemoveAvatar
          (
          CancellationToken cancellationToken
          )
        {
            return await mediator.Send(new RemoveVolunteerAvatarRequest(), cancellationToken);
        }
    }
}
