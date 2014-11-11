using Abp.Zero.EntityFramework;

namespace Abp.Tests._TestBasis
{
    public class TestDbContext : AbpZeroDbContext<TestTenant, TestRole, TestUser>
    {

    }
}