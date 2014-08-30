namespace Abp.MultiTenancy
{
    /// <summary>
    /// Implement this interface for an entity which must have TenantId.
    /// </summary>
    public interface IMustHaveTenant
    {
        int TenantId { get; set; }
    }
}
