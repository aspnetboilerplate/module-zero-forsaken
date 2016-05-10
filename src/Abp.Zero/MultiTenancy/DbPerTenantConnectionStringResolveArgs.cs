using Abp.Domain.Uow;
using System;

namespace Abp.MultiTenancy
{
    public class DbPerTenantConnectionStringResolveArgs : ConnectionStringResolveArgs
    {
        public Guid? TenantId { get; set; }

        public DbPerTenantConnectionStringResolveArgs(Guid? tenantId, MultiTenancySides? multiTenancySide = null)
            : base(multiTenancySide)
        {
            TenantId = tenantId;
        }

        public DbPerTenantConnectionStringResolveArgs(Guid? tenantId, ConnectionStringResolveArgs baseArgs)
        {
            TenantId = tenantId;
            MultiTenancySide = baseArgs.MultiTenancySide;

            foreach (var kvPair in baseArgs)
            {
                Add(kvPair.Key, kvPair.Value);
            }
        }
    }
}