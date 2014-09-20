using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Authorization.Permissions;

namespace Abp.Authorization.Roles
{
    /// <summary>
    /// Used to perform database operations for roles.
    /// </summary>
    public interface IRolePermissionStore
    {
        /// <summary>
        /// Adds a permission grant setting to a role.
        /// </summary>
        /// <param name="role">Role</param>
        /// <param name="permissionGrant">Permission grant setting info</param>
        Task AddPermissionAsync(AbpRole role, PermissionGrantInfo permissionGrant);

        /// <summary>
        /// Removes a permission grant setting from a role.
        /// </summary>
        /// <param name="role">Role </param>
        /// <param name="permissionGrant">Permission grant setting info</param>
        Task RemovePermissionAsync(AbpRole role, PermissionGrantInfo permissionGrant);

        /// <summary>
        /// Gets permission grant setting informations for a role.
        /// </summary>
        /// <param name="role">Role</param>
        /// <returns>List of permission setting informations</returns>
        Task<IList<PermissionGrantInfo>> GetPermissionsAsync(AbpRole role);

        /// <summary>
        /// Checks whether a role has a permission grant setting info.
        /// </summary>
        /// <param name="role">Role</param>
        /// <param name="permissionGrant">Permission grant setting info</param>
        /// <returns></returns>
        Task<bool> HasPermissionAsync(AbpRole role, PermissionGrantInfo permissionGrant);
    }
}