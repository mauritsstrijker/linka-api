using Linka.Application.Repositories;
using Linka.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Linka.Infrastructure.Data.Repositories
{
    public class VolunteerRepository(Context context) : Repository<Volunteer>(context), IVolunteerRepository
    {
        public override Task<Volunteer> Get(Guid id, CancellationToken cancellationToken)
        {
            return _context.Volunteers
                .Include(x => x.User)
                .Include(x => x.Address)
                .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        }
        public Task<Volunteer> GetByCPF(string cpf, CancellationToken cancellationToken)
        {
            return _context.Volunteers.FirstOrDefaultAsync(v => v.CPF == cpf, cancellationToken);
        }

        public Task<Volunteer> GetByUserId(Guid userId, CancellationToken cancellationToken)
        {
            return _context.Volunteers.FirstOrDefaultAsync(v => v.User.Id == userId, cancellationToken);
        }
    }
}
