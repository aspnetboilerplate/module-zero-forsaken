using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.Zero.Configuration;
using Abp.Zero.SampleApp.MultiTenancy;
using Abp.Zero.SampleApp.Roles;

namespace Abp.Zero.SampleApp.Users
{
    public class UserManager : AbpUserManager<Tenant, Role, User>
    {
        public UserManager(
            UserStore userStore,
            RoleManager roleManager,
            IRepository<Tenant> tenantRepository,
            MultiTenancyConfig multiTenancyConfig,
            IPermissionManager permissionManager)
            : base(
            userStore,
            roleManager,
            tenantRepository,
            multiTenancyConfig,
            permissionManager)
        {
        }
    }
}