using Abp.Authorization.Permissions;
using Abp.Dependency;
using Microsoft.AspNet.Identity;

namespace Abp.Authorization.Roles
{
    public class AbpRoleManager : RoleManager<AbpRole, int>, ITransientDependency
    {
        private readonly IPermissionRepository _permissionRepository;

        public AbpRoleManager(AbpRoleStore store, IPermissionRepository permissionRepository)
            : base(store)
        {
            _permissionRepository = permissionRepository;
        }

        public Permission GetPermissionOrNull(string roleName, string permissionName)
        {
            //TODO: We can define a IRolePermissionStore to manipulate role permissions as identity framework.
            var role = this.FindByName(roleName);
            return _permissionRepository.FirstOrDefault(p => p.RoleId == role.Id);
        }
    }
}