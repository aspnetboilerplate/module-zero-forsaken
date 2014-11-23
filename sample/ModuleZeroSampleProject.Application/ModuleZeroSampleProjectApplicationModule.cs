using System.Reflection;
using Abp.Modules;
using ModuleZeroSampleProject.Authorization;

namespace ModuleZeroSampleProject
{
    [DependsOn(typeof(ModuleZeroSampleProjectCoreModule))]
    public class ModuleZeroSampleProjectApplicationModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
            Configuration.Authorization.Providers.Add<ModuleZeroSampleProjectAuthorizationProvider>();
        }
    }
}
