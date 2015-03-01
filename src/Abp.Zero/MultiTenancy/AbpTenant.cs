using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Authorization.Users;
using Abp.Configuration;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace Abp.MultiTenancy
{
    /// <summary>
    /// Represents a Tenant of the application.
    /// </summary>
    [Table("AbpTenants")]
    public class AbpTenant<TTenant, TUser> : AuditedEntity<int, TUser>, ISuspendable
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

        /// <summary>
        /// Is this tenant suspended?
        /// If as tenant is suspended, no user of this tenant can use the application.
        /// </summary>
        public bool IsSuspended { get; set; }

        /// <summary>
        /// Defined settings for this tenant.
        /// </summary>
        [ForeignKey("TenantId")]
        public virtual ICollection<Setting> Settings { get; set; }

        /// <summary>
        /// Creates a new tenant.
        /// </summary>
        public AbpTenant()
        {
            
        }

        /// <summary>
        /// Creates a new tenant.
        /// </summary>
        /// <param name="tenancyName">UNIQUE name of this Tenant</param>
        /// <param name="name">Display name of the Tenant</param>
        public AbpTenant(string tenancyName, string name)
        {
            TenancyName = tenancyName;
            Name = name;
        }
    }
}
