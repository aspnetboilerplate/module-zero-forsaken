using Abp.Dependency;
using Abp.Localization;

namespace Abp.Zero.Configuration
{
    internal class LanguageManagementConfig : ILanguageManagementConfig
    {
        private readonly IIocRegistrar _iocRegistrar;

        public LanguageManagementConfig(IIocRegistrar iocRegistrar)
        {
            _iocRegistrar = iocRegistrar;
        }

        public void Enable()
        {
            _iocRegistrar.Register<ILanguageProvider, ApplicationLanguageProvider>(DependencyLifeStyle.Transient);
        }
    }
}