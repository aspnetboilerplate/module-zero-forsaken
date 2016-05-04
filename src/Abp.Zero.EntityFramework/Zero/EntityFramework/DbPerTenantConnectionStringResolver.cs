using Abp.Configuration.Startup;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.MultiTenancy;
using Abp.Runtime.Session;

namespace Abp.Zero.EntityFramework
{
    /// <summary>
    /// Implements <see cref="IDbPerTenantConnectionStringResolver"/> to dynamically resolve
    /// connection string for a multi tenant application.
    /// </summary>
    public class DbPerTenantConnectionStringResolver : DefaultConnectionStringResolver, IDbPerTenantConnectionStringResolver
    {
        /// <summary>
        /// Reference to the session.
        /// </summary>
        public IAbpSession AbpSession { get; set; }

        private readonly ICurrentUnitOfWorkProvider _currentUnitOfWorkProvider;
        private readonly ITenantCache _tenantCache;

        /// <summary>
        /// Initializes a new instance of the <see cref="DbPerTenantConnectionStringResolver"/> class.
        /// </summary>
        public DbPerTenantConnectionStringResolver(
            IAbpStartupConfiguration configuration, 
            ICurrentUnitOfWorkProvider currentUnitOfWorkProvider,
            ITenantCache tenantCache) 
            : base(
                  configuration)
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
                //Requested for host
                return base.GetNameOrConnectionString();
            }

            var tenantCacheItem = _tenantCache.Get(tenantId.Value);
            if (tenantCacheItem.ConnectionString.IsNullOrEmpty())
            {
                //Tenant has not dedicated database
                return base.GetNameOrConnectionString();
            }

            return tenantCacheItem.ConnectionString;
        }
        
        private int? GetCurrentTenantId()
        {
            return _currentUnitOfWorkProvider.Current != null
                ? _currentUnitOfWorkProvider.Current.GetTenantId()
                : AbpSession.TenantId;
        }
    }
}
