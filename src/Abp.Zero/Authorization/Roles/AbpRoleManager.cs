using Abp.Authorization.Users;
using Abp.Dependency;
using Abp.MultiTenancy;
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
    public class AbpRoleManager<TRole, TTenant, TUser> : RoleManager<TRole, int>, ITransientDependency
        where TRole : AbpRole<TTenant, TUser>
        where TUser : AbpUser<TTenant, TUser>
        where TTenant : AbpTenant<TTenant, TUser>
    {
        private readonly IPermissionManager _permissionManager;

        public AbpRoleManager(AbpRoleStore<TRole, TTenant, TUser> store, IPermissionManager permissionManager)
            : base(store)
        {
            _permissionManager = permissionManager;
        }

        /// <summary>
        /// Checks if a role has a permission.
        /// </summary>
        /// <param name="roleName">The role's name to check it's permission</param>
        /// <param name="permissionName">Name of the permission</param>
        /// <returns>True, if the role has the permission</returns>
        public bool HasPermission(string roleName, string permissionName) //TODO: Async
        {
            var role = this.FindByName(roleName);
            if (role == null)
            {
                throw new AbpAuthorizationException("There is no role named " + roleName);
            }

            return HasPermissionInternal(role, permissionName);
        }

        /// <summary>
        /// Checks if a role has a permission.
        /// </summary>
        /// <param name="roleId">The role's id to check it's permission</param>
        /// <param name="permissionName">Name of the permission</param>
        /// <returns>True, if the role has the permission</returns>
        public bool HasPermission(int roleId, string permissionName) //TODO: Async
        {
            var role = this.FindById(roleId);

            if (role == null)
            {
                throw new AbpAuthorizationException("There is no role by id = " + roleId);
            }

            return HasPermissionInternal(role, permissionName);
        }

        private bool HasPermissionInternal(TRole role, string permissionName) //TODO: Async
        {
            if (!(Store is IRolePermissionStore<TRole, TTenant, TUser>))
            {
                throw new AbpException("Store is not IRolePermissionStore");
            }

            var permission = _permissionManager.GetPermissionOrNull(permissionName);
            if (permission == null)
            {
                throw new AbpException("There is no permission with name: " + permissionName);
            }

            var permissionStore = Store as IRolePermissionStore<TRole, TTenant, TUser>;

            return permission.IsGrantedByDefault
                ? !permissionStore.HasPermissionAsync(role, new PermissionGrantInfo(permissionName, false)).Result
                : permissionStore.HasPermissionAsync(role, new PermissionGrantInfo(permissionName, true)).Result;
        }
    }
}