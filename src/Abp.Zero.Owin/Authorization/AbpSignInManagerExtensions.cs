using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.MultiTenancy;
using Abp.Threading;

namespace Abp.Authorization
{
    public static class AbpSignInManagerExtensions
    {
        public static AbpLoginResult<TTenant, TUser> Login<TTenant, TRole, TUser>(
            this AbpSignInManager<TTenant, TRole, TUser> signInManager, 
            string userNameOrEmailAddress, 
            string plainPassword, 
            string tenancyName = null, 
            bool shouldLockout = true)
                where TTenant : AbpTenant<TUser>
                where TRole : AbpRole<TUser>, new()
                where TUser : AbpUser<TUser>
        {
            return AsyncHelper.RunSync(
                () => signInManager.LoginAsync(
                    userNameOrEmailAddress,
                    plainPassword,
                    tenancyName,
                    shouldLockout
                )
            );
        }
    }
}
