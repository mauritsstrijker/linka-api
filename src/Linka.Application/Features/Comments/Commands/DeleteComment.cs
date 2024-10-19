using Linka.Application.Common;
using Linka.Application.Data;
using Linka.Application.Repositories;
using MediatR;

namespace Linka.Application.Features.Comments.Commands
{
    public class DeleteCommentRequest : IRequest<DeleteCommentResponse>
    {
        public Guid Id { get; set; }
    }

    public class DeleteCommentResponse;

    public class DeleteCommentHandler(IPostCommentRepository postCommentRepository, IUserRepository userRepository, IJwtClaimService jwtClaimService, IUnitOfWork unitOfWork) : IRequestHandler<DeleteCommentRequest, DeleteCommentResponse>
    {
        public async Task<DeleteCommentResponse> Handle(DeleteCommentRequest request, CancellationToken cancellationToken)
        {
            var currentUserId = jwtClaimService.GetClaimValue("userId");

            var comment = await postCommentRepository.Get(request.Id, cancellationToken) ?? throw new Exception();

            if (comment.Author.Id == Guid.Parse(currentUserId))
            {
                await postCommentRepository.Delete(comment, cancellationToken);
                await unitOfWork.Commit(cancellationToken);
            }

            return new DeleteCommentResponse(); 
        }
    }
}
