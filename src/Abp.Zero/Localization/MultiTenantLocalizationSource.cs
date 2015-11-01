using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.Extensions;
using Abp.Localization.Dictionaries;
using Abp.Logging;
using Castle.Core.Logging;

//TODO: No need to inherit from DictionaryBasedLocalizationSource

namespace Abp.Localization
{
    public class MultiTenantLocalizationSource : DictionaryBasedLocalizationSource, IMultiTenantLocalizationSource
    {
        public ILogger Logger { get; set; }

        public MultiTenantLocalizationSource(string name, MultiTenantLocalizationDictionaryProvider dictionaryProvider) 
            : base(name, dictionaryProvider)
        {
            Logger = NullLogger.Instance;
        }

        public override void Initialize(ILocalizationConfiguration configuration, IIocResolver iocResolver)
        {
            base.Initialize(configuration, iocResolver);

            if (Logger is NullLogger && iocResolver.IsRegistered(typeof(ILoggerFactory)))
            {
                Logger = iocResolver.Resolve<ILoggerFactory>().Create(typeof (MultiTenantLocalizationSource));
            }
        }

        public string GetString(int? tenantId, string name, CultureInfo culture)
        {
            var value = GetStringOrNull(tenantId, name, culture);

            if (value == null)
            {
                return ReturnGivenNameOrThrowException(name);
            }

            return value;
        }

        public string GetStringOrNull(int? tenantId, string name, CultureInfo culture, bool tryDefaults = true)
        {
            var cultureCode = culture.Name;
            var dictionaries = DictionaryProvider.Dictionaries;

            //Try to get from original dictionary (with country code)
            ILocalizationDictionary originalDictionary;
            if (dictionaries.TryGetValue(cultureCode, out originalDictionary))
            {
                var strOriginal = originalDictionary.As<IMultiTenantLocalizationDictionary>().GetOrNull(tenantId, name);
                if (strOriginal != null)
                {
                    return strOriginal.Value;
                }
            }

            if (!tryDefaults)
            {
                return null;
            }

            //Try to get from same language dictionary (without country code)
            if (cultureCode.Length == 5) //Example: "tr-TR" (length=5)
            {
                var langCode = cultureCode.Substring(0, 2);
                ILocalizationDictionary langDictionary;
                if (dictionaries.TryGetValue(langCode, out langDictionary))
                {
                    var strLang = langDictionary.As<IMultiTenantLocalizationDictionary>().GetOrNull(tenantId, name);
                    if (strLang != null)
                    {
                        return strLang.Value;
                    }
                }
            }

            //Try to get from default language
            var defaultDictionary = DictionaryProvider.DefaultDictionary;
            if (defaultDictionary == null)
            {
                return null;
            }

            var strDefault = defaultDictionary.As<IMultiTenantLocalizationDictionary>().GetOrNull(tenantId, name);
            if (strDefault == null)
            {
                return null;
            }

            return strDefault.Value;
        }

        //public IReadOnlyList<LocalizedString> GetAllStrings(int? tenantId, CultureInfo culture, bool includeDefaults = true)
        //{
        //    //TODO: Can be optimized (example: if it's already default dictionary, skip overriding)

        //    var dictionaries = DictionaryProvider.Dictionaries;

        //    //Create a temp dictionary to build
        //    var allStrings = new Dictionary<string, LocalizedString>();

        //    if (includeDefaults)
        //    {
        //        //Fill all strings from default dictionary
        //        var defaultDictionary = DictionaryProvider.DefaultDictionary;
        //        if (defaultDictionary != null)
        //        {
        //            foreach (var defaultDictString in defaultDictionary.As<IMultiTenantLocalizationDictionary>().GetAllStrings(tenantId))
        //            {
        //                allStrings[defaultDictString.Name] = defaultDictString;
        //            }
        //        }

        //        //Overwrite all strings from the language based on country culture
        //        if (culture.Name.Length == 5)
        //        {
        //            ILocalizationDictionary langDictionary;
        //            if (dictionaries.TryGetValue(culture.Name.Substring(0, 2), out langDictionary))
        //            {
        //                foreach (var langString in langDictionary.As<IMultiTenantLocalizationDictionary>().GetAllStrings(tenantId))
        //                {
        //                    allStrings[langString.Name] = langString;
        //                }
        //            }
        //        }
        //    }

        //    //Overwrite all strings from the original dictionary
        //    ILocalizationDictionary originalDictionary;
        //    if (dictionaries.TryGetValue(culture.Name, out originalDictionary))
        //    {
        //        foreach (var originalLangString in originalDictionary.As<IMultiTenantLocalizationDictionary>().GetAllStrings())
        //        {
        //            allStrings[originalLangString.Name] = originalLangString;
        //        }
        //    }

        //    return allStrings.Values.ToImmutableList();
        //}
    }
}