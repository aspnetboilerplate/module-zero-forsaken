using Abp.Domain.Entities.Auditing;

namespace Abp.Authorization.Permissions
{
    /// <summary>
    /// Represents a permission for a role or user.
    /// Used to grant/deny a permission for a role or user.
    /// </summary>
    public class Permission : CreationAuditedEntity<long>
    {
        /// <summary>
        /// Role Id.
        /// </summary>
        public virtual int? RoleId { get; set; }

        /// <summary>
        /// User Id.
        /// </summary>
        public virtual long? UserId { get; set; }

        /// <summary>
        /// Unique name of the permission.
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Is this role granted for this permission.
        /// Default value: true.
        /// </summary>
        public virtual bool IsGranted { get; set; }

        public Permission()
        {
            IsGranted = true;
        }
    }
}
