using Linka.Application.Repositories;
using Linka.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Linka.Infrastructure.Data.Repositories
{
    public class PostRepository(Context context) : Repository<Post>(context), IPostRepository
    {
        public override async Task<List<Post>> GetAll(CancellationToken cancellationToken)
        {
            return await _context.Posts
                .Include(x => x.AssociatedOrganization)
                .Include(x => x.Author)
                .ToListAsync(cancellationToken);
        }

        public override async Task<Post?> Get(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Posts
                .Include(x => x.AssociatedOrganization)
                .Include(x => x.Author)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
