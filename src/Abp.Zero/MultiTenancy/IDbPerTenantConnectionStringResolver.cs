using Abp.Domain.Uow;

namespace Abp.MultiTenancy
{
    public interface IDbPerTenantConnectionStringResolver : IConnectionStringResolver
    {
        string GetNameOrConnectionString(int? tenantId);
    }
}
