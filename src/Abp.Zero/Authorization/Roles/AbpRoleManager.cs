using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Authorization.Users;
using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.MultiTenancy;
using Abp.Runtime.Session;
using Abp.Zero.Configuration;
using Microsoft.AspNet.Identity;

namespace Abp.Authorization.Roles
{
    /// <summary>
    /// Extends <see cref="RoleManager{TRole,TKey}"/> of ASP.NET Identity Framework.
    /// Applications should derive this class with appropriate generic arguments.
    /// </summary>
    public abstract class AbpRoleManager<TTenant, TRole, TUser> : RoleManager<TRole, int>, ITransientDependency
        where TTenant : AbpTenant<TTenant, TUser>
        where TRole : AbpRole<TTenant, TUser>, new()
        where TUser : AbpUser<TTenant, TUser>
    {

        public IAbpSession AbpSession { get; set; }
        
        public IRoleManagementConfig RoleManagementConfig { get; private set; }

        private IRolePermissionStore<TTenant, TRole, TUser> RolePermissionStore
        {
            get
            {
                if (!(Store is IRolePermissionStore<TTenant, TRole, TUser>))
                {
                    throw new AbpException("Store is not IRolePermissionStore");
                }

                return Store as IRolePermissionStore<TTenant, TRole, TUser>;
            }
        }

        private readonly IPermissionManager _permissionManager;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="store">Role store</param>
        /// <param name="permissionManager">Permission manager</param>
        /// <param name="unitOfWorkManager"></param>
        protected AbpRoleManager(
            AbpRoleStore<TTenant, TRole, TUser> store, 
            IPermissionManager permissionManager, 
            IRoleManagementConfig roleManagementConfig,
            IUnitOfWorkManager unitOfWorkManager
            )
            : base(store)
        {
            RoleManagementConfig = roleManagementConfig;
            _permissionManager = permissionManager;
            _unitOfWorkManager = unitOfWorkManager;
            AbpSession = NullAbpSession.Instance;
        }

        /// <summary>
        /// Checks if a role has a permission.
        /// </summary>
        /// <param name="roleName">The role's name to check it's permission</param>
        /// <param name="permissionName">Name of the permission</param>
        /// <returns>True, if the role has the permission</returns>
        public virtual async Task<bool> HasPermissionAsync(string roleName, string permissionName)
        {
            return await HasPermissionAsync(await GetRoleByNameAsync(roleName), _permissionManager.GetPermission(permissionName));
        }

        /// <summary>
        /// Checks if a role has a permission.
        /// </summary>
        /// <param name="roleId">The role's id to check it's permission</param>
        /// <param name="permissionName">Name of the permission</param>
        /// <returns>True, if the role has the permission</returns>
        public virtual async Task<bool> HasPermissionAsync(int roleId, string permissionName)
        {
            return await HasPermissionAsync(await GetRoleByIdAsync(roleId), _permissionManager.GetPermission(permissionName));
        }

        /// <summary>
        /// Checks if a role has a permission.
        /// </summary>
        /// <param name="role">The rolepermission</param>
        /// <param name="permission">The permission</param>
        /// <returns>True, if the role has the permission</returns>
        public async Task<bool> HasPermissionAsync(TRole role, Permission permission)
        {
            return permission.IsGrantedByDefault
                ? !(await RolePermissionStore.HasPermissionAsync(role, new PermissionGrantInfo(permission.Name, false)))
                : (await RolePermissionStore.HasPermissionAsync(role, new PermissionGrantInfo(permission.Name, true)));
        }

        /// <summary>
        /// Gets granted permission names for a role.
        /// </summary>
        /// <param name="roleId">Role id</param>
        /// <returns>List of granted permissions</returns>
        public virtual async Task<IReadOnlyList<Permission>> GetGrantedPermissionsAsync(int roleId)
        {
            return await GetGrantedPermissionsAsync(await GetRoleByIdAsync(roleId));
        }

        /// <summary>
        /// Gets granted permission names for a role.
        /// </summary>
        /// <param name="roleName">Role name</param>
        /// <returns>List of granted permissions</returns>
        public virtual async Task<IReadOnlyList<Permission>> GetGrantedPermissionsAsync(string roleName)
        {
            return await GetGrantedPermissionsAsync(await GetRoleByNameAsync(roleName));
        }

        /// <summary>
        /// Gets granted permissions for a role.
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
        /// Prohibits all other permissions.
        /// </summary>
        /// <param name="roleId">Role id</param>
        /// <param name="permissions">Permissions</param>
        public virtual async Task SetGrantedPermissionsAsync(int roleId, IEnumerable<Permission> permissions)
        {
            await SetGrantedPermissionsAsync(await GetRoleByIdAsync(roleId), permissions);
        }

        /// <summary>
        /// Sets all granted permissions of a role at once.
        /// Prohibits all other permissions.
        /// </summary>
        /// <param name="role">The role</param>
        /// <param name="permissions">Permissions</param>
        public virtual async Task SetGrantedPermissionsAsync(TRole role, IEnumerable<Permission> permissions)
        {
            var oldPermissions = await GetGrantedPermissionsAsync(role);
            var newPermissions = permissions.ToArray();

            foreach (var permission in oldPermissions.Where(p => !newPermissions.Contains(p)))
            {
                await ProhibitPermissionAsync(role, permission);
            }

            foreach (var permission in newPermissions.Where(p => !oldPermissions.Contains(p)))
            {
                await GrantPermissionAsync(role, permission);
            }
        }

        /// <summary>
        /// Grants a permission for a role.
        /// </summary>
        /// <param name="role">Role</param>
        /// <param name="permission">Permission</param>
        public async Task GrantPermissionAsync(TRole role, Permission permission)
        {
            if (await HasPermissionAsync(role, permission))
            {
                return;
            }

            if (permission.IsGrantedByDefault)
            {
                await RolePermissionStore.RemovePermissionAsync(role, new PermissionGrantInfo(permission.Name, false));
            }
            else
            {
                await RolePermissionStore.AddPermissionAsync(role, new PermissionGrantInfo(permission.Name, true));
            }
        }

        /// <summary>
        /// Prohibits a permission for a role.
        /// </summary>
        /// <param name="role">Role</param>
        /// <param name="permission">Permission</param>
        public async Task ProhibitPermissionAsync(TRole role, Permission permission)
        {
            if (!await HasPermissionAsync(role, permission))
            {
                return;
            }

            if (permission.IsGrantedByDefault)
            {
                await RolePermissionStore.AddPermissionAsync(role, new PermissionGrantInfo(permission.Name, false));
            }
            else
            {
                await RolePermissionStore.RemovePermissionAsync(role, new PermissionGrantInfo(permission.Name, true));
            }
        }

        /// <summary>
        /// Prohibits all permissions for a role.
        /// </summary>
        /// <param name="role">Role</param>
        public async Task ProhibitAllPermissionsAsync(TRole role)
        {
            foreach (var permission in _permissionManager.GetAllPermissions())
            {
                await ProhibitPermissionAsync(role, permission);
            }
        }

        /// <summary>
        /// Resets all permission settings for a role.
        /// It removes all permission settings for the role.
        /// Role will have permissions those have <see cref="Permission.IsGrantedByDefault"/> set to true.
        /// </summary>
        /// <param name="role">Role</param>
        public async Task ResetAllPermissionsAsync(TRole role)
        {
            await RolePermissionStore.RemoveAllPermissionSettingsAsync(role);
        }

        /// <summary>
        /// Creates a role.
        /// </summary>
        /// <param name="role">Role</param>
        public override async Task<IdentityResult> CreateAsync(TRole role)
        {
            if (AbpSession.MultiTenancySide == MultiTenancySides.Host && role.TenantId.HasValue)
            {
                return await CreateForTenantAsync(role.TenantId.Value, role);
            }
            
            if (AbpSession.TenantId.HasValue)
            {
                role.TenantId = AbpSession.TenantId.Value;
            }
            
            return await base.CreateAsync(role);
        }

        public async Task<IdentityResult> CreateForTenantAsync(int tenantId, TRole role)
        {
            using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                if (Roles.Any(r => r.Name == role.Name && r.TenantId == tenantId))
                {
                    return IdentityResult.Failed("There is already a role with name: " + role.Name);                    
                }

                role.TenantId = tenantId;

                //TODO: Role name constraints and other business

                await Store.CreateAsync(role);
                return IdentityResult.Success;
            }
        }

        /// <summary>
        /// Deletes a role.
        /// </summary>
        /// <param name="role">Role</param>
        public override Task<IdentityResult> DeleteAsync(TRole role)
        {
            if (role.IsStatic)
            {
                throw new AbpException("Can not delete a static role: " + role);
            }

            return base.DeleteAsync(role);
        }

        /// <summary>
        /// Gets a role by given id.
        /// Throws exception if no role with given id.
        /// </summary>
        /// <param name="roleId">Role id</param>
        /// <returns>Role</returns>
        /// <exception cref="AbpException">Throws exception if no role with given id</exception>
        public virtual async Task<TRole> GetRoleByIdAsync(int roleId)
        {
            var role = await FindByIdAsync(roleId);
            if (role == null)
            {
                throw new AbpException("There is no role with id: " + roleId);
            }

            return role;
        }

        /// <summary>
        /// Gets a role by given name.
        /// Throws exception if no role with given roleName.
        /// </summary>
        /// <param name="roleName">Role name</param>
        /// <returns>Role</returns>
        /// <exception cref="AbpException">Throws exception if no role with given roleName</exception>
        public virtual async Task<TRole> GetRoleByNameAsync(string roleName)
        {
            var role = await FindByNameAsync(roleName);
            if (role == null)
            {
                throw new AbpException("There is no role with name: " + roleName);
            }

            return role;
        }

        public virtual async Task<IdentityResult> CreateStaticRolesForTenant(int tenantId)
        {
            var staticRoleDefinitions = RoleManagementConfig.StaticRoles.Where(sr => sr.Side == MultiTenancySides.Tenant);

            foreach (var staticRoleDefinition in staticRoleDefinitions)
            {
                var role = new TRole
                {
                    TenantId = tenantId,
                    Name = staticRoleDefinition.RoleName,
                    DisplayName = staticRoleDefinition.RoleName,
                    IsStatic = true
                };

                var identityResult = await CreateAsync(role);
                if (!identityResult.Succeeded)
                {
                    return identityResult;
                }
            }

            return IdentityResult.Success;
        }
    }
}