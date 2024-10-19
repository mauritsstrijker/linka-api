using Linka.Application.Dtos;
using Linka.Application.Features.Posts.Commands;
using Linka.Application.Features.Posts.Queries;
using Linka.Application.Features.Users.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Linka.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PostController(IMediator mediator) : ControllerBase
    {
        [Authorize]
        [HttpPost]
        public async Task<CreatePostResponse> Create
            (
            [FromBody] CreatePostRequest request,
            CancellationToken cancellationToken
            )
        {
            return await mediator.Send(request, cancellationToken);
        }

        [Authorize]
        [HttpGet]
        public async Task<ICollection<PostDto>> GetAll
            (
            CancellationToken cancellationToken
            )
        {
            return await mediator.Send(new GetAllPostRequest(), cancellationToken);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<DeletePostResponse> Delete
            (
            [FromRoute] Guid id,
            CancellationToken cancellationToken
            )
        {
            return await mediator.Send(new DeletePostRequest { Id = id }, cancellationToken);
        }

    }
}
