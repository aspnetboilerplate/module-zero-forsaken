using Abp.Authorization.Roles;
using Abp.Runtime.Caching;
using Abp.Zero.SampleApp.MultiTenancy;
using Abp.Zero.SampleApp.Users;

namespace Abp.Zero.SampleApp.Roles
{
    public class RolePermissionCacheItemInvalidator : AbpRolePermissionCacheItemInvalidator<Tenant, Role, User>
    {
        public RolePermissionCacheItemInvalidator(ICacheManager cacheManager) 
            : base(cacheManager)
        {
        }
    }
}