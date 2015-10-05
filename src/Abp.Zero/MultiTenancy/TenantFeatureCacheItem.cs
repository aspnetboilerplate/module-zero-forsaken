using System;
using System.Collections.Generic;

namespace Abp.MultiTenancy
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class TenantFeatureCacheItem
    {
        public const string CacheStoreName = "AbpZeroTenantFeatures";

        public int? EditionId { get; set; }

        public IDictionary<string, string> FeatureValues { get; set; }

        public TenantFeatureCacheItem()
        {
            FeatureValues = new Dictionary<string, string>();
        }
    }
}