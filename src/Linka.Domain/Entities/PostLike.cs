using Linka.Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Linka.Domain.Entities
{
    public class PostLike : BaseEntity
    {
        public User User { get; set; }

        public Post Post { get; set; }
    }
}
