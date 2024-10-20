using Linka.Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Linka.Domain.Entities
{
    public class PostShare : BaseEntity
    {
        public User User { get; set; }

        public Post Post { get; set; }
        public static PostShare Create(User user, Post post)
        {
            return new PostShare { Id = Guid.NewGuid(), User = user, Post = post };
        }
    }
}
