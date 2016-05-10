using Abp.Domain.Entities.Auditing;
using Abp.MultiTenancy;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Abp.Authorization.Users
{
    /// <summary>
    /// Represents a summary user
    /// </summary>
    [Table("AbpUserAccounts")]
    [MultiTenancySide(MultiTenancySides.Host)]
    public class UserAccount : FullAuditedEntity
    {
        public virtual Guid? TenantId { get; set; }

        public virtual Guid UserId { get; set; }

        public virtual Guid? UserLinkId { get; set; }

        public virtual string UserName { get; set; }

        public virtual string EmailAddress { get; set; }

        public virtual DateTime? LastLoginTime { get; set; }
    }
}