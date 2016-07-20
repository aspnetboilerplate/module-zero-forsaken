using System.Transactions;
using Abp.Data;
using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.MultiTenancy;
using Microsoft.EntityFrameworkCore;

namespace Abp.Zero.EntityFrameworkCore
{
    public abstract class AbpZeroDbMigrator<TDbContext> : IAbpZeroDbMigrator, ITransientDependency
        where TDbContext : DbContext
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IDbPerTenantConnectionStringResolver _connectionStringResolver;
        private readonly IIocResolver _iocResolver;

        protected AbpZeroDbMigrator(
            IUnitOfWorkManager unitOfWorkManager, 
            IDbPerTenantConnectionStringResolver connectionStringResolver,
            IIocResolver iocResolver)
        {
            _unitOfWorkManager = unitOfWorkManager;
            _connectionStringResolver = connectionStringResolver;
            _iocResolver = iocResolver;
        }

        public virtual void CreateOrMigrateForHost()
        {
            CreateOrMigrate(null);
        }

        public virtual void CreateOrMigrateForTenant(AbpTenantBase tenant)
        {
            if (tenant.ConnectionString.IsNullOrEmpty())
            {
                return;
            }

            CreateOrMigrate(tenant);
        }

        protected virtual void CreateOrMigrate(AbpTenantBase tenant)
        {
            var args = new DbPerTenantConnectionStringResolveArgs(
                tenant == null ? (int?) null : (int?) tenant.Id,
                tenant == null ? MultiTenancySides.Host : MultiTenancySides.Tenant
                );
            args["DbContextType"] = typeof (TDbContext);
            args["DbContextConcreteType"] = typeof(TDbContext);

            var nameOrConnectionString = ConnectionStringHelper.GetConnectionString(_connectionStringResolver.GetNameOrConnectionString(args));

            using (var uow = _unitOfWorkManager.Begin(TransactionScopeOption.Suppress))
            {
                using (var dbContext = _iocResolver.ResolveAsDisposable<TDbContext>(new {nameOrConnectionString = nameOrConnectionString}))
                {
                    //TODO: Please comple the logic of migrations
                    dbContext.Object.Database.Migrate();
                    

                    _unitOfWorkManager.Current.SaveChanges();
                    uow.Complete();
                }
            }
        }
    }
}
