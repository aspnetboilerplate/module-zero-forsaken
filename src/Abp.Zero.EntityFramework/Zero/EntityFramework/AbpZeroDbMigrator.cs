using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Transactions;
using Abp.Data;
using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.MultiTenancy;

namespace Abp.Zero.EntityFramework
{
    public abstract class AbpZeroDbMigrator<TDbContext, TConfiguration> : IAbpZeroDbMigrator, ITransientDependency
        where TDbContext : DbContext
        where TConfiguration : DbMigrationsConfiguration<TDbContext>, ISupportSeedMode, new()
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
            CreateOrMigrate(
                _connectionStringResolver.GetNameOrConnectionString(null),
                SeedMode.Host
                );
        }

        public virtual void CreateOrMigrateForTenant(AbpTenantBase tenant)
        {
            if (tenant.ConnectionString.IsNullOrEmpty())
            {
                return;
            }

            CreateOrMigrate(
                _connectionStringResolver.GetNameOrConnectionString(tenant.Id),
                SeedMode.Tenant
                );
        }

        protected virtual void CreateOrMigrate(string nameOrConnectionString, SeedMode seedMode)
        {
            nameOrConnectionString = ConnectionStringHelper.GetConnectionString(nameOrConnectionString);

            using (var uow = _unitOfWorkManager.Begin(TransactionScopeOption.Suppress))
            {
                using (var dbContext = _iocResolver.ResolveAsDisposable<TDbContext>(new {nameOrConnectionString = nameOrConnectionString}))
                {
                    var dbInitializer = new MigrateDatabaseToLatestVersion<TDbContext, TConfiguration>(
                        true,
                        new TConfiguration
                        {
                            SeedMode = seedMode
                        });

                    dbInitializer.InitializeDatabase(dbContext.Object);

                    _unitOfWorkManager.Current.SaveChanges();
                    uow.Complete();
                }
            }
        }
    }
}
