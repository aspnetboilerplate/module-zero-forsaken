using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Localization.Dictionaries;
using Abp.Runtime.Caching;
using Abp.Runtime.Session;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;

namespace Abp.Localization
{
    /// <summary>
    /// Implements <see cref="IMultiTenantLocalizationDictionary"/>.
    /// </summary>
    public class MultiTenantLocalizationDictionary :
        IMultiTenantLocalizationDictionary
    {
        private readonly string _sourceName;
        private readonly ILocalizationDictionary _internalDictionary;
        private readonly IRepository<ApplicationLanguageText, Guid> _customLocalizationRepository;
        private readonly ICacheManager _cacheManager;
        private readonly IAbpSession _session;

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiTenantLocalizationDictionary"/> class.
        /// </summary>
        public MultiTenantLocalizationDictionary(
            string sourceName,
            ILocalizationDictionary internalDictionary,
            IRepository<ApplicationLanguageText, Guid> customLocalizationRepository,
            ICacheManager cacheManager,
            IAbpSession session)
        {
            _sourceName = sourceName;
            _internalDictionary = internalDictionary;
            _customLocalizationRepository = customLocalizationRepository;
            _cacheManager = cacheManager;
            _session = session;
        }

        public CultureInfo CultureInfo { get { return _internalDictionary.CultureInfo; } }

        public string this[string name]
        {
            get { return _internalDictionary[name]; }
            set { _internalDictionary[name] = value; }
        }

        public LocalizedString GetOrNull(string name)
        {
            return GetOrNull(_session.TenantId, name);
        }

        public LocalizedString GetOrNull(Guid? tenantId, string name)
        {
            //Get cache
            var cache = _cacheManager.GetMultiTenantLocalizationDictionaryCache();

            //Get for current tenant
            var dictionary = cache.Get(CalculateCacheKey(tenantId), () => GetAllValuesFromDatabase(tenantId));
            var value = dictionary.GetOrDefault(name);
            if (value != null)
            {
                return new LocalizedString(name, value, CultureInfo);
            }

            //Fall back to host
            if (tenantId != null)
            {
                dictionary = cache.Get(CalculateCacheKey(null), () => GetAllValuesFromDatabase(null));
                value = dictionary.GetOrDefault(name);
                if (value != null)
                {
                    return new LocalizedString(name, value, CultureInfo);
                }
            }

            //Not found in database, fall back to internal dictionary
            var internalLocalizedString = _internalDictionary.GetOrNull(name);
            if (internalLocalizedString != null)
            {
                return internalLocalizedString;
            }

            //TODO: Fallback to internal's default dictionary!

            //Not found at all
            return null;
        }

        public IReadOnlyList<LocalizedString> GetAllStrings()
        {
            return GetAllStrings(_session.TenantId);
        }

        public IReadOnlyList<LocalizedString> GetAllStrings(Guid? tenantId)
        {
            //Get cache
            var cache = _cacheManager.GetMultiTenantLocalizationDictionaryCache();

            //Create a temp dictionary to build (by underlying dictionary)
            var dictionary = new Dictionary<string, LocalizedString>();

            foreach (var localizedString in _internalDictionary.GetAllStrings())
            {
                dictionary[localizedString.Name] = localizedString;
            }

            //Override by host
            if (tenantId != null)
            {
                var defaultDictionary = cache.Get(CalculateCacheKey(null), () => GetAllValuesFromDatabase(null));
                foreach (var keyValue in defaultDictionary)
                {
                    dictionary[keyValue.Key] = new LocalizedString(keyValue.Key, keyValue.Value, CultureInfo);
                }
            }

            //Override by tenant
            var tenantDictionary = cache.Get(CalculateCacheKey(tenantId), () => GetAllValuesFromDatabase(tenantId));
            foreach (var keyValue in tenantDictionary)
            {
                dictionary[keyValue.Key] = new LocalizedString(keyValue.Key, keyValue.Value, CultureInfo);
            }

            return dictionary.Values.ToImmutableList();
        }

        private string CalculateCacheKey(Guid? tenantId)
        {
            return MultiTenantLocalizationDictionaryCacheHelper.CalculateCacheKey(tenantId, _sourceName, CultureInfo.Name);
        }

        private Dictionary<string, string> GetAllValuesFromDatabase(Guid? tenantId)
        {
            return _customLocalizationRepository
                .GetAllList(l => l.Source == _sourceName && l.LanguageName == CultureInfo.Name && l.TenantId == tenantId)
                .ToDictionary(l => l.Key, l => l.Value);
        }
    }
}