using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Modules.Trips.Application.Repositories;
using Modules.Trips.Infrastructure.Data;

namespace Modules.Trips.Infrastructure.Repositories
{
    public class Repository<TEntity>(TripsDbContext context) : IRepository<TEntity>
        where TEntity : class
    {
        public void Add(TEntity entity)
        {
            context.Set<TEntity>().Add(entity);
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            context.Set<TEntity>().AddRange(entities);
        }

        public void Delete(TEntity entity)
        {
            context.Set<TEntity>().Remove(entity);
        }

        public void Attach(TEntity entity)
        {
            context.Set<TEntity>().Attach(entity);
        }

        public async Task<ICollection<TEntity>> GetByExpWhereAsync(params Expression<Func<TEntity, bool>>[] filters)
        {
            IQueryable<TEntity> query = context.Set<TEntity>();
            foreach (var filter in filters)
                query = query.Where(filter);
            return await query.ToListAsync();
        }

        public Task<TEntity?> GetFirstOrDefaultByFilter(Expression<Func<TEntity, bool>> filter, params Func<IQueryable<TEntity>, IQueryable<TEntity>>[] includes)
        {
            var query = context.Set<TEntity>().Where(filter);
            foreach (var include in includes)
                query = include(query);
            return query.FirstOrDefaultAsync();
        }

        public IQueryable<TEntity> GetQueryable()
        {
            return context.Set<TEntity>().AsQueryable();
        }

        public void Update(TEntity entity)
        {
            context.Set<TEntity>().Update(entity);
        }
    }
}