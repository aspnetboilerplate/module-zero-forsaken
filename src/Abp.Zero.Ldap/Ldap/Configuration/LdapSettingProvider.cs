using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using Abp.Configuration;
using Abp.Localization;

namespace Abp.Zero.Ldap.Configuration
{
    /// <summary>
    /// Defines LDAP settings.
    /// </summary>
    public class LdapSettingProvider : SettingProvider
    {
        public override IEnumerable<SettingDefinition> GetSettingDefinitions(SettingDefinitionProviderContext context)
        {
            //TODO: Settings should be scooped for Application & Tenant levels (without inheritance)
            return new[]
                   {
                       new SettingDefinition(LdapSettingNames.IsEnabled, "true", L("Ldap_IsEnabled"), scopes: SettingScopes.Tenant),
                       new SettingDefinition(LdapSettingNames.ContextType, ContextType.Domain.ToString(), L("Ldap_ContextType"), scopes: SettingScopes.Tenant),
                       new SettingDefinition(LdapSettingNames.Container, null, L("Ldap_Container"), scopes: SettingScopes.Tenant),
                       new SettingDefinition(LdapSettingNames.Domain, null, L("Ldap_Domain"), scopes: SettingScopes.Tenant),
                       new SettingDefinition(LdapSettingNames.UserName, null, L("Ldap_UserName"), scopes: SettingScopes.Tenant),
                       new SettingDefinition(LdapSettingNames.Password, null, L("Ldap_Password"), scopes: SettingScopes.Tenant)
                   };
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, AbpZeroConsts.LocalizationSourceName);
        }
    }
}
