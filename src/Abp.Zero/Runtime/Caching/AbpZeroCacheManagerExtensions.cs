using Abp.Application.Editions;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.MultiTenancy;
using System;

namespace Abp.Runtime.Caching
{
    public static class AbpZeroCacheManagerExtensions
    {
        public static ITypedCache<string, UserPermissionCacheItem> GetUserPermissionCache(this ICacheManager cacheManager)
        {
            return cacheManager.GetCache<string, UserPermissionCacheItem>(UserPermissionCacheItem.CacheStoreName);
        }

        public static ITypedCache<string, RolePermissionCacheItem> GetRolePermissionCache(this ICacheManager cacheManager)
        {
            return cacheManager.GetCache<string, RolePermissionCacheItem>(RolePermissionCacheItem.CacheStoreName);
        }

        public static ITypedCache<Guid, TenantFeatureCacheItem> GetTenantFeatureCache(this ICacheManager cacheManager)
        {
            return cacheManager.GetCache<Guid, TenantFeatureCacheItem>(TenantFeatureCacheItem.CacheStoreName);
        }

        public static ITypedCache<Guid, EditionfeatureCacheItem> GetEditionFeatureCache(this ICacheManager cacheManager)
        {
            return cacheManager.GetCache<Guid, EditionfeatureCacheItem>(EditionfeatureCacheItem.CacheStoreName);
        }
    }
}