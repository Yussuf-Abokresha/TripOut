using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Modules.Users.Application.Repositories
{
    public interface IRepository<TEntity>
        where TEntity : class
    {
        public void Add(TEntity entity);
        public void Update(TEntity entity);
        public void Delete(TEntity entity);
        public void Attach(TEntity entity);
        public Task<TEntity?> GetFirstOrDefaultByFilter(Expression<Func<TEntity, bool>> filter, params Func<IQueryable<TEntity>, IQueryable<TEntity>>[] includes);
        public Task<TEntity?> GetByIdForUpdate<Tid>(Tid id);
        public Task<ICollection<TEntity>> GetByExpWhereAsync(params Expression<Func<TEntity, bool>>[] filters);
        public IQueryable<TEntity> GetQueryable();
    }
}