using Abp.Dependency;
using Abp.Events.Bus.Entities;
using Abp.Events.Bus.Handlers;
using Abp.Runtime.Caching;

namespace Abp.MultiTenancy
{
    public class TenantFeatureCacheItemInvalidator : IEventHandler<EntityChangedEventData<TenantFeatureSetting>>, ITransientDependency
    {
        private readonly ICacheManager _cacheManager;

        public TenantFeatureCacheItemInvalidator(ICacheManager cacheManager)
        {
            _cacheManager = cacheManager;
        }

        public void HandleEvent(EntityChangedEventData<TenantFeatureSetting> eventData)
        {
            _cacheManager.GetTenantFeatureCache().Remove(eventData.Entity.TenantId);
        }
    }
}