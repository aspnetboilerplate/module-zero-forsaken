using Abp.Dependency;
using Abp.Events.Bus.Entities;
using Abp.Events.Bus.Handlers;
using Abp.Runtime.Caching;

namespace Abp.Authorization.Users
{
    public class AbpUserPermissionCacheItemInvalidator :
        IEventHandler<EntityChangedEventData<UserPermissionSetting>>,
        IEventHandler<EntityChangedEventData<UserRole>>,
        IEventHandler<EntityDeletedEventData<AbpUserBase>>,

        ITransientDependency
    {
        private readonly ICacheManager _cacheManager;

        public AbpUserPermissionCacheItemInvalidator(ICacheManager cacheManager)
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

        public void HandleEvent(EntityDeletedEventData<AbpUserBase> eventData)
        {
            _cacheManager.GetUserPermissionCache().Remove(eventData.Entity.Id);
        }
    }
}