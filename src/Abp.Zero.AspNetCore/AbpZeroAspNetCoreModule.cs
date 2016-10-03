using System.Reflection;
using Abp.Modules;

namespace Abp.Zero.AspNetCore
{
    [DependsOn(typeof(AbpZeroCoreModule))]
    public class AbpZeroAspNetCoreModule : AbpModule
    {
        public override void PreInitialize()
        {
            IocManager.Register<IAbpZeroAspNetCoreConfiguration, AbpZeroAspNetCoreConfiguration>();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}