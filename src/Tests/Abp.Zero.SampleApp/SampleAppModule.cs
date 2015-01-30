using System.Reflection;
using Abp.Modules;
using Abp.Zero.EntityFramework;
using Abp.Zero.SampleApp.Authorization;
using Abp.Zero.SampleApp.Configuration;

namespace Abp.Zero.SampleApp
{
    [DependsOn(typeof(AbpZeroEntityFrameworkModule))]
    public class SampleAppModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Authorization.Providers.Add<AppAuthorizationProvider>();
            Configuration.Settings.Providers.Add<AppSettingProvider>();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
