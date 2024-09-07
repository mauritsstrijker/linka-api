using Linka.Application.Common;
using Linka.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Linka.Infrastructure.Data.Repositories;

public class Repository<TEntity, TContext> : IRepository<TEntity> where TEntity : BaseEntity
    where TContext : DbContext
    {
        protected readonly TContext _context;
        public Repository(TContext context)
        {
            _context = context;
        }
        protected virtual IQueryable<TEntity> Query()
        {
            return _context.Set<TEntity>();
        }
        public virtual Task Insert(TEntity entity, CancellationToken cancellationToken)
        {
            entity.DateCreated = DateTime.Now;
            _context.Add(entity);

            return Task.CompletedTask;
        }
        public virtual Task Update(TEntity entity, CancellationToken cancellationToken)
        {
            _context.Update(entity);
            return Task.CompletedTask;
        }
        public virtual Task Delete(TEntity? entity, CancellationToken cancellationToken)
        {
            if (entity != null)
                _context.Remove(entity);

            return Task.CompletedTask;
        }
        public virtual async Task<TEntity?> Get(Guid id, CancellationToken cancellationToken)
        {
            return await Query().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }
        public virtual async Task<List<TEntity>> GetAll(CancellationToken cancellationToken)
        {
            return await Query().ToListAsync(cancellationToken);
        }

        public virtual async Task<bool> Exists(Guid id, CancellationToken cancellationToken)
        {
            return await Query().AnyAsync(x => x.Id == id, cancellationToken);
        }
    }
