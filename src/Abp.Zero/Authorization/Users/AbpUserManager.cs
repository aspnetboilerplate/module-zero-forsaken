using Abp.Authorization.Roles;
using Abp.Dependency;
using Abp.MultiTenancy;
using Microsoft.AspNet.Identity;

namespace Abp.Authorization.Users
{
    /// <summary>
    /// Extends <see cref="UserManager{TRole,TKey}"/> of ASP.NET Identity Framework.
    /// </summary>
    /// <remarks>
    /// Do not directly use <see cref="IAbpUserRepository"/> to perform user operations.
    /// Instead, use this class.
    /// </remarks>
    public class AbpUserManager<TRole, TTenant, TUser> : UserManager<TUser, long>, ITransientDependency
        where TRole : AbpRole<TTenant, TUser> 
        where TTenant : AbpTenant<TTenant, TUser> 
        where TUser : AbpUser<TTenant, TUser>
    {
        public AbpUserManager(AbpUserStore<TRole, TTenant, TUser> store)
            : base(store)
        {

        }
    }
}