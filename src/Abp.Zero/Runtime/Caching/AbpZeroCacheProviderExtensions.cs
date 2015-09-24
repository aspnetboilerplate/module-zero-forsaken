using Abp.Authorization.Roles;
using Abp.Authorization.Users;

namespace Abp.Runtime.Caching
{
    internal static class AbpZeroCacheProviderExtensions
    {
        public static ITypedCache<long, UserPermissionCacheItem> GetUserPermissionCache(this ICacheManager cacheManager)
        {
            return cacheManager.GetCache(UserPermissionCacheItem.CacheStoreName).AsTyped<long, UserPermissionCacheItem>();
        }

        public static ITypedCache<int, RolePermissionCacheItem> GetRolePermissionCache(this ICacheManager cacheManager)
        {
            return cacheManager.GetCache(RolePermissionCacheItem.CacheStoreName).AsTyped<int, RolePermissionCacheItem>();
        }
    }
}
