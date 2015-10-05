using System.Threading.Tasks;
using Abp.Application.Features;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.Domain.Uow;
using Abp.Runtime.Caching;

namespace Abp.Application.Editions
{
    public abstract class AbpEditionManager : IDomainService
    {
        public ICacheManager CacheManager { get; set; }

        public IRepository<Edition> EditionRepository { get; set; }

        public IRepository<EditionFeatureSetting, long>  EditionFeatureRepository { get; set; }
        
        public virtual async Task<string> GetFeatureValueOrNullAsync(int editionId, string featureName)
        {
            var cacheItem = await GetEditionFeatureCacheItemAsync(editionId);
            return cacheItem.FeatureValues.GetOrDefault(featureName);
        }

        protected virtual async Task<EditionfeatureCacheItem> GetEditionFeatureCacheItemAsync(int editionId)
        {
            return await CacheManager
                .GetEditionFeatureCache()
                .GetAsync(
                    editionId,
                    async () => await CreateEditionFeatureCacheItem(editionId)
                );
        }

        protected virtual async Task<EditionfeatureCacheItem> CreateEditionFeatureCacheItem(int editionId)
        {
            var newCacheItem = new EditionfeatureCacheItem();

            var featureSettings = await EditionFeatureRepository.GetAllListAsync(f => f.EditionId == editionId);
            foreach (var featureSetting in featureSettings)
            {
                newCacheItem.FeatureValues[featureSetting.Name] = featureSetting.Value;
            }

            return newCacheItem;
        }

        public virtual Task CreateAsync(Edition edition)
        {
            return EditionRepository.InsertAsync(edition);
        }

        public virtual Task<Edition> FindByNameAsync(string name)
        {
            return EditionRepository.FirstOrDefaultAsync(edition => edition.Name == name);
        }

        public virtual Task<Edition> FindByIdAsync(int id)
        {
            return EditionRepository.FirstOrDefaultAsync(id);
        }

        public virtual Task DeleteAsync(Edition edition)
        {
            return EditionRepository.DeleteAsync(edition);
        }

        [UnitOfWork]
        public virtual async Task SetFeatureValue(int editionId, string featureName, string value)
        {
            if (await GetFeatureValueOrNullAsync(editionId, featureName) == value)
            {
                return;
            }

            var currentSetting = await EditionFeatureRepository.FirstOrDefaultAsync(f => f.EditionId == editionId && f.Name == featureName);
            if (currentSetting == null)
            {
                await EditionFeatureRepository.InsertAsync(new EditionFeatureSetting(editionId, featureName, value));
            }
            else
            {
                currentSetting.Value = value;
            }
        }
    }
}
