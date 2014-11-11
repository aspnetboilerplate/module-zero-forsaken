using Abp.Authorization.Roles;

namespace Abp.Tests._TestBasis
{
    public class TestRole : AbpRole<TestTenant, TestUser>
    {
        protected TestRole()
        {

        }

        public TestRole(int? tenantId, string name, string displayName)
            : base(tenantId, name, displayName)
        {

        }
    }
}