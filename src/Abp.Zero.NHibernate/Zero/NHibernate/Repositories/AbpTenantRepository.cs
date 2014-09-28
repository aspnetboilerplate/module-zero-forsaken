using Abp.Domain.Repositories.NHibernate;
using Abp.MultiTenancy;

namespace Abp.Zero.NHibernate.Repositories
{
    public class AbpTenantRepository : NhRepositoryBase<AbpTenant>, IAbpTenantRepository
    {

    }
}