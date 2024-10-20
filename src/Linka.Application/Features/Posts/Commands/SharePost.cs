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
    public class SharePostRequest : IRequest<SharePostResponse>
    {
        public Guid Id { get; set; }
    }
    public class SharePostResponse;
    public class SharePostHandler(IPostRepository postRepository, IUserRepository userRepository, IJwtClaimService jwtClaimService, IUnitOfWork unitOfWork) : IRequestHandler<SharePostRequest, SharePostResponse>
    {
        public async Task<SharePostResponse> Handle(SharePostRequest request, CancellationToken cancellationToken)
        {
            var post = await postRepository.Get(request.Id, cancellationToken) ?? throw new Exception();
            var currentUser = await userRepository.Get(Guid.Parse(jwtClaimService.GetClaimValue("id")), cancellationToken) ?? throw new Exception();

            var userAlreadyShared = post.Shares.Any(Share => Share.User.Id == currentUser.Id);
            if (userAlreadyShared)
            {
                throw new Exception("Usuário já republicou este post.");
            }

            var newShare = PostShare.Create(currentUser, post);

            post.Shares.Add(newShare);

            await postRepository.Update(post, cancellationToken);

            await unitOfWork.Commit(cancellationToken);

            return new SharePostResponse();
        }
    }
}
