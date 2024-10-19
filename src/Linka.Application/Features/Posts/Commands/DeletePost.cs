using Linka.Application.Common;
using Linka.Application.Data;
using Linka.Application.Repositories;
using MediatR;

namespace Linka.Application.Features.Posts.Commands
{
    public class DeletePostRequest : IRequest<DeletePostResponse>
    {
        public Guid Id { get; set; }
    }

    public class DeletePostResponse;

    public class DeletePostHandler(IPostRepository postRepository, IJwtClaimService jwtClaimService, IUnitOfWork unitOfWork) : IRequestHandler<DeletePostRequest, DeletePostResponse>
    {
        public async Task<DeletePostResponse> Handle(DeletePostRequest request, CancellationToken cancellationToken)
        {
            var post = await postRepository.Get(request.Id, cancellationToken) ?? throw new Exception();

            var currentUserId = jwtClaimService.GetClaimValue("userId");

            if (Guid.Parse(currentUserId) == post.Author.Id) 
            {
                await postRepository.Delete(post, cancellationToken);
                await unitOfWork.Commit(cancellationToken);
                return new DeletePostResponse();
            }

            throw new Exception();
        }
    }
}
