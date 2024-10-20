using Linka.Application.Common;
using Linka.Application.Data;
using Linka.Application.Repositories;
using Linka.Domain.Entities;
using MediatR;

namespace Linka.Application.Features.Posts.Commands
{
    public class LikePostRequest : IRequest<LikePostResponse>
    {
        public Guid Id { get; set; }
    }
    public class LikePostResponse;
    public class LikePostHandler(IPostRepository postRepository, IUserRepository userRepository, IJwtClaimService jwtClaimService, IUnitOfWork unitOfWork) : IRequestHandler<LikePostRequest, LikePostResponse>
    {
        public async Task<LikePostResponse> Handle(LikePostRequest request, CancellationToken cancellationToken)
        {
            var post = await postRepository.Get(request.Id, cancellationToken) ?? throw new Exception();
            var currentUser = await userRepository.Get(Guid.Parse(jwtClaimService.GetClaimValue("id")), cancellationToken) ?? throw new Exception();

            var userAlreadyLiked = post.Likes.Any(like => like.User.Id == currentUser.Id);
            if (userAlreadyLiked)
            {
                throw new Exception("Usuário já curtiu este post.");
            }

            var newLike = PostLike.Create(currentUser, post);

            post.Likes.Add(newLike);

            await postRepository.Update(post, cancellationToken);

            await unitOfWork.Commit(cancellationToken);

            return new LikePostResponse();
        }
    }
}
