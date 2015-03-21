namespace Abp.Zero.Configuration
{
    internal class UserManagementConfig : IUserManagementConfig
    {
        public bool IsEmailConfirmationRequiredForLogin { get; set; }
    }
}