using Abp.Authorization.Permissions;
using Abp.Dependency;

namespace Abp.Authorization.Roles.Management
{
    public interface IRoleManager : ISingletonDependency
    {
        Permission GetPermissionOrNull(string roleName, string permissionName);
    }
}