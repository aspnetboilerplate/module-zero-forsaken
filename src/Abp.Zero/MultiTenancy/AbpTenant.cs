using Abp.Authorization.Users;
using Abp.Domain.Entities.Auditing;

namespace Abp.MultiTenancy
{
    /// <summary>
    /// Represents a Tenant of the application.
    /// </summary>
    public class AbpTenant<TTenant, TUser> : AuditedEntity<int, TUser>
        where TUser : AbpUser<TTenant,TUser>
        where TTenant : AbpTenant<TTenant, TUser>
    {
        /// <summary>
        /// Tenancy name. This property is the UNIQUE name of this Tenant.
        /// It can be used as subdomain name in a web application.
        /// </summary>
        public virtual string TenancyName { get; set; }

        /// <summary>
        /// Display name of the Tenant.
        /// </summary>
        public virtual string Name { get; set; }

        public AbpTenant()
        {
            
        }

        public AbpTenant(string tenancyName, string name)
        {
            TenancyName = tenancyName;
            Name = name;
        }
    }
}
