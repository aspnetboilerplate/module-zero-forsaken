using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Abp.Authorization.Users
{
    /// <summary>
    /// Represents role record of a user.
    /// </summary>
    [Table("AbpUserRoles")]
    public class UserRole : CreationAuditedEntity<Guid>, IMayHaveTenant
    {
        public virtual Guid? TenantId { get; set; }

        /// <summary>
        /// User id.
        /// </summary>
        public virtual Guid UserId { get; set; }

        /// <summary>
        /// Role id.
        /// </summary>
        public virtual Guid RoleId { get; set; }

        /// <summary>
        /// Creates a new <see cref="UserRole"/> object.
        /// </summary>
        public UserRole()
        {
        }

        /// <summary>
        /// Creates a new <see cref="UserRole"/> object.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="roleId">Role id</param>
        public UserRole(Guid? tenantId, Guid userId, Guid roleId)
        {
            TenantId = tenantId;
            UserId = userId;
            RoleId = roleId;
        }
    }
}