using Linka.Domain.Enums;

namespace Linka.Application.Dtos
{
    public class PostDto
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public Guid AuthorId { get; set; }
        public UserType AuthorType { get; set; }
        public string AuthorDisplayName { get; set; }
        public Guid AssociatedOrganizationId { get; set; }
        public string? ImageBase64 { get; set; }
        public int ShareCount { get; set; }
        public int LikeCount { get; set; }
        public int CommentCount { get; set; }   
        public bool CurrentUserHasLiked { get; set; }
        public bool CurrentUserHasShared { get; set; }
    }
}
