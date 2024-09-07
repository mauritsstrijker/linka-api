using Linka.Application.Features.Volunteers.Commands;
using Linka.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Linka.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VolunteerController(IMediator mediator) : ControllerBase
    {
        [HttpPost("registrar")]
        public async Task<RegisterVolunteerResponse> Register
            (
            [FromBody] RegisterVolunteerRequest request,
            CancellationToken cancellationToken
            )
        {
            return await mediator.Send(request, cancellationToken);
        }
    }
}
