using System;
using System.Data.Common;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.EntityFramework;
using Abp.Runtime.Session;
using Castle.MicroKernel.Registration;

namespace Abp.Tests._TestBasis
{
    public abstract class TestBase : IDisposable
    {
        protected IIocManager LocalIocManager { get; private set; }

        protected TestDbContext DbContext { get; private set; }

        protected TestSession Session { get; private set; }

        protected TestBase()
        {
            //Temporarily using Activator. It will be changed when ABP make IocManager's constructor public.
            LocalIocManager = (IIocManager)Activator.CreateInstance(typeof(IocManager), true);

            LocalIocManager.IocContainer.Register(
                Component.For<DbConnection>().UsingFactoryMethod(() => Effort.DbConnectionFactory.CreateTransient()).LifestyleSingleton(),
                Component.For<TestDbContext>().LifestyleSingleton(),
                Component.For<IAbpSession, TestSession>().ImplementedBy<TestSession>().LifestyleSingleton(),
                Component.For<IDbContextProvider<TestDbContext>>().ImplementedBy<TestDbContextProvider>().LifestyleSingleton(),

                Component.For(typeof(IRepository<>)).ImplementedBy(typeof(TestEfRepositoryBase<>)).LifestyleTransient(),
                Component.For(typeof(IRepository<,>)).ImplementedBy(typeof(TestEfRepositoryBase<,>)).LifestyleTransient(),
                Component.For<TestUserStore>().LifestyleTransient(),
                Component.For<TestUserManager>().LifestyleTransient()
                );

            DbContext = LocalIocManager.Resolve<TestDbContext>();
            Session = LocalIocManager.Resolve<TestSession>();
        }

        public void Dispose()
        {
            LocalIocManager.Dispose();
        }

        private class TestDbContextProvider : IDbContextProvider<TestDbContext>
        {
            public TestDbContextProvider(TestDbContext dbContext)
            {
                DbContext = dbContext;
            }

            public TestDbContext DbContext { get; private set; }
        }
    }
}
