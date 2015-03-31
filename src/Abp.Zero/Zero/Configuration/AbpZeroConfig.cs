namespace Abp.Zero.Configuration
{
    internal class AbpZeroConfig : IAbpZeroConfig
    {
        public IUserManagementConfig UserManagement
        {
            get { return _userManagementConfig; }
        }
        private readonly IUserManagementConfig _userManagementConfig;

        public IRoleManagementConfig RoleManagement
        {
            get { return _roleManagementConfig; }
        }
        private readonly IRoleManagementConfig _roleManagementConfig;

        public AbpZeroConfig(IUserManagementConfig userManagementConfig, IRoleManagementConfig roleManagementConfig)
        {
            _userManagementConfig = userManagementConfig;
            _roleManagementConfig = roleManagementConfig;
        }
    }
}