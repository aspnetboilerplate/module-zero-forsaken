namespace Abp.Zero.Configuration
{
    /// <summary>
    /// User manager configuration
    /// </summary>
    public interface IUserManagementConfig
    {
        /// <summary>
        /// Is email confirmation required for login?
        /// Default: false.
        /// </summary>
        bool IsEmailConfirmationRequiredForLogin { get; set; }
    }
}