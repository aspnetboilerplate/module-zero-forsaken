namespace Abp.Zero.Configuration
{
    internal class AbpZeroConfig : IAbpZeroConfig
    {
        public IRoleManagementConfig RoleManagement
        {
            get { return _roleManagementConfig; }
        }
        private readonly IRoleManagementConfig _roleManagementConfig;

        public AbpZeroConfig(IRoleManagementConfig roleManagementConfig)
        {
            _roleManagementConfig = roleManagementConfig;
        }
    }
}