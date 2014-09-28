using System.Reflection;
using Abp.Modules;

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
        public const string CurrentVersion = "0.2.1.1";

        public override void Initialize()
        {
            base.Initialize();
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
