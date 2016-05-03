using System;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.MultiTenancy;

namespace Abp.Authorization.Users
{
    /// <summary>
    /// Represents a summary user
    /// </summary>
    [Table("AbpUserAccounts")]
    [MultiTenancySide(MultiTenancySides.Host)]
    public class UserAccount : FullAuditedEntity<long>
    {
        public int? TenantId { get; set; }

        public long UserId { get; set; }

        public long? UserLinkId { get; set; }

        public string UserName { get; set; }

        public string EmailAddress { get; set; }

        public virtual DateTime? LastLoginTime { get; set; }
    }
}