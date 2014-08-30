namespace Abp.MultiTenancy
{
    /// <summary>
    /// Implement this interface for an entity which may optionally have TenantId.
    /// </summary>
    public interface IMayHaveTenant
    {
        int? TenantId { get; set; }
    }
}