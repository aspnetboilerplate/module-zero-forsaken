using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Runtime.Remoting.Messaging;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Runtime.Security;
using Abp.UI;
using Abp.Web.Mvc.Models;
using Abp.Zero.Configuration;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using ModuleZeroSampleProject.MultiTenancy;
using ModuleZeroSampleProject.Users;
using ModuleZeroSampleProject.Web.Models.Account;

namespace ModuleZeroSampleProject.Web.Controllers
{
    public class AccountController : ModuleZeroSampleProjectControllerBase
    {
        private readonly UserManager _userManager;

        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<Tenant> _tenantRepository;
        private readonly MultiTenancyConfig _multiTenancy;

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        public AccountController(UserManager userManager, IRepository<User, long> userRepository, IRepository<Tenant> tenantRepository, MultiTenancyConfig multiTenancy)
        {
            _userManager = userManager;
            _userRepository = userRepository;
            _tenantRepository = tenantRepository;
            _multiTenancy = multiTenancy;
        }

        public ActionResult Login(string returnUrl = "")
        {
            if (string.IsNullOrWhiteSpace(returnUrl))
            {
                returnUrl = Request.ApplicationPath;
            }

            ViewBag.ReturnUrl = returnUrl;
            
            return View();
        }

        [UnitOfWork]
        [HttpPost]
        public virtual async Task<JsonResult> Login(LoginViewModel loginModel, string returnUrl = "")
        {
            if (!ModelState.IsValid)
            {
                throw new UserFriendlyException("Your form is invalid!");
            }

            User user;

            if (!_multiTenancy.IsEnabled)
            {
                user = await _userManager.FindAsync(loginModel.UsernameOrEmailAddress, loginModel.Password);
                if (user == null)
                {
                    throw new UserFriendlyException("Invalid user name or password!");
                }
            }
            else if (!string.IsNullOrWhiteSpace(loginModel.TenancyName))
            {
                var tenant = await _tenantRepository.FirstOrDefaultAsync(t => t.TenancyName == loginModel.TenancyName);
                if (tenant == null)
                {
                    throw new UserFriendlyException("No tenant with name: " + loginModel.TenancyName);
                }

                user = await _userRepository.FirstOrDefaultAsync(
                    u =>
                        (u.UserName == loginModel.UsernameOrEmailAddress ||
                         u.EmailAddress == loginModel.UsernameOrEmailAddress)
                        && u.TenantId == tenant.Id
                    );

                if (user == null)
                {
                    throw new UserFriendlyException("Invalid user name or password!");
                }

                var verificationResult = new PasswordHasher().VerifyHashedPassword(user.Password, loginModel.Password);
                if (verificationResult != PasswordVerificationResult.Success)
                {
                    throw new UserFriendlyException("Invalid user name or password!");
                }
            }
            else
            {
                throw new Exception("Tenant is not set!");
            }

            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);

            var identity = await _userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            if (user.TenantId.HasValue)
            {
                identity.AddClaim(new Claim(AbpClaimTypes.TenantId, user.TenantId.Value.ToString(CultureInfo.InvariantCulture)));
            }

            AuthenticationManager.SignIn(new AuthenticationProperties { IsPersistent = loginModel.RememberMe }, identity);

            user.LastLoginTime = DateTime.Now;

            if (string.IsNullOrWhiteSpace(returnUrl))
            {
                returnUrl = Request.ApplicationPath;
            }

            return Json(new MvcAjaxResponse { TargetUrl = returnUrl });
        }

        public ActionResult Logout()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Login");
        }
    }
}