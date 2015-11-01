using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using Abp.Collections.Extensions;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Events.Bus.Entities;
using Abp.Events.Bus.Handlers;
using Abp.Localization.Dictionaries;
using Abp.Runtime.Caching;
using Abp.Runtime.Session;

namespace Abp.Localization
{
    public class MultiTenantLocalizationDictionary :
        IMultiTenantLocalizationDictionary,
        ITransientDependency,
        IEventHandler<EntityChangedEventData<ApplicationLanguageText>>
    {
        private readonly string _sourceName;
        private readonly ILocalizationDictionary _internalDictionary;
        private readonly IRepository<ApplicationLanguageText, long> _customLocalizationRepository;
        private readonly IAbpSession _session;
        private readonly ITypedCache<string, Dictionary<string, string>> _cache;

        public MultiTenantLocalizationDictionary(
            string sourceName,
            ILocalizationDictionary internalDictionary,
            IRepository<ApplicationLanguageText, long> customLocalizationRepository,
            ICacheManager cacheManager,
            IAbpSession session)
        {
            _sourceName = sourceName;
            _internalDictionary = internalDictionary;
            _customLocalizationRepository = customLocalizationRepository;
            _session = session;
            _cache = cacheManager.GetCache("CustomLocalizationCache").AsTyped<string, Dictionary<string, string>>();
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

        public LocalizedString GetOrNull(int? tenantId, string name)
        {
            //Get for current tenant
            var dictionary = _cache.Get(CalculateCacheKey(tenantId), () => GetAllValuesFromDatabase(tenantId));
            var value = dictionary.GetOrDefault(name);
            if (value != null)
            {
                return new LocalizedString(name, value, CultureInfo);
            }

            //Fall back to host
            if (tenantId != null)
            {
                dictionary = _cache.Get(CalculateCacheKey(null), () => GetAllValuesFromDatabase(null));
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

        public IReadOnlyList<LocalizedString> GetAllStrings(int? tenantId)
        {
            //Create a temp dictionary to build (by underlying dictionary)
            var dictionary = new Dictionary<string, LocalizedString>();

            foreach (var localizedString in _internalDictionary.GetAllStrings())
            {
                dictionary[localizedString.Name] = localizedString;
            }

            //Override by host
            if (tenantId != null)
            {
                var defaultDictionary = _cache.Get(CalculateCacheKey(null), () => GetAllValuesFromDatabase(null));
                foreach (var keyValue in defaultDictionary)
                {
                    dictionary[keyValue.Key] = new LocalizedString(keyValue.Key, keyValue.Value, CultureInfo);
                }
            }

            //Override by tenant
            var tenantDictionary = _cache.Get(CalculateCacheKey(tenantId), () => GetAllValuesFromDatabase(tenantId));
            foreach (var keyValue in tenantDictionary)
            {
                dictionary[keyValue.Key] = new LocalizedString(keyValue.Key, keyValue.Value, CultureInfo);
            }

            return dictionary.Values.ToImmutableList();
        }

        public void HandleEvent(EntityChangedEventData<ApplicationLanguageText> eventData)
        {
            if (eventData.Entity.LanguageName == CultureInfo.Name)
            {
                _cache.Remove(CalculateCacheKey(eventData.Entity.TenantId));
            }
        }

        private string CalculateCacheKey(int? tenantId)
        {
            return _sourceName + "#" + CultureInfo.Name + "#" + (tenantId ?? 0);
        }

        private Dictionary<string, string> GetAllValuesFromDatabase(int? tenantId)
        {
            return _customLocalizationRepository
                .GetAllList(l => l.LanguageName == CultureInfo.Name && l.TenantId == tenantId)
                .ToDictionary(l => l.Key, l => l.Value);
        }
    }
}