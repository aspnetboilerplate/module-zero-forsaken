using Abp.Configuration.Startup;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.MultiTenancy;
using Abp.Runtime.Session;

namespace Abp.Zero.EntityFramework
{
    public class DbPerTenantConnectionStringResolver : DefaultConnectionStringResolver, IDbPerTenantConnectionStringResolver
    {
        public IAbpSession AbpSession { get; set; }

        private readonly ICurrentUnitOfWorkProvider _currentUnitOfWorkProvider;
        private readonly ITenantCache _tenantCache;

        public DbPerTenantConnectionStringResolver(
            IAbpStartupConfiguration configuration, 
            ICurrentUnitOfWorkProvider currentUnitOfWorkProvider,
            ITenantCache tenantCache
            ) 
            : base(configuration)
        {
            _currentUnitOfWorkProvider = currentUnitOfWorkProvider;
            _tenantCache = tenantCache;

            AbpSession = NullAbpSession.Instance;
        }

        public override string GetNameOrConnectionString(MultiTenancySides? multiTenancySide = null)
        {
            if (multiTenancySide == MultiTenancySides.Host)
            {
                return GetNameOrConnectionString(null);
            }

            return GetNameOrConnectionString(GetCurrentTenantId());
        }

        public string GetNameOrConnectionString(int? tenantId)
        {
            if (tenantId == null)
            {
                return base.GetNameOrConnectionString();
            }

            var tenantCacheItem = _tenantCache.Get(tenantId.Value);
            if (tenantCacheItem.ConnectionString.IsNullOrEmpty())
            {
                return base.GetNameOrConnectionString();
            }

            return tenantCacheItem.ConnectionString;
        }
        
        private int? GetCurrentTenantId()
        {
            if (_currentUnitOfWorkProvider.Current != null)
            {
                return _currentUnitOfWorkProvider.Current.GetTenantId();
            }

            return AbpSession.TenantId;
        }
    }
}
