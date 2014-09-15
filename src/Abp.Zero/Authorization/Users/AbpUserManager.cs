using Abp.Dependency;
using Microsoft.AspNet.Identity;

namespace Abp.Authorization.Users
{
    public class AbpUserManager : UserManager<AbpUser, long>, ITransientDependency
    {
        public AbpUserManager(AbpUserStore store)
            : base(store)
        {

        }
    }
}