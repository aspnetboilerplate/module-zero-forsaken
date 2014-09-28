using Abp.Authorization.Roles;
using Abp.Domain.Repositories.NHibernate;

namespace Abp.Zero.NHibernate.Repositories
{
    public class AbpRoleRepository : NhRepositoryBase<AbpRole>, IAbpRoleRepository
    {
  
    }
}