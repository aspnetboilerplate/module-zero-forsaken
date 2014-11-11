using System.Reflection;
using Abp.Modules;
using Abp.Zero.Configuration;

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
        public const string CurrentVersion = "0.4.2.0";

        public override void PreInitialize()
        {
            IocManager.Register<MultiTenancyConfig>();
            IocManager.Register<ZeroConfig>();
        }

        public override void Initialize()
        {
            base.Initialize();
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
