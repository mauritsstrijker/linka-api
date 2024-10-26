using Linka.Application.Common;
using Linka.Application.Data;
using Linka.Application.Repositories;
using Linka.Domain.Entities;
using MediatR;

namespace Linka.Application.Features.Posts.Commands
{
    public class UnsharePostRequest : IRequest<UnsharePostResponse>
    {
        public Guid Id { get; set; }
    }
    public class UnsharePostResponse;
    public class UnsharePostHandler(IPostRepository postRepository, IUserRepository userRepository, IJwtClaimService jwtClaimService, IUnitOfWork unitOfWork, IRepository<PostShare> shareRepository) : IRequestHandler<UnsharePostRequest, UnsharePostResponse>
    {
        public async Task<UnsharePostResponse> Handle(UnsharePostRequest request, CancellationToken cancellationToken)
        {
            var post = await postRepository.Get(request.Id, cancellationToken) ?? throw new Exception();
            var currentUser = await userRepository.Get(Guid.Parse(jwtClaimService.GetClaimValue("userId")), cancellationToken) ?? throw new Exception();

            var userShare = post.Shares.FirstOrDefault(Share => Share.User.Id == currentUser.Id);
            if (userShare == null)
            {
                throw new Exception("Usuário não republicou este post.");
            }

            post.Shares.Remove(userShare);

            await postRepository.Update(post, cancellationToken);

            await unitOfWork.Commit(cancellationToken);

            return new UnsharePostResponse();
        }
    }
}
