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

        private static List<int> passedThreads = new List<int>();

        public ActionResult Login(string returnUrl = "")
        {
            if (string.IsNullOrWhiteSpace(returnUrl))
            {
                returnUrl = Request.ApplicationPath;
            }

            var vaa1 = CallContext.LogicalGetData("halil") as string;
            if (vaa1 != null)
            {
            }

            if (passedThreads.Contains(Thread.CurrentThread.ManagedThreadId))
            {
                
            }

            passedThreads.Add(Thread.CurrentThread.ManagedThreadId);


            CallContext.LogicalSetData("halil", "42");

            AsyncHelper2.RunSync(async () =>
                                 {
                                     Thread.Sleep(1000);
                                     var vaa = CallContext.LogicalGetData("halil");
                                     if (vaa == null)
                                     {
                                         
                                     }
                                     //var usr = await _userRepository.GetAsync(1);
                                     var x = 5;
                                 });

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
            identity.AddClaim(new Claim(AbpClaimTypes.TenantId, user.TenantId.HasValue ? user.TenantId.Value.ToString() : null));
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

    internal static class AsyncHelper2
    {
        private static readonly TaskFactory _myTaskFactory = new TaskFactory(CancellationToken.None, TaskCreationOptions.None, TaskContinuationOptions.None, TaskScheduler.Default);

        public static TResult RunSync<TResult>(Func<Task<TResult>> func)
        {
            CultureInfo cultureUi = CultureInfo.CurrentUICulture;
            CultureInfo culture = CultureInfo.CurrentCulture;
            return System.Threading.Tasks.TaskExtensions.Unwrap<TResult>(AsyncHelper2._myTaskFactory.StartNew<Task<TResult>>((Func<Task<TResult>>)(() =>
            {
                Thread.CurrentThread.CurrentCulture = culture;
                Thread.CurrentThread.CurrentUICulture = cultureUi;
                return func();
            }))).GetAwaiter().GetResult();
        }

        public static void RunSync(Func<Task> func)
        {
            CultureInfo cultureUi = CultureInfo.CurrentUICulture;
            CultureInfo culture = CultureInfo.CurrentCulture;
            System.Threading.Tasks.TaskExtensions.Unwrap(AsyncHelper2._myTaskFactory.StartNew<Task>((Func<Task>)(() =>
            {
                Thread.CurrentThread.CurrentCulture = culture;
                Thread.CurrentThread.CurrentUICulture = cultureUi;
                return func();
            }))).GetAwaiter().GetResult();
        }
    }
}