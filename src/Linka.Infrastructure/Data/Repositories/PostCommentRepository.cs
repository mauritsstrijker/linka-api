using Linka.Application.Repositories;
using Linka.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linka.Infrastructure.Data.Repositories
{
    public class PostCommentRepository(Context context) : Repository<PostComment>(context), IPostCommentRepository
    {
        public override async Task<PostComment?> Get(Guid id, CancellationToken cancellationToken)
        {
            return await _context.PostComments
                .Include(x => x.Post)
                .Include(x => x.Author)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<List<PostComment>> GetAllByPostId(Guid postId, CancellationToken cancellationToken)
        {
            return await _context.PostComments
                .Include(x => x.Author)
                .Include(x => x.Post)
                .Where(x => x.Post.Id == postId)
                .ToListAsync(cancellationToken);
        }
    }
}
