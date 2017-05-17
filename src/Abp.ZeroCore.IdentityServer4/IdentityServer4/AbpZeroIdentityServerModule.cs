using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Zero;

namespace Abp.IdentityServer4
{
    [DependsOn(typeof(AbpZeroCoreModule))]
    public class AbpZeroIdentityServerModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(AbpZeroIdentityServerModule).GetAssembly());
        }
    }
}
