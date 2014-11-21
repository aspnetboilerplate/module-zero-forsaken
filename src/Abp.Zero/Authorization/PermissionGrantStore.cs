using System.Linq;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.Dependency;
using Abp.MultiTenancy;
using Abp.Runtime.Session;
using Castle.Core.Logging;
using Microsoft.AspNet.Identity;

namespace Abp.Authorization
{
    public abstract class PermissionGrantStore<TRole, TTenant, TUser> : IPermissionGrantStore, ITransientDependency
        where TRole : AbpRole<TTenant, TUser> 
        where TUser : AbpUser<TTenant, TUser> 
        where TTenant : AbpTenant<TTenant, TUser>
    {
        private readonly AbpRoleManager<TTenant, TRole, TUser> _roleManager;
        private readonly AbpUserManager<TTenant,TRole, TUser> _userManager;

        public ILogger Logger { get; set; }

        public IAbpSession AbpSession { get; set; }

        public PermissionGrantStore(AbpRoleManager<TTenant, TRole, TUser> roleManager, AbpUserManager<TTenant, TRole, TUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
    
            Logger = NullLogger.Instance;
            AbpSession = NullAbpSession.Instance;
        }

        public bool IsGranted(long userId, string permissionName)
        {
            return _userManager
                .GetRoles(userId)
                .Any(roleName => _roleManager.HasPermission(roleName, permissionName));
        }
    }
}
