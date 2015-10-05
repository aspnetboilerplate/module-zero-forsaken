using System;
using System.Collections.Generic;

namespace Abp.Application.Editions
{
    [Serializable]
    public class EditionfeatureCacheItem
    {
        public const string CacheStoreName = "AbpZeroRolePermissions";

        public IDictionary<string, string> FeatureValues { get; set; }

        public EditionfeatureCacheItem()
        {
            FeatureValues = new Dictionary<string, string>();
        }
    }
}