using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Abp.Application.Editions;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.Events.Bus.Entities;
using Abp.Events.Bus.Handlers;
using Abp.IdentityFramework;
using Abp.Localization;
using Abp.Runtime.Caching;
using Abp.Zero;
using Microsoft.AspNet.Identity;

namespace Abp.MultiTenancy
{
    /// <summary>
    /// Tenant manager.
    /// Implements domain logic for <see cref="AbpTenant{TTenant,TUser}"/>.
    /// </summary>
    /// <typeparam name="TTenant">Type of the application Tenant</typeparam>
    /// <typeparam name="TRole">Type of the application Role</typeparam>
    /// <typeparam name="TUser">Type of the application User</typeparam>
    public abstract class AbpTenantManager<TTenant, TRole, TUser> : IDomainService, 
        IEventHandler<EntityChangedEventData<TTenant>>
        where TTenant : AbpTenant<TTenant, TUser>
        where TRole : AbpRole<TTenant, TUser>
        where TUser : AbpUser<TTenant, TUser>
    {
        public AbpEditionManager EditionManager { get; set; }
        public ILocalizationManager LocalizationManager { get; set; }

        public ICacheManager CacheManager { get; set; }

        public IRepository<TTenant> TenantRepository { get; set; }

        public IRepository<TenantFeatureSetting, long> TenantFeatureRepository { get; set; }

        protected AbpTenantManager(AbpEditionManager editionManager)
        {
            EditionManager = editionManager;
            LocalizationManager = NullLocalizationManager.Instance;
        }

        public virtual IQueryable<TTenant> Tenants { get { return TenantRepository.GetAll(); } }

        public virtual async Task<IdentityResult> CreateAsync(TTenant tenant)
        {
            if (await TenantRepository.FirstOrDefaultAsync(t => t.TenancyName == tenant.TenancyName) != null)
            {
                return AbpIdentityResult.Failed(string.Format(L("TenancyNameIsAlreadyTaken"), tenant.TenancyName));
            }

            var validationResult = await ValidateTenantAsync(tenant);
            if (!validationResult.Succeeded)
            {
                return validationResult;
            }

            await TenantRepository.InsertAsync(tenant);
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> UpdateAsync(TTenant tenant)
        {
            if (await TenantRepository.FirstOrDefaultAsync(t => t.TenancyName == tenant.TenancyName && t.Id != tenant.Id) != null)
            {
                return AbpIdentityResult.Failed(string.Format(L("TenancyNameIsAlreadyTaken"), tenant.TenancyName));
            }

            await TenantRepository.UpdateAsync(tenant);
            return IdentityResult.Success;
        }

        public virtual async Task<TTenant> FindByIdAsync(int id)
        {
            return await TenantRepository.FirstOrDefaultAsync(id);
        }

        public virtual async Task<TTenant> GetByIdAsync(int id)
        {
            var tenant = await FindByIdAsync(id);
            if (tenant == null)
            {
                throw new AbpException("There is no tenant with id: " + id);
            }

            return tenant;
        }

        public virtual Task<TTenant> FindByTenancyNameAsync(string tenancyName)
        {
            return TenantRepository.FirstOrDefaultAsync(t => t.TenancyName == tenancyName);
        }

        public virtual async Task<IdentityResult> DeleteAsync(TTenant tenant)
        {
            await TenantRepository.DeleteAsync(tenant);
            return IdentityResult.Success;
        }
        
        #region Features

        public async Task<string> GetFeatureValueOrNullAsync(int tenantId, string featureName)
        {
            var cacheItem = await GetTenantFeatureCacheItemAsync(tenantId);
            var value = cacheItem.FeatureValues.GetOrDefault(featureName);
            if (value != null)
            {
                return value;
            }

            if (cacheItem.EditionId.HasValue)
            {
                value = await EditionManager.GetFeatureValueOrNullAsync(cacheItem.EditionId.Value, featureName);
                if (value != null)
                {
                    return value;
                }
            }

            return null;
        }

        private async Task<TenantFeatureCacheItem> GetTenantFeatureCacheItemAsync(int tenantId)
        {
            return await CacheManager.GetTenantFeatureCache().GetAsync(tenantId, async () =>
            {
                var tenant = await GetByIdAsync(tenantId);

                var newCacheItem = new TenantFeatureCacheItem {EditionId = tenant.EditionId};

                var featureSettings = await TenantFeatureRepository.GetAllListAsync(f => f.TenantId == tenantId);
                foreach (var featureSetting in featureSettings)
                {
                    newCacheItem.FeatureValues[featureSetting.Name] = featureSetting.Value;
                }

                return newCacheItem;
            });
        }

        #endregion

        protected virtual async Task<IdentityResult> ValidateTenantAsync(TTenant tenant)
        {
            var nameValidationResult = await ValidateTenancyNameAsync(tenant.TenancyName);
            if (!nameValidationResult.Succeeded)
            {
                return nameValidationResult;
            }

            return IdentityResult.Success;
        }

        protected virtual async Task<IdentityResult> ValidateTenancyNameAsync(string tenancyName)
        {
            if (!Regex.IsMatch(tenancyName, AbpTenant<TTenant, TUser>.TenancyNameRegex))
            {
                return AbpIdentityResult.Failed(L("InvalidTenancyName"));
            }

            return IdentityResult.Success;
        }

        private string L(string name)
        {
            return LocalizationManager.GetString(AbpZeroConsts.LocalizationSourceName, name);
        }

        public void HandleEvent(EntityChangedEventData<TTenant> eventData)
        {
            if (eventData.Entity.IsTransient())
            {
                return;
            }

            CacheManager.GetTenantFeatureCache().Remove(eventData.Entity.Id);
        }
    }
}