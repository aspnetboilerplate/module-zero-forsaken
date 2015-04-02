using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Configuration.Startup;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
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
            IMultiTenancyConfig multiTenancyConfig,
            IPermissionManager permissionManager,
            IUnitOfWorkManager unitOfWorkManager)
            : base(
            userStore,
            roleManager,
            tenantRepository,
            multiTenancyConfig,
            permissionManager,
            unitOfWorkManager)
        {
        }
    }
}