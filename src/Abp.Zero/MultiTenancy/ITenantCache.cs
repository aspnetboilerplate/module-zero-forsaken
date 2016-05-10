using System;

namespace Abp.MultiTenancy
{
    public interface ITenantCache
    {
        TenantCacheItem Get(Guid tenantId);
    }
}