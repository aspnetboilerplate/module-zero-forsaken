using System;
using System.Configuration;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Runtime.Security;
using Abp.UI;
using Abp.Web.Mvc.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using ModuleZeroSampleProject.MultiTenancy;
using ModuleZeroSampleProject.Users;
using ModuleZeroSampleProject.Web.Models.Home;

namespace ModuleZeroSampleProject.Web.Controllers
{
    public class AccountController : ModuleZeroSampleProjectControllerBase
    {
        private readonly ModuleZeroSampleProjectUserManager _userManager;

        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<Tenant> _tenantRepository;

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        public AccountController(ModuleZeroSampleProjectUserManager userManager, IRepository<User, long> userRepository, IRepository<Tenant> tenantRepository)
        {
            _userManager = userManager;
            _userRepository = userRepository;
            _tenantRepository = tenantRepository;
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
        public virtual JsonResult Login(LoginViewModel loginModel, string returnUrl = "")
        {
            if (!ModelState.IsValid)
            {
                throw new UserFriendlyException("Your form is invalid!");
            }

            User user;

            if (!IsMultiTenancyEnabled())
            {
                user = _userManager.Find(loginModel.UsernameOrEmailAddress, loginModel.Password);
            }
            else if (!string.IsNullOrWhiteSpace(loginModel.TenancyName))
            {
                var tenant = _tenantRepository.FirstOrDefault(t => t.TenancyName == loginModel.TenancyName);
                if (tenant == null)
                {
                    throw new UserFriendlyException("No tenant with name: " + loginModel.TenancyName);
                }

                user = _userRepository.FirstOrDefault(
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
            var identity = _userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie).Result;
            identity.AddClaim(new Claim(AbpClaimTypes.TenantId, user.TenantId.HasValue ? user.TenantId.Value.ToString() : null));

            AuthenticationManager.SignIn(new AuthenticationProperties { IsPersistent = loginModel.RememberMe }, identity);

            user.LastLoginTime = DateTime.Now;

            if (string.IsNullOrWhiteSpace(returnUrl))
            {
                returnUrl = Request.ApplicationPath;
            }

            return Json(new MvcAjaxResponse { TargetUrl = returnUrl });
        }

        private static bool IsMultiTenancyEnabled()
        {
            return string.Equals(ConfigurationManager.AppSettings["Abp.MultiTenancy.IsEnabled"], "true", StringComparison.InvariantCultureIgnoreCase);
        }
    }
}