using Abp.Authorization.Permissions;
using Abp.Dependency;
using Microsoft.AspNet.Identity;

namespace Abp.Authorization.Roles
{
    /// <summary>
    /// Extends <see cref="RoleManager{TRole,TKey}"/> of ASP.NET Identity Framework.
    /// </summary>
    /// <remarks>
    /// Do not directly use <see cref="IAbpRoleRepository"/> to perform role operations.
    /// Instead, use this class.
    /// </remarks>
    public class AbpRoleManager : RoleManager<AbpRole, int>, ITransientDependency
    {
        private readonly IPermissionManager _permissionManager;

        public AbpRoleManager(AbpRoleStore store, IPermissionManager permissionManager)
            : base(store)
        {
            _permissionManager = permissionManager;
        }

        public bool HasPermission(string roleName, string permissionName) //TODO: Async
        {
            var role = this.FindByName(roleName);
            return HasPermission(role.Id, permissionName);
        }

        public bool HasPermission(int roleId, string permissionName) //TODO: Async
        {
            if (!(Store is IRolePermissionStore))
            {
                throw new AbpException("Store is not IRolePermissionStore");
            }

            var permission = _permissionManager.GetPermissionOrNull(permissionName);
            if (permission == null)
            {
                throw new AbpException("There is no permission with name: " + permissionName);
            }

            var role = this.FindById(roleId);

            var permissionStore = Store as IRolePermissionStore;

            return permission.IsGrantedByDefault
                ? !permissionStore.HasPermissionAsync(role, new PermissionGrantInfo(permissionName, false)).Result
                : permissionStore.HasPermissionAsync(role, new PermissionGrantInfo(permissionName, true)).Result;
        }
    }
}