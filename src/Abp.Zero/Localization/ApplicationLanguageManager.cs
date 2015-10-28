using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Events.Bus.Entities;
using Abp.Events.Bus.Handlers;
using Abp.Runtime.Caching;

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

        private ITypedCache<int, List<ApplicationLanguage>> LanguageListCache
        {
            get { return _cacheManager.GetCache<int, List<ApplicationLanguage>>(CacheName); }
        }

        private readonly IRepository<ApplicationLanguage> _languageRepository;
        private readonly ICacheManager _cacheManager;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationLanguageManager"/> class.
        /// </summary>
        public ApplicationLanguageManager(
            IRepository<ApplicationLanguage> languageRepository,
            ICacheManager cacheManager,
            IUnitOfWorkManager unitOfWorkManager)
        {
            _languageRepository = languageRepository;
            _cacheManager = cacheManager;
            _unitOfWorkManager = unitOfWorkManager;
        }

        public async Task<IReadOnlyList<ApplicationLanguage>> GetLanguagesAsync(int? tenantId)
        {
            var languageList = (await GetLanguagesFromCacheAsync(null)).ToList(); //Creates a copy of the list, so .ToList() is important.

            if (tenantId == null)
            {
                return languageList;
            }

            foreach (var tenantLanguage in await GetLanguagesFromCacheAsync(tenantId.Value))
            {
                var hostLanguage = languageList.FirstOrDefault(l => l.Name == tenantLanguage.Name);
                if (hostLanguage == null)
                {
                    languageList.Add(tenantLanguage);                    
                }
                else
                {
                    hostLanguage.IsActive = tenantLanguage.IsActive;
                }
            }

            return languageList;
        }

        [UnitOfWork]
        public virtual async Task AddAsync(ApplicationLanguage language)
        {
            if ((await GetLanguagesAsync(language.TenantId)).Any(l => l.Name == language.Name))
            {
                throw new AbpException("There is already a language with name = " + language.Name);
            }

            using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                await _languageRepository.InsertAsync(language);
                await _unitOfWorkManager.Current.SaveChangesAsync();
            }
        }

        [UnitOfWork]
        public virtual async Task RemoveAsync(int? tenantId, string languageName)
        {
            var currentLanguage = (await GetLanguagesAsync(tenantId)).FirstOrDefault(l => l.Name == languageName);
            if (currentLanguage == null)
            {
                return;
            }

            if (currentLanguage.TenantId == null && tenantId != null)
            {
                throw new AbpException("Can not delete a host language from tenant. Try to make is passive!");
            }

            using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                await _languageRepository.DeleteAsync(currentLanguage.Id);
                await _unitOfWorkManager.Current.SaveChangesAsync();
            }
        }

        public void HandleEvent(EntityChangedEventData<ApplicationLanguage> eventData)
        {
            LanguageListCache.Remove(eventData.Entity.TenantId ?? 0);

            //Also invalidate the language script cache
            _cacheManager.GetCache("AbpLocalizationScripts").Clear(); //TODO: OPTIMIZATION???
        }

        private Task<List<ApplicationLanguage>> GetLanguagesFromCacheAsync(int? tenantId)
        {
            return LanguageListCache.GetAsync(tenantId ?? 0, () => GetLanguagesFromDatabaseAsync(tenantId));
        }

        [UnitOfWork]
        protected virtual async Task<List<ApplicationLanguage>> GetLanguagesFromDatabaseAsync(int? tenantId)
        {
            using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                return await _languageRepository.GetAllListAsync(l => l.TenantId == tenantId);
            }
        }
    }
}