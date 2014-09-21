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
    public class AbpRole : AuditedEntity, IRole<int>, IMayHaveTenant
    {
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
    }
}
