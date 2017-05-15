using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.ZeroCore.SampleApp;
using Castle.Windsor.MsDependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Abp.Zero
{
    [DependsOn(typeof(AbpZeroCoreSampleAppModule))]
    public class AbpZeroTestModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;

            var services = new ServiceCollection();

            var serviceProvider = WindsorRegistrationHelper.CreateServiceProvider(
                IocManager.IocContainer,
                services
            );
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(AbpZeroTestModule).GetAssembly());
        }
    }
}
