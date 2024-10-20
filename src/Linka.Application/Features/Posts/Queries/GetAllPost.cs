using Linka.Application.Dtos;
using Linka.Application.Repositories;
using Linka.Domain.Entities;
using MediatR;

namespace Linka.Application.Features.Posts.Queries
{
    public class GetAllPostRequest : IRequest<ICollection<PostDto>>
    {
    }

    public class GetAllPostHandler(IPostRepository postRepository) : IRequestHandler<GetAllPostRequest, ICollection<PostDto>>
    {
        public async Task<ICollection<PostDto>> Handle(GetAllPostRequest request, CancellationToken cancellationToken)
        {
            var posts = await postRepository.GetAll(cancellationToken);

            return MapPostsToDto(posts);
        }

        private ICollection<PostDto> MapPostsToDto(List<Post> posts)
        {
            return posts.Select(x => new PostDto
            {
                Description = x.Description,
                AuthorId = x.Author.Id, 
                AssociatedOrganizationId = x.AssociatedOrganization.Id, 
                ImageBase64 = x.ImageBytes != null ? Convert.ToBase64String(x.ImageBytes) : null,
                ShareCount = x.Shares.Count,
                LikeCount = x.Likes.Count
            }).ToList();
        }
    }
}
