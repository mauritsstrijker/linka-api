using Linka.Application.Dtos;
using Linka.Application.Features.Comments.Commands;
using Linka.Application.Features.Comments.Queries;
using Linka.Application.Features.Posts.Commands;
using Linka.Application.Features.Posts.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Linka.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CommentController(IMediator mediator) : ControllerBase
    {
        [Authorize]
        [HttpPost]
        public async Task<CreateCommentResponse> Create
            (
            [FromBody] CreateCommentRequest request,
            CancellationToken cancellationToken
            )
        {
            return await mediator.Send(request, cancellationToken);
        }

        [Authorize]
        [HttpGet("posts/{postId}")]
        public async Task<ICollection<CommentDto>> GetAllByPostId
            (
            [FromRoute] Guid postId,
            CancellationToken cancellationToken
            )
        {
            return await mediator.Send(new GetAllCommentsByPostIdRequest { PostId = postId }, cancellationToken);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<DeleteCommentResponse> Delete
            (
            [FromRoute] Guid id,
            CancellationToken cancellationToken
            )
        {
            return await mediator.Send(new DeleteCommentRequest { Id = id }, cancellationToken);
        }

    }
}
