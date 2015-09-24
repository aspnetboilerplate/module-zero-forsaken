using System;
using System.Collections.Generic;

namespace Abp.Authorization.Users
{
    /// <summary>
    /// Used to cache roles and permissions of a user.
    /// It's invalidated on;
    /// + User delete
    /// + Role delete
    /// + User-Role changes
    /// + User-Permission changes
    /// + External cache clear for <see cref="CacheStoreName"/>
    /// </summary>
    [Serializable]
    internal class UserPermissionCacheItem
    {
        public const string CacheStoreName = "Abp.Zero.UserPermissions";

        public static TimeSpan CacheExpireTime { get; private set; }

        public long UserId { get; set; }

        public List<int> RoleIds { get; set; }

        public HashSet<string> GrantedPermissions { get; set; }

        public HashSet<string> ProhibitedPermissions { get; set; }

        static UserPermissionCacheItem()
        {
            CacheExpireTime = TimeSpan.FromMinutes(20);
        }

        public UserPermissionCacheItem()
        {
            RoleIds = new List<int>();
            GrantedPermissions = new HashSet<string>();
            ProhibitedPermissions = new HashSet<string>();
        }

        public UserPermissionCacheItem(long userId)
            : this()
        {
            UserId = userId;
        }
    }
}
