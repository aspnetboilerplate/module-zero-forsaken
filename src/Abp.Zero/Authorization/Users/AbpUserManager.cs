using Abp.Authorization.Roles;
using Abp.Dependency;
using Abp.MultiTenancy;
using Microsoft.AspNet.Identity;

namespace Abp.Authorization.Users
{
    /// <summary>
    /// Extends <see cref="UserManager{TRole,TKey}"/> of ASP.NET Identity Framework.
    /// </summary>
    public class AbpUserManager<TTenant, TRole, TUser> : UserManager<TUser, long>, ITransientDependency
        where TTenant : AbpTenant<TTenant, TUser>
        where TRole : AbpRole<TTenant, TUser> 
        where TUser : AbpUser<TTenant, TUser>
    {
        public AbpUserManager(AbpUserStore<TTenant, TRole, TUser> store)
            : base(store)
        {

        }
    }
}