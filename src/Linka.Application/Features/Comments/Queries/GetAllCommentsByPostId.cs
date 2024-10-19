using Linka.Application.Dtos;
using Linka.Application.Repositories;
using Linka.Domain.Entities;
using Linka.Domain.Enums;
using MediatR;

namespace Linka.Application.Features.Comments.Queries
{
    public class GetAllCommentsByPostIdRequest : IRequest<ICollection<CommentDto>>
    {
        public Guid PostId { get; set; }
    }

    public class GetAllCommentsByPostIdHandler(IPostCommentRepository postCommentRepository, IVolunteerRepository volunteerRepository, IOrganizationRepository organizationRepository) : IRequestHandler<GetAllCommentsByPostIdRequest, ICollection<CommentDto>>
    {
        public async Task<ICollection<CommentDto>> Handle(GetAllCommentsByPostIdRequest request, CancellationToken cancellationToken)
        {
            var comments = await postCommentRepository.GetAllByPostId(request.PostId, cancellationToken);

            return await MapCommentsToDto(comments, cancellationToken);
        }

        private async Task<ICollection<CommentDto>> MapCommentsToDto(List<PostComment> comments, CancellationToken cancellationToken)
        {
            List<CommentDto> response = [];

            foreach(var comment in comments) 
            {
                Guid authorId;
                string authorDisplayName;
                if (comment.Author.Type == UserType.Volunteer)
                {
                    var volunteer = await volunteerRepository.GetByUserId(comment.Author.Id, cancellationToken);
                    authorDisplayName = volunteer.FullName;
                    authorId = volunteer.Id;
                } else
                {
                    var organization = await organizationRepository.GetByUserId(comment.Author.Id, cancellationToken);
                    authorDisplayName = organization.TradingName;
                    authorId = organization.Id;
                }
                response.Add(new CommentDto
                {
                    Content = comment.Content,
                    AuthorId = authorId,
                    Type = comment.Author.Type,
                    AuthorDisplayName = authorDisplayName
                });
            }

            return response;
        }
    }
}
