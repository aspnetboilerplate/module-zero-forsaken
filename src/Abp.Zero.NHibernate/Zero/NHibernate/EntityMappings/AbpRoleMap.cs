using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.MultiTenancy;
using Abp.NHibernate.EntityMappings;

namespace Abp.Zero.NHibernate.EntityMappings
{
    /// <summary>
    /// Base class for role mapping.
    /// </summary>
    public abstract class AbpRoleMap<TTenant, TRole, TUser> : EntityMap<TRole>
        where TRole : AbpRole<TTenant, TUser>
        where TUser : AbpUser<TTenant, TUser>
        where TTenant : AbpTenant<TTenant, TUser>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        protected AbpRoleMap()
            : base("AbpRoles")
        {
            References(x => x.Tenant).Column("TenantId").Nullable();
            
            Map(x => x.Name);
            Map(x => x.DisplayName);
            Map(x => x.IsStatic);
            Map(x => x.IsDefault);
            
            this.MapFullAudited();

            Polymorphism.Explicit();
        }
    }
}