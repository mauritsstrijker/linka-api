﻿using Linka.Application.Repositories;
using Linka.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Linka.Infrastructure.Data.Repositories;
public class ConnectionRepository(Context context) : Repository<Connection>(context), IConnectionRepository
{
    public async Task<bool> HasConnectionAsync(Guid volunteerId1, Guid volunteerId2, CancellationToken cancellationToken)
    {
        return await _context.Connections
            .AnyAsync(c =>
                (c.Volunteer1.Id == volunteerId1 && c.Volunteer2.Id == volunteerId2) ||
                (c.Volunteer1.Id == volunteerId2 && c.Volunteer2.Id == volunteerId1),
                cancellationToken);
    }
}
