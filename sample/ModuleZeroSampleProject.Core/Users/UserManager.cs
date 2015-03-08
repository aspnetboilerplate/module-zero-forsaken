using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.Zero.Configuration;
using ModuleZeroSampleProject.Authorization;
using ModuleZeroSampleProject.MultiTenancy;

namespace ModuleZeroSampleProject.Users
{
    public class UserManager : AbpUserManager<Tenant, Role, User>
    {
        public UserManager(UserStore store, RoleManager roleManager, IRepository<Tenant> tenantRepository, MultiTenancyConfig multiTenancyConfig)
            : base(store, roleManager, tenantRepository, multiTenancyConfig)
        {
        }
    }
}