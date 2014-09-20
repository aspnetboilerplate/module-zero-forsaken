using System.Reflection;
using Abp.Dependency;
using Abp.Modules;
using Abp.Startup;

namespace Abp.Zero
{
    /// <summary>
    /// ABP zero module.
    /// </summary>
    public class AbpZeroModule : AbpModule
    {
        /// <summary>
        /// Current version of the zero module.
        /// </summary>
        public const string CurrentVersion = "0.2.0.0";

        public override void Initialize(IAbpInitializationContext initializationContext)
        {
            base.Initialize(initializationContext);
            IocManager.Instance.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
