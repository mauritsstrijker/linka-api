using Linka.Application.Common;
using Linka.Application.Data;
using Linka.Application.Repositories;
using Linka.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linka.Application.Features.Posts.Commands
{
    public class UnlikePostRequest : IRequest<UnlikePostResponse>
    {
        public Guid Id { get; set; }
    }
    public class UnlikePostResponse;
    public class UnlikePostHandler(IPostRepository postRepository, IUserRepository userRepository, IJwtClaimService jwtClaimService, IUnitOfWork unitOfWork) : IRequestHandler<UnlikePostRequest, UnlikePostResponse>
    {
        public async Task<UnlikePostResponse> Handle(UnlikePostRequest request, CancellationToken cancellationToken)
        {
            var post = await postRepository.Get(request.Id, cancellationToken) ?? throw new Exception();
            var currentUser = await userRepository.Get(Guid.Parse(jwtClaimService.GetClaimValue("userId")), cancellationToken) ?? throw new Exception();

            var userLike = post.Likes.FirstOrDefault(like => like.User.Id == currentUser.Id);
            if (userLike == null)
            {
                throw new Exception("Usuário não curtiu este post.");
            }

            post.Likes.Remove(userLike);

            await postRepository.Update(post, cancellationToken);

            await unitOfWork.Commit(cancellationToken);

            return new UnlikePostResponse();
        }
    }
}
