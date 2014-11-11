using System;
using System.Data.Common;
using Abp.Dependency;

namespace Abp.Tests._TestBasis
{
    public abstract class TestBase : IDisposable
    {
        protected IIocManager LocalIocManager { get; private set; }

        protected TestDbContext DbContext { get; private set; }

        private DbConnection Connection { get; set; }

        protected TestBase()
        {
            //Temporarily using Activator. It will be changed when ABP make IocManager's constructor public.
            LocalIocManager = (IIocManager)Activator.CreateInstance(typeof(IocManager), true);
            Connection = Effort.DbConnectionFactory.CreateTransient();
            DbContext = new TestDbContext(Connection);
        }
        
        public void Dispose()
        {
            DbContext.Dispose();
            Connection.Dispose();
        }
    }
}
