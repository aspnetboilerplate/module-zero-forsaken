using Abp.Authorization.Permissions;
using Abp.Authorization.Roles;
using Microsoft.AspNet.Identity;

namespace Abp.Runtime.Security.IdentityFramework
{
    public class AbpRoleManager : RoleManager<AbpRole, int>
    {
        private readonly IPermissionRepository _permissionRepository;

        public AbpRoleManager(AbpRoleStore store, IPermissionRepository permissionRepository) : base(store)
        {
            _permissionRepository = permissionRepository;
        }

        public Permission GetPermissionOrNull(string roleName, string permissionName)
        {
            var role = this.FindByName(roleName);
            return _permissionRepository.FirstOrDefault(p => p.RoleId == role.Id);
        }
    }
}