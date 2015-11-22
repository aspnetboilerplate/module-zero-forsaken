using Abp.Dependency;
using Abp.Events.Bus.Entities;
using Abp.Events.Bus.Handlers;
using Abp.MultiTenancy;
using Abp.Runtime.Caching;

namespace Abp.Authorization.Users
{
    public abstract class AbpUserPermissionCacheItemInvalidator<TTenant, TUser> :
        IEventHandler<EntityChangedEventData<UserPermissionSetting>>,
        IEventHandler<EntityChangedEventData<UserRole>>,
        IEventHandler<EntityDeletedEventData<TUser>>,

        ITransientDependency

        where TTenant : AbpTenant<TTenant, TUser>
        where TUser : AbpUser<TTenant, TUser>
    {
        private readonly ICacheManager _cacheManager;

        protected AbpUserPermissionCacheItemInvalidator(ICacheManager cacheManager)
        {
            _cacheManager = cacheManager;
        }

        public void HandleEvent(EntityChangedEventData<UserPermissionSetting> eventData)
        {
            _cacheManager.GetUserPermissionCache().Remove(eventData.Entity.UserId);
        }

        public void HandleEvent(EntityChangedEventData<UserRole> eventData)
        {
            _cacheManager.GetUserPermissionCache().Remove(eventData.Entity.UserId);
        }

        public void HandleEvent(EntityDeletedEventData<TUser> eventData)
        {
            _cacheManager.GetUserPermissionCache().Remove(eventData.Entity.Id);
        }
    }
}