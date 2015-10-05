using Abp.Application.Features;

namespace Abp.MultiTenancy
{
    /// <summary>
    /// Feature setting for a Tenant (<see cref="AbpTenant{TTenant,TUser}"/>).
    /// </summary>
    public class TenantFeatureSetting : FeatureSetting
    {
        /// <summary>
        /// Tenant's Id.
        /// </summary>
        public virtual int TenantId { get; set; }
    }
}