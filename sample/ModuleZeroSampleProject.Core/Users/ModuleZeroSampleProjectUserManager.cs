using Abp.Authorization.Users;
using ModuleZeroSampleProject.Authorization;
using ModuleZeroSampleProject.MultiTenancy;

namespace ModuleZeroSampleProject.Users
{
    public class ModuleZeroSampleProjectUserManager : AbpUserManager<Tenant, Role, User>
    {
        public ModuleZeroSampleProjectUserManager(ModuleZeroSampleProjectUserStore store)
            : base(store)
        {
        }
    }
}