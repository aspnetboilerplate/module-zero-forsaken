using Abp.Authorization.Users;
using Abp.Zero.SampleApp.MultiTenancy;
using Abp.Zero.SampleApp.Roles;

namespace Abp.Zero.SampleApp.Users
{
    public class UserManager : AbpUserManager<Tenant, Role, User>
    {
        public UserManager(UserStore userStore, RoleManager roleManager)
            : base(userStore, roleManager)
        {
        }
    }
}