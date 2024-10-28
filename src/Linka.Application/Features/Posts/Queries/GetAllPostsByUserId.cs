using Linka.Application.Common;
using Linka.Application.Dtos;
using Linka.Application.Repositories;
using Linka.Domain.Entities;
using Linka.Domain.Enums;
using MediatR;
using System.Threading;

namespace Linka.Application.Features.Posts.Queries
{
    public class GetAllPostsByUserIdRequest : IRequest<ICollection<PostDto>>
    {
        public Guid UserId { get; set; }
    }

    public class GetAllPostsByUserIdHandler(IPostRepository postRepository, IJwtClaimService jwtClaimService, IPostCommentRepository postCommentRepository, IVolunteerRepository volunteerRepository, IOrganizationRepository organizationRepository) : IRequestHandler<GetAllPostsByUserIdRequest, ICollection<PostDto>>
    {
        public async Task<ICollection<PostDto>> Handle(GetAllPostsByUserIdRequest request, CancellationToken cancellationToken)
        {
            var posts = await postRepository.GetAllByUserId(request.UserId, cancellationToken);

            return await MapPostsToDto(posts, cancellationToken);
        }

        private async Task<ICollection<PostDto>> MapPostsToDto(List<Post> posts, CancellationToken cancellationToken)
        {
            var postDtos = new List<PostDto>();

            foreach (var post in posts)
            {
                var commentCount = await postCommentRepository.GetCountByPostId(post.Id, cancellationToken);

                Guid authorId;
                string authorDisplayName;
                if (post.Author.Type == UserType.Volunteer)
                {
                    var volunteer = await volunteerRepository.GetByUserId(post.Author.Id, cancellationToken);
                    authorDisplayName = volunteer.FullName;
                    authorId = volunteer.Id;
                }
                else
                {
                    var organization = await organizationRepository.GetByUserId(post.Author.Id, cancellationToken);
                    authorDisplayName = organization.TradingName;
                    authorId = organization.Id;
                }

                var currentUserId = Guid.Parse(jwtClaimService.GetClaimValue("userId"));

                var currentUserHasLiked = post.Likes.Any(like => like.User.Id == currentUserId);
                var currentUserHasShared = post.Shares.Any(share => share.User.Id == currentUserId);

                postDtos.Add(new PostDto
                {
                    Id = post.Id,
                    Description = post.Description,
                    AuthorId = authorId,
                    AuthorDisplayName = authorDisplayName,
                    AuthorType = post.Author.Type,
                    AssociatedOrganizationId = post.AssociatedOrganization.Id,
                    ImageBase64 = post.ImageBytes != null ? Convert.ToBase64String(post.ImageBytes) : null,
                    ShareCount = post.Shares.Count,
                    LikeCount = post.Likes.Count,
                    CommentCount = commentCount
                });
            }

            return postDtos;
        }
    }
}
