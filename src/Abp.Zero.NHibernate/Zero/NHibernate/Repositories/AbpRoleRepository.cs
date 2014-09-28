using Abp.Authorization.Roles;
using Abp.NHibernate.Repositories;

namespace Abp.Zero.NHibernate.Repositories
{
    public class AbpRoleRepository : NhRepositoryBase<AbpRole>, IAbpRoleRepository
    {
  
    }
}