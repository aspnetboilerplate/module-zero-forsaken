using Abp.MultiTenancy;

namespace Abp.Tests._TestBasis
{
    public class TestTenant : AbpTenant<TestTenant, TestUser>
    {
        protected TestTenant()
        {

        }

        public TestTenant(string tenancyName, string name)
            : base(tenancyName, name)
        {
        }
    }
}