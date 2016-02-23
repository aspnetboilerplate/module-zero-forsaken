using System.Reflection;
using Abp.Dependency;
using Abp.Localization;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Xml;
using Abp.Modules;
using Abp.Zero.Configuration;

namespace Abp.Zero
{
    /// <summary>
    /// ABP zero core module.
    /// </summary>
    [DependsOn(typeof(AbpKernelModule))]
    public class AbpZeroCoreModule : AbpModule
    {
        /// <summary>
        /// Current version of the zero module.
        /// </summary>
        public const string CurrentVersion = "0.8.2.0";

        public override void PreInitialize()
        {
            IocManager.Register<IRoleManagementConfig, RoleManagementConfig>();
            IocManager.Register<IUserManagementConfig, UserManagementConfig>();
            IocManager.Register<ILanguageManagementConfig, LanguageManagementConfig>();
            IocManager.Register<IAbpZeroConfig, AbpZeroConfig>();

            Configuration.Settings.Providers.Add<AbpZeroSettingProvider>();

            Configuration.Localization.Sources.Add(
                new DictionaryBasedLocalizationSource(
                    AbpZeroConsts.LocalizationSourceName,
                    new XmlEmbeddedFileLocalizationDictionaryProvider(
                        Assembly.GetExecutingAssembly(), "Abp.Zero.Localization.Source"
                        )));
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
            IocManager.Register<IMultiTenantLocalizationDictionary, MultiTenantLocalizationDictionary>(DependencyLifeStyle.Transient);
        }
    }
}
