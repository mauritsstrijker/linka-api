﻿using Linka.Application.Common;
using Linka.Application.Dtos;
using Linka.Application.Repositories;
using Linka.Domain.Entities;
using Linka.Domain.Enums;
using MediatR;
using System.Threading;

namespace Linka.Application.Features.Posts.Queries
{
    public class GetAllPostRequest : IRequest<ICollection<PostDto>>
    {
    }

    public class GetAllPostHandler(IPostRepository postRepository, IFeedService feedService, IJwtClaimService jwtClaimService,IPostCommentRepository postCommentRepository, IVolunteerRepository volunteerRepository, IOrganizationRepository organizationRepository) : IRequestHandler<GetAllPostRequest, ICollection<PostDto>>
    {
        public async Task<ICollection<PostDto>> Handle(GetAllPostRequest request, CancellationToken cancellationToken)
        {
            var userType = jwtClaimService.GetClaimValue("type");
            List<Post> posts = [];

            if (userType == "volunteer")
            {
                posts = await feedService.GetFeedForVolunteerAsync(Guid.Parse(jwtClaimService.GetClaimValue("id")));
            } 
            else
            {
                posts = await feedService.GetOrganizationFeed(Guid.Parse(jwtClaimService.GetClaimValue("id")));
            }

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
                    CommentCount = commentCount,
                    CurrentUserHasLiked = currentUserHasLiked,
                    CurrentUserHasShared = currentUserHasShared,    
                    DateCreated = post.DateCreated,
                });
            }

            return postDtos;
        }
    }
}
