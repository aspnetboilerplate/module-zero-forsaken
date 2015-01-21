using System.Linq;
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

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="userStore"></param>
        /// <param name="roleManager"></param>
        protected AbpUserManager(AbpUserStore<TTenant, TRole, TUser> userStore, AbpRoleManager<TTenant, TRole, TUser> roleManager)
            : base(userStore)
        {
            _roleManager = roleManager;
        }

        public bool IsGranted(long userId, string permissionName)
        {
            //TODO: Should use UserManager for that
            return this.GetRoles(userId)
                .Any(roleName => _roleManager.HasPermissionAsync(roleName, permissionName).Result);
        }
    }
}