using Abp.Domain.Entities.Auditing;

namespace Abp.Authorization.Users
{
    /// <summary>
    /// Represents role record of a user. 
    /// </summary>
    public class UserRole : CreationAuditedEntity<long>
    {
        public virtual long UserId { get; set; }

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