using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Authorization.Users;
using Abp.Dependency;
using Abp.MultiTenancy;
using Castle.Core.Internal;
using Microsoft.AspNet.Identity;

namespace Abp.Authorization.Roles
{
    /// <summary>
    /// Extends <see cref="RoleManager{TRole,TKey}"/> of ASP.NET Identity Framework.
    /// Applications should derive this class with appropriate generic arguments.
    /// </summary>
    public abstract class AbpRoleManager<TTenant, TRole, TUser> : RoleManager<TRole, int>, ITransientDependency
        where TTenant : AbpTenant<TTenant, TUser>
        where TRole : AbpRole<TTenant, TUser>
        where TUser : AbpUser<TTenant, TUser>
    {
        private readonly IPermissionManager _permissionManager;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="store">Role store</param>
        /// <param name="permissionManager">Permission manager</param>
        protected AbpRoleManager(AbpRoleStore<TTenant, TRole, TUser> store, IPermissionManager permissionManager)
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
        public virtual async Task<bool> HasPermissionAsync(string roleName, string permissionName)
        {
            return await HasPermissionAsync(await GetRole(roleName), GetPermission(permissionName));
        }

        /// <summary>
        /// Checks if a role has a permission.
        /// </summary>
        /// <param name="roleId">The role's id to check it's permission</param>
        /// <param name="permissionName">Name of the permission</param>
        /// <returns>True, if the role has the permission</returns>
        public virtual async Task<bool> HasPermissionAsync(int roleId, string permissionName)
        {
            return await HasPermissionAsync(await GetRole(roleId), GetPermission(permissionName));
        }

        /// <summary>
        /// Checks if a role has a permission.
        /// </summary>
        /// <param name="role">The rolepermission</param>
        /// <param name="permission">The permission</param>
        /// <returns>True, if the role has the permission</returns>
        public async Task<bool> HasPermissionAsync(TRole role, Permission permission)
        {
            var permissionStore = GetRolePermissionStore();

            return permission.IsGrantedByDefault
                ? !(await permissionStore.HasPermissionAsync(role, new PermissionGrantInfo(permission.Name, false)))
                : (await permissionStore.HasPermissionAsync(role, new PermissionGrantInfo(permission.Name, true)));
        }

        /// <summary>
        /// Gets granted permission names for a role.
        /// </summary>
        /// <param name="roleId">Role id</param>
        /// <returns>List of granted permissions</returns>
        public virtual async Task<IReadOnlyList<Permission>> GetGrantedPermissionsAsync(int roleId)
        {
            return await GetGrantedPermissionsAsync(await GetRole(roleId));
        }

        /// <summary>
        /// Gets granted permission names for a role.
        /// </summary>
        /// <param name="roleName">Role name</param>
        /// <returns>List of granted permissions</returns>
        public virtual async Task<IReadOnlyList<Permission>> GetGrantedPermissionsAsync(string roleName)
        {
            return await GetGrantedPermissionsAsync(await GetRole(roleName));
        }

        /// <summary>
        /// Gets granted permission names for a role.
        /// </summary>
        /// <param name="role">Role</param>
        /// <returns>List of granted permissions</returns>
        public virtual async Task<IReadOnlyList<Permission>> GetGrantedPermissionsAsync(TRole role)
        {
            var permissionList = new List<Permission>();

            foreach (var permission in _permissionManager.GetAllPermissions())
            {
                if (await HasPermissionAsync(role, permission))
                {
                    permissionList.Add(permission);
                }
            }

            return permissionList;
        }

        /// <summary>
        /// Sets all granted permissions of a role at once.
        /// Clears all other permissions before settings.
        /// </summary>
        /// <param name="roleId">Role id</param>
        /// <param name="permissions">Permissions</param>
        public virtual async Task SetGrantedPermissionsAsync(int roleId, IEnumerable<Permission> permissions)
        {
            await SetGrantedPermissionsAsync(await GetRole(roleId), permissions);
        }

        /// <summary>
        /// Sets all granted permissions of a role at once.
        /// Clears all other permissions before settings.
        /// </summary>
        /// <param name="role">The role</param>
        /// <param name="permissions">Permissions</param>
        public virtual async Task SetGrantedPermissionsAsync(TRole role, IEnumerable<Permission> permissions)
        {
            await ClearAllPermissionsAsync(role);
            foreach (var permission in permissions)
            {
                await GrantPermissionAsync(role, permission);
            }
        }

        private async Task GrantPermissionAsync(TRole role, Permission permission)
        {
            if (await HasPermissionAsync(role, permission))
            {
                return;
            }

            //TODO: Default'da verilip verilmediðine göre birþeyler yapýlacak.

            await GetRolePermissionStore().AddPermissionAsync(role, new PermissionGrantInfo(permission.Name, true));
        }

        private async Task ClearAllPermissionsAsync(TRole role)
        {
            await GetRolePermissionStore().RemoveAllPermissionSettingsAsync(role);
        }

        public override Task<IdentityResult> DeleteAsync(TRole role)
        {
            if (role.IsStatic)
            {
                throw new AbpException("Can not delete a static role: " + role);
            }

            return base.DeleteAsync(role);
        }

        private Permission GetPermission(string permissionName)
        {
            var permission = _permissionManager.GetPermissionOrNull(permissionName);
            if (permission == null)
            {
                throw new AbpException("There is no permission with name: " + permissionName);
            }
            return permission;
        }

        private IRolePermissionStore<TTenant, TRole, TUser> GetRolePermissionStore()
        {
            if (!(Store is IRolePermissionStore<TTenant, TRole, TUser>))
            {
                throw new AbpException("Store is not IRolePermissionStore");
            }

            return Store as IRolePermissionStore<TTenant, TRole, TUser>;
        }

        private async Task<TRole> GetRole(int roleId)
        {
            var role = await FindByIdAsync(roleId);
            if (role == null)
            {
                throw new AbpException("There is no role with id = " + roleId);
            }

            return role;
        }

        private async Task<TRole> GetRole(string roleName)
        {
            var role = await FindByNameAsync(roleName);
            if (role == null)
            {
                throw new AbpException("There is no role with name = " + roleName);
            }

            return role;
        }
    }
}