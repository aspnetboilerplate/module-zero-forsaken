namespace Abp.MultiTenancy
{
    /// <summary>
    /// Implement this interface for an entity which must have TenantId.
    /// </summary>
    public interface IMustHaveTenant : IFilterByTenant
    {
        /// <summary>
        /// Id of the tenant.
        /// </summary>
        int TenantId { get; set; }
    }
}
