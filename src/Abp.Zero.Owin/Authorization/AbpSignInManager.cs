using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.Domain.Uow;
using Abp.MultiTenancy;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace Abp.Authorization
{
    public abstract class AbpSignInManager<TTenant, TRole, TUser> : SignInManager<TUser, long>
        where TTenant : AbpTenant<TUser>
        where TRole : AbpRole<TUser>, new()
        where TUser : AbpUser<TUser>
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        protected AbpSignInManager(
            AbpUserManager<TRole, TUser> userManager,
            IAuthenticationManager authenticationManager,
            IUnitOfWorkManager unitOfWorkManager)
            : base(
                  userManager,
                  authenticationManager)
        {
            _unitOfWorkManager = unitOfWorkManager;
        }

        /// <summary>
        /// This method can return two results:
        /// <see cref="SignInStatus.Success"/> indicates that user has successfully signed in.
        /// <see cref="SignInStatus.RequiresVerification"/> indicates that user has successfully signed in.
        /// </summary>
        /// <param name="loginResult">The login result received from <see cref="AbpLogInManager{TTenant,TRole,TUser}"/> Should be Success.</param>
        /// <param name="isPersistent">True to use persistent cookie.</param>
        /// <param name="rememberBrowser">Remember user's browser (and not use two factor auth again) or not.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">loginResult.Result should be success in order to sign in!</exception>
        [UnitOfWork]
        public virtual async Task<SignInStatus> SignInOrTwoFactor(AbpLoginResult<TTenant, TUser> loginResult, bool isPersistent, bool rememberBrowser = false)
        {
            if (loginResult.Result != AbpLoginResultType.Success)
            {
                throw new ArgumentException("loginResult.Result should be success in order to sign in!");
            }

            using (_unitOfWorkManager.Current.SetTenantId(loginResult.Tenant?.Id))
            {
                if (await UserManager.GetTwoFactorEnabledAsync(loginResult.User.Id))
                {
                    if ((await UserManager.GetValidTwoFactorProvidersAsync(loginResult.User.Id)).Count > 0)
                    {
                        if (!await AuthenticationManager.TwoFactorBrowserRememberedAsync(loginResult.User.Id.ToString()))
                        {
                            var claimsIdentity = new ClaimsIdentity(DefaultAuthenticationTypes.TwoFactorCookie);
                            claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, loginResult.User.Id.ToString()));
                            AuthenticationManager.SignIn(claimsIdentity);
                            return SignInStatus.RequiresVerification;
                        }
                    }
                }

                await SignInAsync(loginResult.User, isPersistent, rememberBrowser);
                return SignInStatus.Success;
            }
        }
    }
}
