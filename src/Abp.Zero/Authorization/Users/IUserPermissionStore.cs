﻿using Abp.MultiTenancy;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Abp.Authorization.Users
{
    /// <summary>
    /// Used to perform permission database operations for a user.
    /// </summary>
    public interface IUserPermissionStore<TTenant, TUser>
        where TUser : AbpUser<TTenant, TUser>
        where TTenant : AbpTenant<TTenant, TUser>
    {
        /// <summary>
        /// Adds a permission grant setting to a user.
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="permissionGrant">Permission grant setting info</param>
        Task AddPermissionAsync(TUser user, PermissionGrantInfo permissionGrant);

        /// <summary>
        /// Removes a permission grant setting from a user.
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="permissionGrant">Permission grant setting info</param>
        Task RemovePermissionAsync(TUser user, PermissionGrantInfo permissionGrant);

        /// <summary>
        /// Gets permission grant setting informations for a user.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <returns>List of permission setting informations</returns>
        Task<IList<PermissionGrantInfo>> GetPermissionsAsync(Guid userId);

        /// <summary>
        /// Checks whether a role has a permission grant setting info.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="permissionGrant">Permission grant setting info</param>
        /// <returns></returns>
        Task<bool> HasPermissionAsync(Guid userId, PermissionGrantInfo permissionGrant);

        /// <summary>
        /// Deleted all permission settings for a role.
        /// </summary>
        /// <param name="user">User</param>
        Task RemoveAllPermissionSettingsAsync(TUser user);
    }
}