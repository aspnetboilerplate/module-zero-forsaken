using System.Reflection;
using Abp.Modules;

namespace Abp.Zero
{
    /// <summary>
    /// ABP zero module.
    /// </summary>
    public class AbpZeroCoreModule : AbpModule
    {
        /// <summary>
        /// Current version of the zero module.
        /// </summary>
        public const string CurrentVersion = "0.3.0.0";

        public override void Initialize()
        {
            base.Initialize();
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
