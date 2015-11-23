using Abp.Dependency;
using Abp.Events.Bus.Entities;
using Abp.Events.Bus.Handlers;
using Abp.Runtime.Caching;

namespace Abp.Authorization.Roles
{
    public class AbpRolePermissionCacheItemInvalidator :
        IEventHandler<EntityChangedEventData<RolePermissionSetting>>,
        IEventHandler<EntityDeletedEventData<AbpRoleBase>>,

        ITransientDependency
    {
        private readonly ICacheManager _cacheManager;

        public AbpRolePermissionCacheItemInvalidator(ICacheManager cacheManager)
        {
            _cacheManager = cacheManager;
        }

        public void HandleEvent(EntityChangedEventData<RolePermissionSetting> eventData)
        {
            _cacheManager.GetRolePermissionCache().Remove(eventData.Entity.RoleId);
        }

        public void HandleEvent(EntityDeletedEventData<AbpRoleBase> eventData)
        {
            _cacheManager.GetRolePermissionCache().Remove(eventData.Entity.Id);
        }
    }
}