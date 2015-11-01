using System.Collections.Generic;
using Abp.Runtime.Caching;

namespace Abp.Localization
{
    public static class MultiTenantLocalizationDictionaryCacheHelper
    {
        public const string CacheName = "AbpZeroMultiTenantLocalizationDictionaryCache";

        public static ITypedCache<string, Dictionary<string, string>> GetMultiTenantLocalizationDictionaryCache(this ICacheManager cacheManager)
        {
            return cacheManager.GetCache(CacheName).AsTyped<string, Dictionary<string, string>>();
        }

        public static string CalculateCacheKey(int? tenantId, string sourceName, string languageName)
        {
            return sourceName + "#" + languageName + "#" + (tenantId ?? 0);
        }
    }
}