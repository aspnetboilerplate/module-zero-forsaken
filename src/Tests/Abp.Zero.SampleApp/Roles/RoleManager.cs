using Abp.Authorization;
using Abp.Authorization.Roles;
using Abp.Domain.Uow;
using Abp.Zero.Configuration;
using Abp.Zero.SampleApp.MultiTenancy;
using Abp.Zero.SampleApp.Users;

namespace Abp.Zero.SampleApp.Roles
{
    public class RoleManager : AbpRoleManager<Tenant, Role, User>
    {
        public RoleManager(RoleStore store, IPermissionManager permissionManager, IRoleManagementConfig roleManagementConfig, IUnitOfWorkManager unitOfWorkManager)
            : base(store, permissionManager, roleManagementConfig, unitOfWorkManager)
        {
        }
    }
}