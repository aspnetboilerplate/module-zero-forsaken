using Abp.Domain.Uow;

namespace Abp.MultiTenancy
{
    /// <summary>
    /// Extends <see cref="IConnectionStringResolver"/> to
    /// get connection string for given tenant.
    /// </summary>
    public interface IDbPerTenantConnectionStringResolver : IConnectionStringResolver
    {
        /// <summary>
        /// Gets the connection string for given tenantId.
        /// </summary>
        /// <param name="tenantId">Tenant Id. Can be null to get host connection string.</param>
        string GetNameOrConnectionString(int? tenantId);
    }
}
