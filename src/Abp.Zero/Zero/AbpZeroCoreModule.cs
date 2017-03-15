using System.Reflection;
using Abp.Modules;

namespace Abp.Zero
{
    [DependsOn(typeof(AbpZeroCommonModule))]
    public class AbpZeroCoreModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
