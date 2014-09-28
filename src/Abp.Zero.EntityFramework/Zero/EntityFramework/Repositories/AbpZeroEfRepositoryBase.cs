using Abp.Domain.Entities;
using Abp.Domain.Repositories.EntityFramework;

namespace Abp.Zero.EntityFramework.Repositories
{
    public abstract class AbpZeroEfRepositoryBase<TEntity> : AbpZeroEfRepositoryBase<TEntity, int>
        where TEntity : class, IEntity<int>
    {
        
    }

    public abstract class AbpZeroEfRepositoryBase<TEntity, TPrimaryKey> : EfRepositoryBase<AbpZeroDbContext, TEntity, TPrimaryKey> where TEntity : class, IEntity<TPrimaryKey>
    {
        
    }
}