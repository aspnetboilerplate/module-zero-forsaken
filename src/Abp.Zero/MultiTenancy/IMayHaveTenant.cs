namespace Abp.MultiTenancy
{
    /// <summary>
    /// Implement this interface for an entity which may optionally have TenantId.
    /// </summary>
    public interface IMayHaveTenant
    {
        /// <summary>
        /// Id of the tenant.
        /// </summary>
        int? TenantId { get; set; }
    }
}