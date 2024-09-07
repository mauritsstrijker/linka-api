using Linka.Application.Common;
using Linka.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Linka.Infrastructure.Data.Repositories;

public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly Context _context;
        public Repository(Context context)
        {
            _context = context;
        }
        protected virtual IQueryable<T> Query()
        {
            return _context.Set<T>();
        }
        public virtual Task Insert(T entity, CancellationToken cancellationToken)
        {
            entity.DateCreated = DateTime.Now;
            _context.Add(entity);

            return Task.CompletedTask;
        }
        public virtual Task Update(T entity, CancellationToken cancellationToken)
        {
            _context.Update(entity);
            return Task.CompletedTask;
        }
        public virtual Task Delete(T? entity, CancellationToken cancellationToken)
        {
            if (entity != null)
                _context.Remove(entity);

            return Task.CompletedTask;
        }
        public virtual async Task<T?> Get(Guid id, CancellationToken cancellationToken)
        {
            return await Query().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }
        public virtual async Task<List<T>> GetAll(CancellationToken cancellationToken)
        {
            return await Query().ToListAsync(cancellationToken);
        }

        public virtual async Task<bool> Exists(Guid id, CancellationToken cancellationToken)
        {
            return await Query().AnyAsync(x => x.Id == id, cancellationToken);
        }
    }
