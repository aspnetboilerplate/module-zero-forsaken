using Abp.Authorization.Users;
using Abp.Dependency;
using Abp.Events.Bus.Entities;
using Abp.Events.Bus.Handlers;
using Abp.MultiTenancy;
using Abp.Runtime.Caching;

namespace Abp.Authorization.Roles
{
    public abstract class AbpRolePermissionCacheItemInvalidator<TTenant, TRole, TUser> :
        IEventHandler<EntityChangedEventData<RolePermissionSetting>>,
        IEventHandler<EntityDeletedEventData<TRole>>,

        ITransientDependency

        where TRole : AbpRole<TTenant, TUser>
        where TUser : AbpUser<TTenant, TUser>
        where TTenant : AbpTenant<TTenant, TUser>
    {
        private readonly ICacheManager _cacheManager;

        protected AbpRolePermissionCacheItemInvalidator(ICacheManager cacheManager)
        {
            _cacheManager = cacheManager;
        }

        public void HandleEvent(EntityChangedEventData<RolePermissionSetting> eventData)
        {
            _cacheManager.GetRolePermissionCache().Remove(eventData.Entity.RoleId);
        }

        public void HandleEvent(EntityDeletedEventData<TRole> eventData)
        {
            _cacheManager.GetRolePermissionCache().Remove(eventData.Entity.Id);
        }
    }
}