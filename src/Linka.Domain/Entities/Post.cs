using Linka.Domain.Common;

namespace Linka.Domain.Entities
{
    public class Post : BaseEntity
    {
        public string Description { get; set; }
        public User Author { get; set; }
        public Organization AssociatedOrganization { get; set; }
        public List<PostComment> Comments { get; set; }
        public List<PostShare> Shares { get; set; }
        public List<PostLike> Likes { get; set; }
        public byte[]? ImageBytes { get; set; }

        public static Post Create(string description, User Author, Organization associatedOrganization, byte[]? imageBytes)
        {
            return new Post
            {
                Id = Guid.NewGuid(),    
                Description = description,
                Author = Author,
                AssociatedOrganization = associatedOrganization,
                ImageBytes = imageBytes
            };
        }

    }
}
