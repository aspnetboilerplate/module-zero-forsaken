using Abp.Configuration.Startup;

namespace Abp.Zero.Configuration
{
    public static class MyModuleConfigurationExtensions
    {
        public static ZeroConfig Zero(this IModuleConfigurations moduleConfigurations)
        {
            return moduleConfigurations.AbpConfiguration
                .GetOrCreate("ZeroConfig",
                    () => moduleConfigurations.AbpConfiguration.IocManager.Resolve<ZeroConfig>()
                );
        }
    }
}