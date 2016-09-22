using Abp.Authorization.Users;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace Abp.Authorization
{
    public class AbpSignInManager<TUser> : SignInManager<TUser, long> where TUser : AbpUser<TUser>
    {
        public AbpSignInManager(
            UserManager<TUser, long> userManager,
            IAuthenticationManager authenticationManager)
            : base(
                userManager,
                authenticationManager)
        {

        }
    }
}
