using System;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace Abp.Authorization.Users
{
    /// <summary>
    /// Represents a summary user
    /// </summary>
    public class UserAccount : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public long UserId { get; set; }

        public long UserLinkId { get; set; }

        public string Username { get; set; }

        public string EmailAddress { get; set; }
    }
}