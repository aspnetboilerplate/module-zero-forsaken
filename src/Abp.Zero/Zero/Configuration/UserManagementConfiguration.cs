using Abp.Configuration;
using Abp.Dependency;

namespace Abp.Zero.Configuration
{
    /// <summary>
    /// Implements <see cref="IUserManagementConfiguration"/> using <see cref="ISettingManager"/>.
    /// </summary>
    public class UserManagementConfiguration : IUserManagementConfiguration, ITransientDependency
    {
        public virtual bool IsEmailConfirmationRequiredForLogin
        {
            get
            {
                return _settingManager.GetSettingValue<bool>(AbpZeroSettingNames.UserManagement.IsEmailConfirmationRequiredForLogin);
            }
        }

        private readonly ISettingManager _settingManager;

        /// <summary>
        /// Creates a new <see cref="UserManagementConfiguration"/>.
        /// </summary>
        /// <param name="settingManager">Setting manager</param>
        public UserManagementConfiguration(ISettingManager settingManager)
        {
            _settingManager = settingManager;
        }
    }
}