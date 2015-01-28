using Abp.Authorization.Users;
using ModuleZeroSampleProject.Authorization;
using ModuleZeroSampleProject.MultiTenancy;

namespace ModuleZeroSampleProject.Users
{
    public class UserManager : AbpUserManager<Tenant, Role, User>
    {
        public UserManager(UserStore store, RoleManager roleManager)
            : base(store, roleManager)
        {
        }
    }
}