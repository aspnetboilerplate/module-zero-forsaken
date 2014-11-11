using System.Data.Common;
using Abp.Zero.EntityFramework;

namespace Abp.Tests._TestBasis
{
    public class TestDbContext : AbpZeroDbContext<TestTenant, TestRole, TestUser>
    {
        public TestDbContext(DbConnection connection)
            : base(connection, true)
        {

        }
    }
}