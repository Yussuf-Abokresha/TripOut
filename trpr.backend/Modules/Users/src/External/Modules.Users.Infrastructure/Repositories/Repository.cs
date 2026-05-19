using System.Linq.Expressions;
using Common.Application;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Modules.Users.Application.Repositories;
using Modules.Users.Infrastructure.Data;
using Quartz.Xml.JobSchedulingData20;

namespace Modules.Users.Infrastructure.Repositories
{
    public class Repository<TEntity>(UsersDbContext context) : IRepository<TEntity>
        where TEntity : class
    {
        public void Add(TEntity entity)
        {
            context.Set<TEntity>().Add(entity);
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

        public async Task<TEntity?> GetByIdForUpdate<Tid>(Tid id)
        {
            if (context.Database.CurrentTransaction == null)
                throw new InvalidOperationException("FOR UPDATE requires an active transaction.");

            var entityType = context.Model.FindEntityType(typeof(TEntity));

            var tableName = entityType!.GetTableName();
            var schemaName = entityType!.GetSchema();
            var keyProperty = entityType!.FindPrimaryKey()?.Properties.FirstOrDefault();
            var storeObject = StoreObjectIdentifier.Create(entityType, StoreObjectType.Table);
            var keyColumnName = keyProperty!.GetColumnName(storeObject!.Value);

            var sql = $"SELECT * FROM \"{schemaName}\".\"{tableName}\" WHERE \"{keyColumnName}\" = @p0 FOR UPDATE";

            var entity = await context.Set<TEntity>().FromSqlRaw(sql, id).FirstOrDefaultAsync();

            return entity;
        }
    }
}