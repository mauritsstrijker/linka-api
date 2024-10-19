using Linka.Application.Common;
using Linka.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linka.Application.Repositories
{
    public interface IPostCommentRepository : IRepository<PostComment>
    {
        Task<List<PostComment>> GetAllByPostId(Guid postId, CancellationToken cancellationToken);
    }
}
