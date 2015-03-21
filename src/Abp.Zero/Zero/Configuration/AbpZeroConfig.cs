namespace Abp.Zero.Configuration
{
    internal class AbpZeroConfig : IAbpZeroConfig
    {
        public IUserManagementConfig UserManagement
        {
            get { return _userManagementConfig; }
        }
        private readonly IUserManagementConfig _userManagementConfig;

        public AbpZeroConfig(IUserManagementConfig userManagementConfig)
        {
            _userManagementConfig = userManagementConfig;
        }
    }
}