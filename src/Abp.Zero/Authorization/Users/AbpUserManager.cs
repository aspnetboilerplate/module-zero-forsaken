using Abp.Dependency;
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
    public class AbpUserManager : UserManager<AbpUser, long>, ITransientDependency
    {
        public AbpUserManager(AbpUserStore store)
            : base(store)
        {

        }
    }
}