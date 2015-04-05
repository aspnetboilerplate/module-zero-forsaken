namespace Abp.Zero.Configuration
{
    /// <summary>
    /// User manager configuration
    /// </summary>
    public interface IUserManagementConfiguration
    {
        /// <summary>
        /// Is email confirmation required for login?
        /// </summary>
        bool IsEmailConfirmationRequiredForLogin { get; }
    }
}