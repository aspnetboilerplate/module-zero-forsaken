using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Authorization.Permissions;

namespace Abp.Authorization.Roles
{
    public interface IRolePermissionStore
    {
        Task AddPermissionAsync(AbpRole role, PermissionSettingInfo permissionSetting);

        Task RemovePermissionAsync(AbpRole role, PermissionSettingInfo permissionSetting);

        Task<IList<PermissionSettingInfo>> GetPermissionsAsync(AbpRole role);

        Task<bool> HasPermissionAsync(AbpRole role, PermissionSettingInfo permissionSetting);
    }
}