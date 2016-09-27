using System;
using Abp.Authorization.Roles;
using Abp.MultiTenancy;
using Abp.Threading;

namespace Abp.Authorization.Users
{
    /// <summary>
    /// Extension methods for <see cref="AbpUserManager{TTenant,TRole,TUser}"/>.
    /// </summary>
    public static class AbpUserManagerExtensions
    {
        /// <summary>
        /// Check whether a user is granted for a permission.
        /// </summary>
        /// <param name="manager">User manager</param>
        /// <param name="userId">User id</param>
        /// <param name="permissionName">Permission name</param>
        public static bool IsGranted<TTenant, TRole, TUser>(AbpUserManager<TTenant, TRole, TUser> manager, long userId, string permissionName)
            where TTenant : AbpTenant<TUser>
            where TRole : AbpRole<TUser>, new()
            where TUser : AbpUser<TUser>
        {
            if (manager == null)
            {
                throw new ArgumentNullException(nameof(manager));
            }

            return AsyncHelper.RunSync(() => manager.IsGrantedAsync(userId, permissionName));
        }

        public static AbpUserManager<TTenant, TRole, TUser>.AbpLoginResult Login<TTenant, TRole, TUser>(AbpUserManager<TTenant, TRole, TUser> manager, string userNameOrEmailAddress, string plainPassword, string tenancyName = null)
            where TTenant : AbpTenant<TUser>
            where TRole : AbpRole<TUser>, new()
            where TUser : AbpUser<TUser>
        {
            if (manager == null)
            {
                throw new ArgumentNullException(nameof(manager));
            }

            return AsyncHelper.RunSync(() => manager.LoginAsync(userNameOrEmailAddress, plainPassword, tenancyName));
        }
    }
}