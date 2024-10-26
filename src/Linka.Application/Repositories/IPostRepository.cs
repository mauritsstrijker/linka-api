using Linka.Application.Common;
using Linka.Domain.Entities;

namespace Linka.Application.Repositories
{
    public interface IPostRepository : IRepository<Post>
    {
        Task<List<Post>> GetAllByUserId(Guid userId, CancellationToken cancellationToken);
    }
}
