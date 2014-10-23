using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Authorization.Users;
using Abp.Domain.Entities.Auditing;
using Abp.MultiTenancy;
using Microsoft.AspNet.Identity;

namespace Abp.Authorization.Roles
{
    /// <summary>
    /// Represents a role in an application.
    /// A role is used to group some permissions to grant users all together.
    /// A user can have multiple roles.
    /// </summary>
    /// <remarks> 
    /// Application should use permissions to check if user is granted to perform an operation.
    /// </remarks>
    public class AbpRole<TTenant, TUser> : AuditedEntity<int, TUser>, IRole<int>, IMayHaveTenant<TTenant, TUser>
        where TUser : AbpUser<TTenant, TUser>
        where TTenant : AbpTenant<TTenant, TUser>
    {
        /// <summary>
        /// The Tenant if this role is a tenant-level role.
        /// </summary>
        [ForeignKey("TenantId")]
        public TTenant Tenant { get; set; }

        /// <summary>
        /// Tenant's Id if this role is a tenant-level role.
        /// </summary>
        public virtual int? TenantId { get; set; }

        /// <summary>
        /// Unique name of this role.
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Display name of this role.
        /// </summary>
        public virtual string DisplayName { get; set; }

        [ForeignKey("RoleId")]
        public virtual ICollection<RolePermissionSetting> Permissions { get; set; }

        public AbpRole()
        {
            
        }
        
        public AbpRole(int? tenantId, string name, string displayName)
        {
            TenantId = tenantId;
            Name = name;
            DisplayName = displayName;
        }
    }
}
