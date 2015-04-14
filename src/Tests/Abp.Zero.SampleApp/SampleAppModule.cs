using System.Reflection;
using Abp.Localization;
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
            Configuration.Localization.Languages.Add(new LanguageInfo("en", "English", isDefault: true));
            Configuration.Localization.Languages.Add(new LanguageInfo("tr", "Türkçe"));

            Configuration.Authorization.Providers.Add<AppAuthorizationProvider>();
            Configuration.Settings.Providers.Add<AppSettingProvider>();
            Configuration.MultiTenancy.IsEnabled = true;
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
