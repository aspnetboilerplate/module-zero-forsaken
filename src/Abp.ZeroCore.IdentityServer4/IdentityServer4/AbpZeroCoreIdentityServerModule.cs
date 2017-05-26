using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Zero;

namespace Abp.IdentityServer4
{
    [DependsOn(typeof(AbpZeroCoreModule))]
    public class AbpZeroCoreIdentityServerModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(AbpZeroCoreIdentityServerModule).GetAssembly());
        }
    }
}
