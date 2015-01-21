using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.Dependency;
using Abp.MultiTenancy;
using Abp.Runtime.Session;
using Castle.Core.Logging;

namespace Abp.Authorization
{
    public abstract class PermissionChecker<TTenant, TRole, TUser> : IPermissionChecker, ITransientDependency
        where TRole : AbpRole<TTenant, TUser> 
        where TUser : AbpUser<TTenant, TUser> 
        where TTenant : AbpTenant<TTenant, TUser>
    {
        private readonly AbpUserManager<TTenant,TRole, TUser> _userManager;

        public ILogger Logger { get; set; }

        public IAbpSession AbpSession { get; set; }

        protected PermissionChecker(AbpUserManager<TTenant, TRole, TUser> userManager)
        {
            _userManager = userManager;
    
            Logger = NullLogger.Instance;
            AbpSession = NullAbpSession.Instance;
        }

        public bool IsGranted(string permissionName)
        {
            return AbpSession.UserId.HasValue && _userManager.IsGranted(AbpSession.UserId.Value, permissionName);
        }

        public bool IsGranted(long userId, string permissionName)
        {
            return _userManager.IsGranted(userId, permissionName);
        }
    }
}
