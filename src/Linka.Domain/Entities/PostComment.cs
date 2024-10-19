using Linka.Domain.Common;

namespace Linka.Domain.Entities
{
    public class PostComment : BaseEntity
    {
        public Post Post { get; set; }
        public User Author { get; set; }
        public string Content { get; set; }
        public static PostComment Create(Post post, User author, string content)
        {
            return new PostComment { Id = Guid.NewGuid(), Post = post, Author = author, Content = content };
        }
    }
}
