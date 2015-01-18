using Abp.Authorization;
using Abp.Authorization.Roles;

namespace Abp.Tests._TestBasis
{
    public class TestRoleManager : AbpRoleManager<TestTenant, TestRole, TestUser>
    {
        public TestRoleManager(TestRoleStore store, IPermissionManager permissionManager)
            : base(store, permissionManager)
        {
        }
    }
}