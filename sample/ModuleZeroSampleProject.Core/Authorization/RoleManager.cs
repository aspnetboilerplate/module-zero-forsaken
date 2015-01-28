using Abp.Authorization;
using Abp.Authorization.Roles;
using ModuleZeroSampleProject.MultiTenancy;
using ModuleZeroSampleProject.Users;

namespace ModuleZeroSampleProject.Authorization
{
    public class RoleManager : AbpRoleManager<Tenant, Role, User>
    {
        public RoleManager(RoleStore store, IPermissionManager permissionManager)
            : base(store, permissionManager)
        {
        }
    }
}