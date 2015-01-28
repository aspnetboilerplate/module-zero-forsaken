using Abp.Authorization.Roles;
using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using ModuleZeroSampleProject.MultiTenancy;
using ModuleZeroSampleProject.Users;

namespace ModuleZeroSampleProject.Authorization
{
    public class RoleStore : AbpRoleStore<Tenant, Role, User>
    {
        public RoleStore(
            IRepository<Role> roleRepository,
            IRepository<RolePermissionSetting, long> rolePermissionSettingRepository,
            IAbpSession session)
            : base(
                roleRepository,
                rolePermissionSettingRepository,
                session)
        {
        }
    }
}