using System;
using System.Collections.Generic;

namespace Abp.Authorization.Users
{
    /// <summary>
    /// Used to cache roles and permissions of a user.
    /// </summary>
    [Serializable]
    public class UserPermissionCacheItem
    {
        public const string CacheStoreName = "AbpZeroUserPermissions";

        /// <summary>
        /// Gets/sets expire time for cache items.
        /// Default: 20 minutes.
        /// TODO: This is not used yet!
        /// </summary>
        public static TimeSpan CacheExpireTime { get; private set; }

        public Guid UserId { get; set; }

        public List<Guid> RoleIds { get; set; }

        public HashSet<string> GrantedPermissions { get; set; }

        public HashSet<string> ProhibitedPermissions { get; set; }

        static UserPermissionCacheItem()
        {
            CacheExpireTime = TimeSpan.FromMinutes(20);
        }

        public UserPermissionCacheItem()
        {
            RoleIds = new List<Guid>();
            GrantedPermissions = new HashSet<string>();
            ProhibitedPermissions = new HashSet<string>();
        }

        public UserPermissionCacheItem(Guid userId)
            : this()
        {
            UserId = userId;
        }
    }
}