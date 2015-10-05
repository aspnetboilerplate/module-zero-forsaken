using Abp.Application.Features;
using Abp.Dependency;
using Abp.Events.Bus.Entities;
using Abp.Events.Bus.Handlers;
using Abp.Runtime.Caching;

namespace Abp.Application.Editions
{
    public class EditionFeatureCacheItemInvalidator : IEventHandler<EntityChangedEventData<EditionFeatureSetting>>,ITransientDependency
    {
        private readonly ICacheManager _cacheManager;

        public EditionFeatureCacheItemInvalidator(ICacheManager cacheManager)
        {
            _cacheManager = cacheManager;
        }

        public void HandleEvent(EntityChangedEventData<EditionFeatureSetting> eventData)
        {
            _cacheManager.GetEditionFeatureCache().Remove(eventData.Entity.EditionId);
        }
    }
}