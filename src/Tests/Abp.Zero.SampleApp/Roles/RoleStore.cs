using Abp.Authorization.Roles;
using Abp.Domain.Repositories;
using Abp.Zero.SampleApp.MultiTenancy;
using Abp.Zero.SampleApp.Users;

namespace Abp.Zero.SampleApp.Roles
{
    public class RoleStore : AbpRoleStore<Tenant, Role, User>
    {
        public RoleStore(IRepository<Role> roleRepository, IRepository<RolePermissionSetting, long> rolePermissionSettingRepository)
            : base(roleRepository, rolePermissionSettingRepository)
        {
        }
    }
}