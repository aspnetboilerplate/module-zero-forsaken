using System.Reflection;
using Abp.Modules;
using Abp.Zero.EntityFramework;
using Abp.Zero.SampleApp.Authorization;

namespace Abp.Zero.SampleApp
{
    [DependsOn(typeof(AbpZeroEntityFrameworkModule))]
    public class SampleAppModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Authorization.Providers.Add<AppAuthorizationProvider>();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
