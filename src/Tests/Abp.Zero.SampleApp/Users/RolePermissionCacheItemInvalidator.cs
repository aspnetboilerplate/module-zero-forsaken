using Abp.Authorization.Users;
using Abp.Runtime.Caching;
using Abp.Zero.SampleApp.MultiTenancy;

namespace Abp.Zero.SampleApp.Users
{
    public class UserPermissionCacheItemInvalidator : AbpUserPermissionCacheItemInvalidator<Tenant, User>
    {
        public UserPermissionCacheItemInvalidator(ICacheManager cacheManager)
            : base(cacheManager)
        {
        }
    }
}