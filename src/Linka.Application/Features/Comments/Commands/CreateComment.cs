using Linka.Application.Common;
using Linka.Application.Data;
using Linka.Application.Repositories;
using Linka.Domain.Entities;
using MediatR;

namespace Linka.Application.Features.Comments.Commands
{
    public class CreateCommentRequest : IRequest<CreateCommentResponse>
    {
        public string Content { get; set; }
        public Guid PostId { get; set; }
    }

    public class CreateCommentResponse;

    public class CreateCommentHandler(IPostRepository postRepository, IPostCommentRepository postCommentRepository, IUserRepository userRepository, IJwtClaimService jwtClaimService, IUnitOfWork unitOfWork) : IRequestHandler<CreateCommentRequest, CreateCommentResponse>
    {
        public async Task<CreateCommentResponse> Handle(CreateCommentRequest request, CancellationToken cancellationToken)
        {
            var post = await postRepository.Get(request.PostId, cancellationToken) ?? throw new Exception();

            var author = await userRepository.Get(Guid.Parse(jwtClaimService.GetClaimValue("userId")), cancellationToken) ?? throw new Exception();

            var comment = PostComment.Create(post, author, request.Content);

            await postCommentRepository.Insert(comment, cancellationToken);

            await unitOfWork.Commit(cancellationToken);

            return new CreateCommentResponse();
        }
    }
}
