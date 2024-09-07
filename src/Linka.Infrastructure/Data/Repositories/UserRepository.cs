using Linka.Application.Repositories;
using Linka.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Linka.Infrastructure.Data.Repositories
{
    public class UserRepository(Context context) : Repository<User>(context), IUserRepository
    {
        public Task<User> GetByUsername(string username, CancellationToken cancellationToken)
        {
            return _context.Users.FirstOrDefaultAsync(u => u.Username == username, cancellationToken);
        }
        public Task<User> GetByEmail(string email, CancellationToken cancellationToken)
        {
            return _context.Users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
        }
    }
}
