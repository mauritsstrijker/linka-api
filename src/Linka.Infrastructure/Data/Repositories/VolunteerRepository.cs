using Linka.Application.Repositories;
using Linka.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Linka.Infrastructure.Data.Repositories
{
    public class VolunteerRepository(Context context) : Repository<Volunteer>(context), IVolunteerRepository
    {
        public Task<Volunteer> GetByCPF(string cpf, CancellationToken cancellationToken)
        {
            return _context.Volunteers.FirstOrDefaultAsync(v => v.CPF == cpf, cancellationToken);
        }
    }
}
