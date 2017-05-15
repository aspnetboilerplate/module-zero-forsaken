using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Zero.EntityFrameworkCore;
using Abp.ZeroCore.SampleApp.Core;
using Castle.Windsor.MsDependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Abp.ZeroCore.SampleApp
{
    [DependsOn(typeof(AbpZeroCoreEntityFrameworkCoreModule))]
    public class AbpZeroCoreSampleAppModule : AbpModule
    {
        public override void PreInitialize()
        {
            var services = new ServiceCollection();

            ServicesCollectionDependencyRegistrar.Register(services);

            WindsorRegistrationHelper.CreateServiceProvider(IocManager.IocContainer, services);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(AbpZeroCoreSampleAppModule).GetAssembly());
        }
    }
}
