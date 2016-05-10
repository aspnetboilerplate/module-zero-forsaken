using Abp.Dependency;
using Abp.Events.Bus.Entities;
using Abp.Events.Bus.Handlers;
using Abp.Runtime.Caching;
using System;

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
            var cacheKey = eventData.Entity.RoleId + "@" + (eventData.Entity.TenantId ?? Guid.Empty);
            _cacheManager.GetRolePermissionCache().Remove(cacheKey);
        }

        public void HandleEvent(EntityDeletedEventData<AbpRoleBase> eventData)
        {
            var cacheKey = eventData.Entity.Id + "@" + (eventData.Entity.TenantId ?? Guid.Empty);
            _cacheManager.GetRolePermissionCache().Remove(cacheKey);
        }
    }
}