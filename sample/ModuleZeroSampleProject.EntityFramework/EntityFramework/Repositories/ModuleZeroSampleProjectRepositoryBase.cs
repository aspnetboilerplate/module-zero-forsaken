using Abp.Domain.Entities;
using Abp.EntityFramework.Repositories;

namespace ModuleZeroSampleProject.EntityFramework.Repositories
{
    public abstract class ModuleZeroSampleProjectRepositoryBase<TEntity, TPrimaryKey> : EfRepositoryBase<ModuleZeroSampleProjectDbContext, TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {
    }

    public abstract class ModuleZeroSampleProjectRepositoryBase<TEntity> : ModuleZeroSampleProjectRepositoryBase<TEntity, int>
        where TEntity : class, IEntity<int>
    {

    }
}
