using Abp.Domain.Entities.Mapping;
using Abp.MultiTenancy;

namespace Abp.Zero.NHibernate.EntityMappings
{
    public abstract class TenantMapBase<TTenant> : EntityMap<TTenant> where TTenant : AbpTenant
    {
        protected TenantMapBase()
            : base("AbpTenants")
        {
            Map(x => x.TenancyName);
            Map(x => x.Name);
            Map(x => x.CreationTime);

            Polymorphism.Explicit();
        }
    }
}