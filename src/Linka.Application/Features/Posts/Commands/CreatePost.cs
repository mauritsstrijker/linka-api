using Linka.Application.Common;
using Linka.Application.Data;
using Linka.Application.Repositories;
using Linka.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Linka.Application.Features.Posts.Commands
{
    public class CreatePostRequest : IRequest<CreatePostResponse>
    {
        public string Description { get; set; }
        public Guid AssociatedOrganizationId { get; set; }
        public string? ImageBase64 { get; set; }
    }

    public class CreatePostResponse;

    public class CreatePostHandler
        (
        IUserRepository userRepository,
        IOrganizationRepository organizationRepository,
        IJwtClaimService jwtClaimService,
        IPostRepository postRepository,
        IUnitOfWork unitOfWork
        ) : IRequestHandler<CreatePostRequest, CreatePostResponse>
    {
        public async Task<CreatePostResponse> Handle(CreatePostRequest request, CancellationToken cancellationToken)
        {
            var authorId = jwtClaimService.GetClaimValue("userId");

            var author = await userRepository.Get(Guid.Parse(authorId), cancellationToken) ?? throw new Exception();

            var associatedOrganization = await organizationRepository.Get(request.AssociatedOrganizationId, cancellationToken);

            var imageBytes = string.IsNullOrEmpty(request.ImageBase64) ? null : Convert.FromBase64String(request.ImageBase64);

            var post = Post.Create(request.Description, author, associatedOrganization, imageBytes);

            await postRepository.Insert(post, cancellationToken);

            await unitOfWork.Commit(cancellationToken);

            return new CreatePostResponse();
        }
    }
}
