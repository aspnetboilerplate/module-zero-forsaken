using System.Reflection;
using Abp.Dependency;
using Abp.Modules;
using Abp.Startup;

namespace Abp.Zero
{
    public class AbpZeroModule : AbpModule
    {
        public const string CurrentVersion = "0.1.0.1";

        public override void Initialize(IAbpInitializationContext initializationContext)
        {
            base.Initialize(initializationContext);
            IocManager.Instance.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
