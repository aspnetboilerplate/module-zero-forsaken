using Abp.MultiTenancy;
using Abp.NHibernate.Repositories;

namespace Abp.Zero.NHibernate.Repositories
{
    public class AbpTenantRepository : NhRepositoryBase<AbpTenant>, IAbpTenantRepository
    {

    }
}