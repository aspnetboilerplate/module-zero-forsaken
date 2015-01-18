using Abp.Authorization.Roles;
using Abp.Domain.Repositories;
using Abp.Runtime.Session;

namespace Abp.Tests._TestBasis
{
    public class TestRoleStore : AbpRoleStore<TestTenant, TestRole, TestUser>
    {
        public TestRoleStore(IRepository<TestRole> roleRepository, IRepository<RolePermissionSetting, long> rolePermissionSettingRepository, IAbpSession session)
            : base(roleRepository, rolePermissionSettingRepository, session)
        {
        }
    }
}