using Abp.Configuration;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Events.Bus.Entities;
using Abp.Events.Bus.Handlers;
using Abp.Runtime.Caching;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Abp.Localization
{
    /// <summary>
    /// Manages host and tenant languages.
    /// </summary>
    public class ApplicationLanguageManager :
        IApplicationLanguageManager,
        IEventHandler<EntityChangedEventData<ApplicationLanguage>>,
        ISingletonDependency
    {
        /// <summary>
        /// Cache name for languages.
        /// </summary>
        public const string CacheName = "AbpZeroLanguages";

        private ITypedCache<Guid, Dictionary<string, ApplicationLanguage>> LanguageListCache
        {
            get { return _cacheManager.GetCache<Guid, Dictionary<string, ApplicationLanguage>>(CacheName); }
        }

        private readonly IRepository<ApplicationLanguage, Guid> _languageRepository;
        private readonly ICacheManager _cacheManager;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly ISettingManager _settingManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationLanguageManager"/> class.
        /// </summary>
        public ApplicationLanguageManager(
            IRepository<ApplicationLanguage, Guid> languageRepository,
            ICacheManager cacheManager,
            IUnitOfWorkManager unitOfWorkManager,
            ISettingManager settingManager)
        {
            _languageRepository = languageRepository;
            _cacheManager = cacheManager;
            _unitOfWorkManager = unitOfWorkManager;
            _settingManager = settingManager;
        }

        /// <summary>
        /// Gets list of all languages available to given tenant (or null for host)
        /// </summary>
        /// <param name="tenantId">TenantId or null for host</param>
        public async Task<IReadOnlyList<ApplicationLanguage>> GetLanguagesAsync(Guid? tenantId)
        {
            return (await GetLanguageDictionary(tenantId)).Values.ToImmutableList();
        }

        /// <summary>
        /// Adds a new language.
        /// </summary>
        /// <param name="language">The language.</param>
        [UnitOfWork]
        public virtual async Task AddAsync(ApplicationLanguage language)
        {
            if ((await GetLanguagesAsync(language.TenantId)).Any(l => l.Name == language.Name))
            {
                throw new AbpException("There is already a language with name = " + language.Name); //TODO: LOCALIZE?
            }

            using (_unitOfWorkManager.Current.SetTenantId(language.TenantId))
            {
                await _languageRepository.InsertAsync(language);
                await _unitOfWorkManager.Current.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Deletes a language.
        /// </summary>
        /// <param name="tenantId">Tenant Id or null for host.</param>
        /// <param name="languageName">Name of the language.</param>
        [UnitOfWork]
        public virtual async Task RemoveAsync(Guid? tenantId, string languageName)
        {
            var currentLanguage = (await GetLanguagesAsync(tenantId)).FirstOrDefault(l => l.Name == languageName);
            if (currentLanguage == null)
            {
                return;
            }

            if (currentLanguage.TenantId == null && tenantId != null)
            {
                throw new AbpException("Can not delete a host language from tenant!"); //TODO: LOCALIZE?
            }

            using (_unitOfWorkManager.Current.SetTenantId(currentLanguage.TenantId))
            {
                await _languageRepository.DeleteAsync(currentLanguage.Id);
                await _unitOfWorkManager.Current.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Updates a language.
        /// </summary>
        /// <param name="language">The language to be updated</param>
        [UnitOfWork]
        public virtual async Task UpdateAsync(Guid? tenantId, ApplicationLanguage language)
        {
            var existingLanguageWithSameName = (await GetLanguagesAsync(language.TenantId)).FirstOrDefault(l => l.Name == language.Name);
            if (existingLanguageWithSameName != null)
            {
                if (existingLanguageWithSameName.Id != language.Id)
                {
                    throw new AbpException("There is already a language with name = " + language.Name); //TODO: LOCALIZE
                }
            }

            if (language.TenantId == null && tenantId != null)
            {
                throw new AbpException("Can not update a host language from tenant"); //TODO: LOCALIZE
            }

            using (_unitOfWorkManager.Current.SetTenantId(language.TenantId))
            {
                await _languageRepository.UpdateAsync(language);
                await _unitOfWorkManager.Current.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Gets the default language or null for a tenant or the host.
        /// </summary>
        /// <param name="tenantId">Tenant Id of null for host</param>
        public async Task<ApplicationLanguage> GetDefaultLanguageOrNullAsync(Guid? tenantId)
        {
            var defaultLanguageName = tenantId.HasValue
                ? await _settingManager.GetSettingValueForTenantAsync(LocalizationSettingNames.DefaultLanguage, tenantId.Value)
                : await _settingManager.GetSettingValueForApplicationAsync(LocalizationSettingNames.DefaultLanguage);

            return (await GetLanguagesAsync(tenantId)).FirstOrDefault(l => l.Name == defaultLanguageName);
        }

        /// <summary>
        /// Sets the default language for a tenant or the host.
        /// </summary>
        /// <param name="tenantId">Tenant Id of null for host</param>
        /// <param name="languageName">Name of the language.</param>
        public async Task SetDefaultLanguageAsync(Guid? tenantId, string languageName)
        {
            var cultureInfo = CultureInfo.GetCultureInfo(languageName);
            if (tenantId.HasValue)
            {
                await _settingManager.ChangeSettingForTenantAsync(tenantId.Value, LocalizationSettingNames.DefaultLanguage, cultureInfo.Name);
            }
            else
            {
                await _settingManager.ChangeSettingForApplicationAsync(LocalizationSettingNames.DefaultLanguage, cultureInfo.Name);
            }
        }

        public void HandleEvent(EntityChangedEventData<ApplicationLanguage> eventData)
        {
            LanguageListCache.Remove(eventData.Entity.TenantId ?? Guid.Empty);

            //Also invalidate the language script cache
            _cacheManager.GetCache("AbpLocalizationScripts").Clear(); //TODO: CAN BE AN OPTIMIZATION?
        }

        private async Task<Dictionary<string, ApplicationLanguage>> GetLanguageDictionary(Guid? tenantId)
        {
            //Creates a copy of the cached dictionary (to not modify it)
            var languageDictionary = new Dictionary<string, ApplicationLanguage>(await GetLanguageDictionaryFromCacheAsync(null));

            if (tenantId == null)
            {
                return languageDictionary;
            }

            //Override tenant languages
            foreach (var tenantLanguage in await GetLanguageDictionaryFromCacheAsync(tenantId.Value))
            {
                languageDictionary[tenantLanguage.Key] = tenantLanguage.Value;
            }

            return languageDictionary;
        }

        private Task<Dictionary<string, ApplicationLanguage>> GetLanguageDictionaryFromCacheAsync(Guid? tenantId)
        {
            return LanguageListCache.GetAsync(tenantId ?? Guid.Empty, () => GetLanguagesFromDatabaseAsync(tenantId));
        }

        [UnitOfWork]
        protected virtual async Task<Dictionary<string, ApplicationLanguage>> GetLanguagesFromDatabaseAsync(Guid? tenantId)
        {
            using (_unitOfWorkManager.Current.SetTenantId(tenantId))
            {
                return (await _languageRepository.GetAllListAsync()).ToDictionary(l => l.Name);
            }
        }
    }
}