using Linka.Application.Features.Users.Commands;
using Linka.Application.Features.Users.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Linka.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController(IMediator mediator) : ControllerBase
    {
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<AuthenticateResponse> Login
            (
            [FromBody] AuthenticateRequest request,
            CancellationToken cancellationToken
            )
        {
            return await mediator.Send(request, cancellationToken);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<GetUserByIdResponse> GetById
            (
            CancellationToken cancellationToken,
            [FromRoute] Guid id
            )
        {
            return await mediator.Send(new GetUserByIdRequest { Id = id }, cancellationToken);
        }
    }
}
