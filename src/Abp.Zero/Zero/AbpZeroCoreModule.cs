using System.Reflection;
using Abp.Modules;
using Abp.Zero.Configuration;

namespace Abp.Zero
{
    /// <summary>
    /// ABP zero core module.
    /// </summary>
    public class AbpZeroCoreModule : AbpModule
    {
        /// <summary>
        /// Current version of the zero module.
        /// </summary>
        public const string CurrentVersion = "0.5.12.3";

        public override void PreInitialize()
        {
            IocManager.Register<IUserManagementConfig, UserManagementConfig>();
            IocManager.Register<IRoleManagementConfig, RoleManagementConfig>();
            IocManager.Register<IAbpZeroConfig, AbpZeroConfig>();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
