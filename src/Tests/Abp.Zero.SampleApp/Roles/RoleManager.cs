using Abp.Authorization;
using Abp.Authorization.Roles;
using Abp.Zero.SampleApp.MultiTenancy;
using Abp.Zero.SampleApp.Users;

namespace Abp.Zero.SampleApp.Roles
{
    public class RoleManager : AbpRoleManager<Tenant, Role, User>
    {
        public RoleManager(RoleStore store, IPermissionManager permissionManager)
            : base(store, permissionManager)
        {
        }
    }
}