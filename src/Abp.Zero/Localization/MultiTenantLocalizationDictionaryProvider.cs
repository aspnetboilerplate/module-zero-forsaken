using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Abp.Collections.Extensions;
using Abp.Dependency;
using Abp.Localization.Dictionaries;

namespace Abp.Localization
{
    public class MultiTenantLocalizationDictionaryProvider : ILocalizationDictionaryProvider
    {
        public ILocalizationDictionary DefaultDictionary
        {
            get { return GetDefaultDictionary(); }
        }

        public IDictionary<string, ILocalizationDictionary> Dictionaries
        {
            get { return GetDictionaries(); }
        }

        private readonly ConcurrentDictionary<string, ILocalizationDictionary> _dictionaries;

        private string _sourceName;

        private readonly ILocalizationDictionaryProvider _internalProvider;

        private readonly IIocManager _iocManager;
        private ILanguageManager _languageManager;

        public MultiTenantLocalizationDictionaryProvider(ILocalizationDictionaryProvider internalProvider, IIocManager iocManager)
        {
            _internalProvider = internalProvider;
            _iocManager = iocManager;
            _dictionaries = new ConcurrentDictionary<string, ILocalizationDictionary>();
        }

        public void Initialize(string sourceName)
        {
            _sourceName = sourceName;
            _languageManager = _iocManager.Resolve<ILanguageManager>();
            _internalProvider.Initialize(_sourceName);
        }

        private IDictionary<string, ILocalizationDictionary> GetDictionaries()
        {
            var languages = _languageManager.GetLanguages();

            foreach (var language in languages)
            {
                _dictionaries.GetOrAdd(language.Name, s => CreateLocalizationDictionary(language));
            }

            return _dictionaries;
        }

        private ILocalizationDictionary GetDefaultDictionary()
        {
            var defaultLanguage = _languageManager.GetLanguages().FirstOrDefault(l => l.IsDefault);
            if (defaultLanguage == null)
            {
                throw new ApplicationException("Default language is not defined!");
            }

            return _dictionaries.GetOrAdd(defaultLanguage.Name, s => CreateLocalizationDictionary(defaultLanguage));
        }

        private IMultiTenantLocalizationDictionary CreateLocalizationDictionary(LanguageInfo language)
        {
            var internalDictionary =
                _internalProvider.Dictionaries.GetOrDefault(language.Name) ??
                new EmptyDictionary(CultureInfo.GetCultureInfo(language.Name));

            return _iocManager.Resolve<IMultiTenantLocalizationDictionary>(new
            {
                sourceName = _sourceName,
                internalDictionary = internalDictionary
            });
        }
    }
}