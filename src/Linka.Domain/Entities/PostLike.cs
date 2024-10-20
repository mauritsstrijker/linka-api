using Linka.Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Linka.Domain.Entities
{
    public class PostLike : BaseEntity
    {
        public User User { get; set; }

        public Post Post { get; set; }

        public static PostLike Create(User user,Post post)
        {
            return new PostLike { Id = Guid.NewGuid(), User = user, Post = post };   
        }
    }
}
