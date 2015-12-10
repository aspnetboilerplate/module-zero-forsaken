using System.Collections.Generic;
using Abp.Configuration;
using Abp.Localization;

namespace Abp.Zero.Configuration
{
    public class AbpZeroSettingProvider : SettingProvider
    {
        public override IEnumerable<SettingDefinition> GetSettingDefinitions(SettingDefinitionProviderContext context)
        {
            return new List<SettingDefinition>
                   {
                       new SettingDefinition(
                           AbpZeroSettingNames.UserManagement.IsEmailConfirmationRequiredForLogin,
                           "false",
                           new FixedLocalizableString("Is email confirmation required for login."),
                           scopes: SettingScopes.Application | SettingScopes.Tenant,
                           isVisibleToClients: true
                           ),
                       new SettingDefinition(
                           AbpZeroSettingNames.OrganizationUnits.MaxUserMembershipCount,
                           int.MaxValue.ToString(),
                           new FixedLocalizableString("Maximum allowed organization unit membership count for a user."),
                           scopes: SettingScopes.Application | SettingScopes.Tenant,
                           isVisibleToClients: true
                           )
                   };
        }
    }
}
