using System.Linq;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.Dependency;
using Abp.Runtime.Session;
using Castle.Core.Logging;
using Microsoft.AspNet.Identity;

namespace Abp.Authorization
{
    public class PermissionGrantStore : IPermissionGrantStore, ITransientDependency
    {
        private readonly AbpRoleManager _roleManager;
        private readonly AbpUserManager _userManager;

        public ILogger Logger { get; set; }

        public IAbpSession AbpSession { get; set; }

        public PermissionGrantStore(AbpRoleManager roleManager, AbpUserManager userManager)
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
