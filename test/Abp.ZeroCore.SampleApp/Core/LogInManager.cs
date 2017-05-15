using Abp.Authorization;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.Configuration;
using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Zero.Configuration;
using Microsoft.AspNetCore.Identity;

namespace Abp.ZeroCore.SampleApp.Core
{
    public class LogInManager : AbpLogInManager<Tenant, Role, User>
    {
        public LogInManager(
            AbpUserManager<Role, User> userManager, 
            IMultiTenancyConfig multiTenancyConfig, 
            IRepository<Tenant> tenantRepository, 
            IUnitOfWorkManager unitOfWorkManager, 
            ISettingManager settingManager, 
            IRepository<UserLoginAttempt, long> userLoginAttemptRepository, 
            IUserManagementConfig userManagementConfig, 
            IIocResolver iocResolver, 
            IPasswordHasher<User> passwordHasher, 
            AbpRoleManager<Role, User> roleManager,
            UserClaimsPrincipalFactory claimsPrincipalFactory
            ) : base(
                userManager, 
                multiTenancyConfig, 
                tenantRepository, 
                unitOfWorkManager, 
                settingManager, 
                userLoginAttemptRepository, 
                userManagementConfig, 
                iocResolver, 
                passwordHasher, 
                roleManager, 
                claimsPrincipalFactory)
        {
        }
    }
}
