using System.Data.Entity;
using System.Reflection;
using Abp.EntityFramework;
using Abp.Modules;
using ModuleZeroSampleProject.EntityFramework;

namespace ModuleZeroSampleProject
{
    [DependsOn(typeof(AbpEntityFrameworkModule), typeof(ModuleZeroSampleProjectCoreModule))]
    public class ModuleZeroSampleProjectDataModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.DefaultNameOrConnectionString = "Default";
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
            Database.SetInitializer<ModuleZeroSampleProjectDbContext>(null);
        }
    }
}
