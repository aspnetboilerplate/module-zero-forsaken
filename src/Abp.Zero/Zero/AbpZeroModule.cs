using System.Reflection;
using Abp.Dependency;
using Abp.Modules;
using Abp.Startup;

namespace Abp.Zero
{
    public class AbpZeroModule : AbpModule
    {
        public const string CurrentVersion = "0.2.0.0";

        public override void Initialize(IAbpInitializationContext initializationContext)
        {
            base.Initialize(initializationContext);
            IocManager.Instance.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
