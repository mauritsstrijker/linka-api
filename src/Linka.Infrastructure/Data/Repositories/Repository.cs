using Linka.Application.Common;
using Linka.Domain.Common;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;

namespace Linka.Infrastructure.Data.Repositories
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly IContext _context;

        public Repository(Context context)
        {
            _context = context;
        }

        protected virtual IQueryable<T> Query(params Expression<Func<T, object>>[] joins)
        {
            var query = _context.Set<T>().AsQueryable();

           
                query = query.IncludeAll(); 
           
          

            return query;
        }

        protected virtual IQueryable<T> Query(Expression<Func<T, object>> orderByDescending, params Expression<Func<T, object>>[] joins)
        {
            return Query(joins).OrderByDescending(orderByDescending);
        }

        public virtual async Task<IEnumerable<T>> GetAsync(CancellationToken cancellationToken, params Expression<Func<T, object>>[] joins)
        {
            return await Query(joins).ToListAsync(cancellationToken);
        }

        public virtual async Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> lambda, CancellationToken cancellationToken, params Expression<Func<T, object>>[] joins)
        {
            return await Query(joins).Where(lambda).ToListAsync(cancellationToken);
        }

        public virtual async Task<T> GetFirstAsync(Expression<Func<T, bool>> lambda, CancellationToken cancellationToken, params Expression<Func<T, object>>[] joins)
        {
            return await Query(joins).FirstOrDefaultAsync(lambda, cancellationToken);
        }

        public virtual async Task<T> GetOrderedFirstAsync(Expression<Func<T, bool>> lambda, Expression<Func<T, object>> orderByDescending, CancellationToken cancellationToken, params Expression<Func<T, object>>[] joins)
        {
            return await Query(orderByDescending, joins).FirstOrDefaultAsync(lambda, cancellationToken);
        }

        public virtual async Task<bool> ExistsAsync(Expression<Func<T, bool>> lambda, CancellationToken cancellationToken)
        {
            return await Query().AnyAsync(lambda, cancellationToken);
        }

        public virtual async Task AddAsync(T entity, CancellationToken cancellationToken)
        {
            await _context.Set<T>().AddAsync(entity, cancellationToken);
        }

        public virtual async Task AddCollectionAsync(IEnumerable<T> entities, CancellationToken cancellationToken)
        {
            await _context.Set<T>().AddRangeAsync(entities, cancellationToken);
        }

        public virtual async Task UpdateAsync(T entity, CancellationToken cancellationToken)
        {
            await Task.Run(() =>
            {
                _context.Set<T>().Update(entity);
            }, cancellationToken);
        }

        public virtual async Task UpdateCollectionAsync(IEnumerable<T> entities, Guid usuarioId, CancellationToken cancellationToken)
        {
            await Task.Run(() =>
            {
                _context.Set<T>().UpdateRange(entities);
            }, cancellationToken);
        }

        public virtual async Task RemoveAsync(T entity, CancellationToken cancellationToken)
        {
            await Task.Run(() =>
            {
                _context.Set<T>().Remove(entity);
            }, cancellationToken);
        }

        public virtual async Task RemoveAsyncByLambda(Expression<Func<T, bool>> lambda, CancellationToken cancellationToken)
        {
            var entity = await Query().FirstOrDefaultAsync(lambda, cancellationToken);
            if (entity == null) return;

            await Task.Run(() =>
            {
                _context.Set<T>().Remove(entity);
            }, cancellationToken);
        }

        public virtual async Task<long> GetCountAsync(CancellationToken cancellationToken)
        {
            return await Query().CountAsync(cancellationToken);
        }

        public virtual async Task<long> GetCountAsync(Expression<Func<T, bool>> lambda, CancellationToken cancellationToken)
        {
            return await Query().Where(lambda).CountAsync(cancellationToken);
        }

        public virtual async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        protected virtual bool IsAttached(T entity)
        {
            return _context.Set<T>().Local.Any(e => e == entity);
        }

        protected async Task<IEnumerable<T>> GetPage(Expression<Func<T, bool>> lambda, Guid lastId, int take = 1000, CancellationToken cancellationToken = default, params Expression<Func<T, object>>[] joins)
        {
            return await Query(joins)
                .OrderBy(b => b.Id)
                .Where(lambda)
                .Where(b => b.Id > lastId)
                .Take(take)
                .ToListAsync(cancellationToken);
        }

        public async IAsyncEnumerable<T> GetPagingAsync(Expression<Func<T, bool>> lambda, int take = 1000, CancellationToken cancellationToken = default, params Expression<Func<T, object>>[] joins)
        {
            var page = await GetPage(lambda, Guid.Empty, take, cancellationToken, joins);
            while (page.Count() > 0)
            {
                foreach (var item in page)
                    yield return item;

                page = await GetPage(lambda, page.Max(a => a.Id), take, cancellationToken);
            }
        }
    }
}

public static class IQueryableExtensions
{
    public static IQueryable<T> IncludeAll<T>(this IQueryable<T> query) where T : class
    {
        var entityType = typeof(T);
        var navigationProperties = entityType
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.PropertyType.IsClass && p.PropertyType != typeof(string) && !p.PropertyType.IsArray && !p.PropertyType.IsEnum);

        foreach (var property in navigationProperties)
        {
            query = query.Include(property.Name);
        }

        return query;
    }

    public static IQueryable<T> IncludeMultiple<T>(this IQueryable<T> query, params Expression<Func<T, object>>[] includes) where T : class
    {
        if (includes != null)
        {
            query = includes.Aggregate(query, (current, include) => current.Include(include));
        }

        return query;
    }
}