using Abp.Authorization.Users;
using Abp.MultiTenancy;
using Abp.NHibernate.EntityMappings;

namespace Abp.Zero.NHibernate.EntityMappings
{
    public abstract class TenantMap<TTenant, TUser> : EntityMap<TTenant>
        where TTenant : AbpTenant<TTenant, TUser>
        where TUser : AbpUser<TTenant, TUser>
    {
        protected TenantMap()
            : base("AbpTenants")
        {
            Map(x => x.TenancyName);
            Map(x => x.Name);
            Map(x => x.CreationTime);

            Polymorphism.Explicit();
        }
    }
}