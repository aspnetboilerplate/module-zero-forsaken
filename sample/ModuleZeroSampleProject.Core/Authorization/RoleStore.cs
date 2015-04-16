using Abp.Authorization.Roles;
using Abp.Domain.Repositories;
using ModuleZeroSampleProject.MultiTenancy;
using ModuleZeroSampleProject.Users;

namespace ModuleZeroSampleProject.Authorization
{
    public class RoleStore : AbpRoleStore<Tenant, Role, User>
    {
        public RoleStore(
            IRepository<Role> roleRepository,
            IRepository<RolePermissionSetting, long> rolePermissionSettingRepository)
            : base(
                roleRepository,
                rolePermissionSettingRepository)
        {
        }
    }
}