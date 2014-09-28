using Abp.MultiTenancy;
using Abp.NHibernate.EntityMappings;

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