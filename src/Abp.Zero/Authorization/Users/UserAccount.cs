using System;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace Abp.Authorization.Users
{
    /// <summary>
    /// Represents a summary user
    /// </summary>
    [Table("AbpUserAccounts")]
    public class UserAccount : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public long UserId { get; set; }

        public long? UserLinkId { get; set; }

        public string UserName { get; set; }

        public string EmailAddress { get; set; }
    }
}