using System.Linq;
using System.Threading.Tasks;
using Abp.Authorization.Roles;
using Abp.Dependency;
using Abp.MultiTenancy;
using Microsoft.AspNet.Identity;

namespace Abp.Authorization.Users
{
    /// <summary>
    /// Extends <see cref="UserManager{TRole,TKey}"/> of ASP.NET Identity Framework.
    /// </summary>
    public abstract class AbpUserManager<TTenant, TRole, TUser> : UserManager<TUser, long>, ITransientDependency
        where TTenant : AbpTenant<TTenant, TUser>
        where TRole : AbpRole<TTenant, TUser>
        where TUser : AbpUser<TTenant, TUser>
    {
        private readonly AbpRoleManager<TTenant, TRole, TUser> _roleManager;

        protected AbpUserManager(AbpUserStore<TTenant, TRole, TUser> userStore, AbpRoleManager<TTenant, TRole, TUser> roleManager)
            : base(userStore)
        {
            _roleManager = roleManager;
        }

        /// <summary>
        /// Check whether a user is granted for a permission.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="permissionName">Permission name</param>
        public async Task<bool> IsGrantedAsync(long userId, string permissionName)
        {
            foreach (var role in await GetRolesAsync(userId))
            {
                if (await _roleManager.HasPermissionAsync(role, permissionName))
                {
                    return true;
                }
            }

            return false;
        }
    }
}