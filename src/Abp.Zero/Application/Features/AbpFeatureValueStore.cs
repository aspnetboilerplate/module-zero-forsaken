using Abp.Application.Editions;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.Collections.Extensions;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.MultiTenancy;
using Abp.Runtime.Caching;
using System;
using System.Threading.Tasks;

namespace Abp.Application.Features
{
    /// <summary>
    /// Implements <see cref="IFeatureValueStore"/>.
    /// </summary>
    public abstract class AbpFeatureValueStore<TTenant, TRole, TUser> : IAbpZeroFeatureValueStore, ITransientDependency
        where TTenant : AbpTenant<TTenant, TUser>
        where TRole : AbpRole<TTenant, TUser>
        where TUser : AbpUser<TTenant, TUser>
    {
        private readonly ICacheManager _cacheManager;
        private readonly IRepository<TenantFeatureSetting, Guid> _tenantFeatureRepository;
        private readonly IRepository<TTenant, Guid> _tenantRepository;
        private readonly IRepository<EditionFeatureSetting, Guid> _editionFeatureRepository;
        private readonly IFeatureManager _featureManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="AbpFeatureValueStore{TTenant, TRole, TUser}"/> class.
        /// </summary>
        protected AbpFeatureValueStore(
            ICacheManager cacheManager,
            IRepository<TenantFeatureSetting, Guid> tenantFeatureRepository,
            IRepository<TTenant, Guid> tenantRepository,
            IRepository<EditionFeatureSetting, Guid> editionFeatureRepository,
            IFeatureManager featureManager)
        {
            _cacheManager = cacheManager;
            _tenantFeatureRepository = tenantFeatureRepository;
            _tenantRepository = tenantRepository;
            _editionFeatureRepository = editionFeatureRepository;
            _featureManager = featureManager;
        }

        /// <inheritdoc/>
        public virtual Task<string> GetValueOrNullAsync(Guid tenantId, Feature feature)
        {
            return GetValueOrNullAsync(tenantId, feature.Name);
        }

        public virtual async Task<string> GetEditionValueOrNullAsync(Guid editionId, string featureName)
        {
            var cacheItem = await GetEditionFeatureCacheItemAsync(editionId);
            return cacheItem.FeatureValues.GetOrDefault(featureName);
        }

        public async Task<string> GetValueOrNullAsync(Guid tenantId, string featureName)
        {
            var cacheItem = await GetTenantFeatureCacheItemAsync(tenantId);
            var value = cacheItem.FeatureValues.GetOrDefault(featureName);
            if (value != null)
            {
                return value;
            }

            if (cacheItem.EditionId.HasValue)
            {
                value = await GetEditionValueOrNullAsync(cacheItem.EditionId.Value, featureName);
                if (value != null)
                {
                    return value;
                }
            }

            return null;
        }

        [UnitOfWork]
        public virtual async Task SetEditionFeatureValueAsync(Guid editionId, string featureName, string value)
        {
            if (await GetEditionValueOrNullAsync(editionId, featureName) == value)
            {
                return;
            }

            var currentSetting = await _editionFeatureRepository.FirstOrDefaultAsync(f => f.EditionId == editionId && f.Name == featureName);

            var feature = _featureManager.GetOrNull(featureName);
            if (feature == null || feature.DefaultValue == value)
            {
                if (currentSetting != null)
                {
                    await _editionFeatureRepository.DeleteAsync(currentSetting);
                }

                return;
            }

            if (currentSetting == null)
            {
                await _editionFeatureRepository.InsertAsync(new EditionFeatureSetting(editionId, featureName, value));
            }
            else
            {
                currentSetting.Value = value;
            }
        }

        private async Task<TenantFeatureCacheItem> GetTenantFeatureCacheItemAsync(Guid tenantId)
        {
            return await _cacheManager.GetTenantFeatureCache().GetAsync(tenantId, async () =>
            {
                var tenant = await _tenantRepository.GetAsync(tenantId);

                var newCacheItem = new TenantFeatureCacheItem { EditionId = tenant.EditionId };

                var featureSettings = await _tenantFeatureRepository.GetAllListAsync(f => f.TenantId == tenantId);
                foreach (var featureSetting in featureSettings)
                {
                    newCacheItem.FeatureValues[featureSetting.Name] = featureSetting.Value;
                }

                return newCacheItem;
            });
        }

        protected virtual async Task<EditionfeatureCacheItem> GetEditionFeatureCacheItemAsync(Guid editionId)
        {
            return await _cacheManager
                .GetEditionFeatureCache()
                .GetAsync(
                    editionId,
                    async () => await CreateEditionFeatureCacheItem(editionId)
                );
        }

        protected virtual async Task<EditionfeatureCacheItem> CreateEditionFeatureCacheItem(Guid editionId)
        {
            var newCacheItem = new EditionfeatureCacheItem();

            var featureSettings = await _editionFeatureRepository.GetAllListAsync(f => f.EditionId == editionId);
            foreach (var featureSetting in featureSettings)
            {
                newCacheItem.FeatureValues[featureSetting.Name] = featureSetting.Value;
            }

            return newCacheItem;
        }
    }
}