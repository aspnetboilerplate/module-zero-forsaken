using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using Abp.Configuration;
using Abp.Localization;

namespace Abp.Zero.Ldap.Configuration
{
    public class LdapSettingProvider : SettingProvider
    {
        public override IEnumerable<SettingDefinition> GetSettingDefinitions(SettingDefinitionProviderContext context)
        {
            return new[]
                   {
                       new SettingDefinition(LdapSettingNames.IsEnabled, "true", L("Ldap_IsEnabled")),
                       new SettingDefinition(LdapSettingNames.ContextType, ContextType.Domain.ToString(), L("Ldap_ContextType")),
                       new SettingDefinition(LdapSettingNames.Container, null, L("Ldap_Container")),
                       new SettingDefinition(LdapSettingNames.Domain, null, L("Ldap_Domain")),
                       new SettingDefinition(LdapSettingNames.UserName, null, L("Ldap_UserName")),
                       new SettingDefinition(LdapSettingNames.Password, null, L("Ldap_Password"))
                   };
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, AbpZeroConsts.LocalizationSourceName);
        }
    }
}
