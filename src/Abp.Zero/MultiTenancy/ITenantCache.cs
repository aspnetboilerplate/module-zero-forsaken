namespace Abp.MultiTenancy
{
    public interface ITenantCache
    {
        TenantCacheItem Get(int tenantId);
    }
}