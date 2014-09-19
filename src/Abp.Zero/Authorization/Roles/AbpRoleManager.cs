using Abp.Authorization.Permissions;
using Abp.Dependency;
using Microsoft.AspNet.Identity;

namespace Abp.Authorization.Roles
{
    public class AbpRoleManager : RoleManager<AbpRole, int>, ITransientDependency
    {
        private readonly IPermissionSettingRepository _permissionSettingRepository;
        private readonly IPermissionManager _permissionManager;

        public AbpRoleManager(AbpRoleStore store, IPermissionSettingRepository permissionSettingRepository, IPermissionManager permissionManager)
            : base(store)
        {
            _permissionSettingRepository = permissionSettingRepository;
            _permissionManager = permissionManager;
        }

        public PermissionSetting GetPermissionOrNull(string roleName, string permissionName)
        {
            //TODO: We can define a IRolePermissionStore to manipulate role permissions as identity framework.
            var role = this.FindByName(roleName);
            return _permissionSettingRepository.FirstOrDefault(p => p.RoleId == role.Id);
        }

        public bool HasPermission(int roleId, string permissionName) //TODO: Async
        {
            if (!(Store is IRolePermissionStore))
            {
                throw new AbpException("Store is not IRolePermissionStore");
            }

            var permission = _permissionManager.GetPermissionOrNull(permissionName);
            if (permission == null)
            {
                throw new AbpException("There is no permission with name: " + permissionName);
            }

            var role = this.FindById(roleId);

            var permissionStore = Store as IRolePermissionStore;

            return permission.IsGrantedByDefault
                ? !permissionStore.HasPermissionAsync(role, new PermissionSettingInfo(permissionName, false)).Result
                : permissionStore.HasPermissionAsync(role, new PermissionSettingInfo(permissionName, true)).Result;
        }
    }
}