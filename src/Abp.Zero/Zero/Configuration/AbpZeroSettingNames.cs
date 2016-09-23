namespace Abp.Zero.Configuration
{
    public static class AbpZeroSettingNames
    {
        public static class UserManagement
        {
            /// <summary>
            /// "Abp.Zero.UserManagement.IsEmailConfirmationRequiredForLogin".
            /// </summary>
            public const string IsEmailConfirmationRequiredForLogin = "Abp.Zero.UserManagement.IsEmailConfirmationRequiredForLogin";

            public static class TwoFactorLogin
            {
                /// <summary>
                /// "Abp.Zero.UserManagement.TwoFactorLogin.IsEnabled".
                /// </summary>
                public const string IsEnabled = "Abp.Zero.UserManagement.TwoFactorLogin.IsEnabled";

                /// <summary>
                /// "Abp.Zero.UserManagement.TwoFactorLogin.IsEmailProviderEnabled".
                /// </summary>
                public const string IsEmailProviderEnabled = "Abp.Zero.UserManagement.TwoFactorLogin.IsEmailProviderEnabled";

                /// <summary>
                /// "Abp.Zero.UserManagement.TwoFactorLogin.IsSmsProviderEnabled".
                /// </summary>
                public const string IsSmsProviderEnabled = "Abp.Zero.UserManagement.TwoFactorLogin.IsSmsProviderEnabled";

                /// <summary>
                /// "Abp.Zero.UserManagement.TwoFactorLogin.IsRememberBrowserEnabled".
                /// </summary>
                public const string IsRememberBrowserEnabled = "Abp.Zero.UserManagement.TwoFactorLogin.IsRememberBrowserEnabled";
            }
        }

        public static class OrganizationUnits
        {
            /// <summary>
            /// "Abp.Zero.OrganizationUnits.MaxUserMembershipCount".
            /// </summary>
            public const string MaxUserMembershipCount = "Abp.Zero.OrganizationUnits.MaxUserMembershipCount";
        }
    }
}