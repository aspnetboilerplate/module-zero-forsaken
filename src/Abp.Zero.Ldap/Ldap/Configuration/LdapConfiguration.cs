using System.DirectoryServices.AccountManagement;
using Abp.Configuration;
using Abp.Dependency;
using Abp.Extensions;

namespace Abp.Zero.Ldap.Configuration
{
    /// <summary>
    /// Implements <see cref="ILdapConfiguration"/> to get settings from <see cref="ISettingManager"/>.
    /// </summary>
    public class LdapConfiguration : ILdapConfiguration, ITransientDependency
    {
        private readonly ISettingManager _settingManager;

        public LdapConfiguration(ISettingManager settingManager)
        {
            _settingManager = settingManager;
        }

        public bool IsEnabled
        {
            get { return _settingManager.GetSettingValue<bool>(LdapSettingNames.IsEnabled); }
        }

        public ContextType ContextType
        {
            get { return _settingManager.GetSettingValue(LdapSettingNames.ContextType).ToEnum<ContextType>(); }
        }

        public string Container
        {
            get { return _settingManager.GetSettingValue(LdapSettingNames.Container); }
        }

        public string Domain
        {
            get { return _settingManager.GetSettingValue(LdapSettingNames.Domain); }
        }

        public string UserName
        {
            get { return _settingManager.GetSettingValue(LdapSettingNames.UserName); }
        }

        public string Password
        {
            get { return _settingManager.GetSettingValue(LdapSettingNames.Password); }
        }
    }
}