using Microsoft.Extensions.DependencyInjection;

namespace Modules.Users.Application.Repositories
{
    public class RepositoryFactory(IServiceProvider serviceProvider)
    {
        public IRepository<TEntity> Repository<TEntity>() where TEntity : class
        {
            return serviceProvider.GetRequiredService<IRepository<TEntity>>();
        }
    }
}