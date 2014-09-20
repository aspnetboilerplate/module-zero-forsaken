using System.Linq;
using Abp.Authorization.Permissions;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.Dependency;
using Abp.Runtime.Session;
using Castle.Core.Logging;
using Microsoft.AspNet.Identity;

namespace Abp.Authorization
{
    //TODO: Move this to Abp.Zero module since it should be open source!
    public class AuthorizationService : IAuthorizationService, ITransientDependency
    {
        private readonly IPermissionManager _permissionManager;
        private readonly AbpRoleManager _roleManager;
        private readonly AbpUserManager _userManager;

        public ILogger Logger { get; set; }

        public IAbpSession AbpSession { get; set; }

        public AuthorizationService(
            IPermissionManager permissionManager,
            AbpRoleManager roleManager,
            AbpUserManager userManager)
        {
            _permissionManager = permissionManager;
            _roleManager = roleManager;
            _userManager = userManager;
    
            Logger = NullLogger.Instance;
            AbpSession = NullAbpSession.Instance;
        }

        public bool HasAnyOfPermissions(string[] permissionNames)
        {
            return permissionNames.Any(HasPermission);
        }

        public bool HasAllOfPermissions(string[] permissionNames)
        {
            return permissionNames.All(HasPermission);
        }

        public string[] GetGrantedPermissionNames()
        {
            return _permissionManager.GetAllPermissionNames().Where(HasPermission).ToArray(); //TODO@Halil: Must be optimized!!
        }

        private bool HasPermission(string permissionName)
        {
            var permission = _permissionManager.GetPermissionOrNull(permissionName);
            if (permission == null)
            {
                Logger.Warn("Permission is not defined: " + permissionName);
                return false;
            }

            if (!AbpSession.UserId.HasValue)
            {
                Logger.Warn("Not logged in, no permission.");
                return false;
            }

            return _userManager
                .GetRoles(AbpSession.UserId.Value)
                .Any(roleName => _roleManager.HasPermission(roleName, permissionName));
        }
    }
}
