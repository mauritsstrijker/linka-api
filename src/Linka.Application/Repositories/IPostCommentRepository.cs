using Linka.Application.Common;
using Linka.Domain.Entities;

namespace Linka.Application.Repositories
{
    public interface IPostCommentRepository : IRepository<PostComment>
    {
        Task<List<PostComment>> GetAllByPostId(Guid postId, CancellationToken cancellationToken);
        Task<int> GetCountByPostId(Guid postId, CancellationToken cancellationToken);
    }
}
