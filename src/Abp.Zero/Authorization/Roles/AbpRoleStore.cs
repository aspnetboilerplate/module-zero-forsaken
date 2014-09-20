using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Authorization.Permissions;
using Abp.Dependency;
using Abp.Runtime.Session;
using Microsoft.AspNet.Identity;

namespace Abp.Authorization.Roles
{
    public class AbpRoleStore :
        IQueryableRoleStore<AbpRole, int>,
        IRolePermissionStore,
        ITransientDependency
    {
        #region Private fields

        private readonly IAbpRoleRepository _roleRepository;
        private readonly IPermissionSettingRepository _permissionSettingRepository;
        private readonly IAbpSession _session;

        #endregion

        #region Constructor

        public AbpRoleStore(IAbpRoleRepository roleRepository, IPermissionSettingRepository permissionSettingRepository, IAbpSession session)
        {
            _roleRepository = roleRepository;
            _permissionSettingRepository = permissionSettingRepository;
            _session = session;
        }

        #endregion

        #region IQueryableRoleStore

        public IQueryable<AbpRole> Roles
        {
            get { return _roleRepository.GetAll(); }
        }

        public Task CreateAsync(AbpRole role)
        {
            role.TenantId = _session.TenantId;
            return Task.Factory.StartNew(() => _roleRepository.Insert(role));
        }

        public Task UpdateAsync(AbpRole role)
        {
            return Task.Factory.StartNew(() => _roleRepository.Update(role));
        }

        public Task DeleteAsync(AbpRole role)
        {
            return Task.Factory.StartNew(() => _roleRepository.Delete(role.Id));
        }

        public Task<AbpRole> FindByIdAsync(int roleId)
        {
            return Task.Factory.StartNew(() => _roleRepository.FirstOrDefault(roleId));
        }

        public Task<AbpRole> FindByNameAsync(string roleName)
        {
            return Task.Factory.StartNew(() => _roleRepository.FirstOrDefault(role => role.Name == roleName && role.TenantId == _session.TenantId));
        }

        #endregion

        #region IRolePermissionStore

        public Task AddPermissionAsync(AbpRole role, PermissionSettingInfo permissionSetting)
        {
            return Task.Factory.StartNew(() =>
                                         {
                                             if (HasPermissionAsync(role, permissionSetting).Result) //TODO: async/await?
                                             {
                                                 return;
                                             }

                                             _permissionSettingRepository.Insert(
                                                 new PermissionSetting
                                                 {
                                                     RoleId = role.Id,
                                                     Name = permissionSetting.Name,
                                                     IsGranted = permissionSetting.IsGranted
                                                 });
                                         });
        }

        public Task RemovePermissionAsync(AbpRole role, PermissionSettingInfo permissionSetting)
        {
            return Task.Factory.StartNew(() =>
                                         {
                                             var permissionEntity = _permissionSettingRepository.FirstOrDefault(
                                                 p => p.RoleId == role.Id &&
                                                      p.Name == permissionSetting.Name &&
                                                      p.IsGranted == permissionSetting.IsGranted);
                                             if (permissionEntity == null)
                                             {
                                                 return;
                                             }

                                             _permissionSettingRepository.Delete(permissionEntity);
                                         });
        }

        public Task<IList<PermissionSettingInfo>> GetPermissionsAsync(AbpRole role)
        {
            return Task.Factory.StartNew<IList<PermissionSettingInfo>>(
                () => _permissionSettingRepository
                    .GetAllList(p => p.RoleId == role.Id)
                    .Select(p => new PermissionSettingInfo(p.Name, p.IsGranted))
                    .ToList()
                );
        }

        public Task<bool> HasPermissionAsync(AbpRole role, PermissionSettingInfo permissionSetting)
        {
            return Task.Factory.StartNew(() => _permissionSettingRepository.FirstOrDefault(p => p.RoleId == role.Id && p.Name == permissionSetting.Name && p.IsGranted == permissionSetting.IsGranted) != null);
        }

        #endregion
        
        #region IDisposable

        public void Dispose()
        {
            //No need to dispose since using dependency injection manager
        }

        #endregion
    }
}
