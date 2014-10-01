using Abp.Domain.Entities.Auditing;

namespace Abp.Authorization.Users
{
    /// <summary>
    /// Represents role record of a user. 
    /// </summary>
    public class UserRole : CreationAuditedEntity<long> //TODO: Add a unique index for UserId, RoleId
    {
        /// <summary>
        /// User Id.
        /// </summary>
        public virtual long UserId { get; set; }

        /// <summary>
        /// Role Id.
        /// </summary>
        public virtual int RoleId { get; set; }

        public UserRole()
        {
            
        }

        public UserRole(long userId, int roleId)
        {
            UserId = userId;
            RoleId = roleId;
        }
    }
}