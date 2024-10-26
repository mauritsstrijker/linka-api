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

        [Authorize]
        [HttpPost("{id}/like")]
        public async Task<LikePostResponse> Like
            (
            [FromRoute] Guid id,
            CancellationToken cancellationToken
            )
        {
            return await mediator.Send(new LikePostRequest { Id = id }, cancellationToken); 
        }

        [Authorize]
        [HttpPost("{id}/unlike")]
        public async Task<UnlikePostResponse> Unlike
            (
            [FromRoute] Guid id,
            CancellationToken cancellationToken
            )
        {
            return await mediator.Send(new UnlikePostRequest { Id = id }, cancellationToken);
        }

        [Authorize]
        [HttpPost("{id}/share")]
        public async Task<SharePostResponse> Share
            (
            [FromRoute] Guid id,
            CancellationToken cancellationToken
            )
        {
            return await mediator.Send(new SharePostRequest { Id = id }, cancellationToken); 
        }

        [Authorize]
        [HttpPost("{id}/unshare")]
        public async Task<UnsharePostResponse> Unshare
            (
            [FromRoute] Guid id,
            CancellationToken cancellationToken
            )
        {
            return await mediator.Send(new UnsharePostRequest { Id = id }, cancellationToken);
        }

        [Authorize]
        [HttpGet("user/{userId}")]
        public async Task<ICollection<PostDto>> GetAll
            (
            [FromRoute] Guid userId,
            CancellationToken cancellationToken
            )
        {
            return await mediator.Send(new GetAllPostsByUserIdRequest { UserId = userId }, cancellationToken);
        }
    }
}
