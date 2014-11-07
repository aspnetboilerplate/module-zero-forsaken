using System.Reflection;
using Abp.Modules;

namespace ModuleZeroSampleProject
{
    public class ModuleZeroSampleProjectCoreModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
