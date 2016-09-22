using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace Abp.Authorization
{
    public abstract class AbpSignInManager<TRole, TUser> : SignInManager<TUser, long>
        where TRole : AbpRole<TUser>, new()
        where TUser : AbpUser<TUser>
    {
        protected AbpSignInManager(
            AbpUserManager<TRole, TUser> userManager, 
            IAuthenticationManager authenticationManager)
            : base(
                  userManager, 
                  authenticationManager)
        {

        }
    }
}
