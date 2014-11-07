using System.Reflection;
using Abp.Modules;

namespace ModuleZeroSampleProject
{
    [DependsOn(typeof(ModuleZeroSampleProjectCoreModule))]
    public class ModuleZeroSampleProjectApplicationModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
