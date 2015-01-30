using System.Reflection;
using Abp.AutoMapper;
using Abp.Modules;
using ModuleZeroSampleProject.Authorization;

namespace ModuleZeroSampleProject
{
    [DependsOn(typeof(ModuleZeroSampleProjectCoreModule), typeof(AbpAutoMapperModule))]
    public class ModuleZeroSampleProjectApplicationModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
            Configuration.Authorization.Providers.Add<ModuleZeroSampleProjectAuthorizationProvider>();
        }
    }
}
