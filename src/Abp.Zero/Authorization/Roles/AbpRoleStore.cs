using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Authorization.Users;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.MultiTenancy;
using Abp.Runtime.Session;
using Microsoft.AspNet.Identity;

namespace Abp.Authorization.Roles
{
    /// <summary>
    /// Implements 'Role Store' of ASP.NET Identity Framework.
    /// </summary>
    public class AbpRoleStore<TRole, TTenant, TUser> :
        IQueryableRoleStore<TRole, int>,
        IRolePermissionStore<TRole, TTenant, TUser>,
        ITransientDependency
        where TRole : AbpRole<TTenant, TUser>
        where TUser : AbpUser<TTenant, TUser>
        where TTenant : AbpTenant<TTenant, TUser>
    {
        #region Private fields

        private readonly IRepository<TRole> _roleRepository;
        private readonly IRepository<PermissionSetting, long> _permissionSettingRepository;
        private readonly IAbpSession _session;

        #endregion

        #region Constructor

        public AbpRoleStore(IRepository<TRole> roleRepository, IRepository<PermissionSetting, long> permissionSettingRepository, IAbpSession session)
        {
            _roleRepository = roleRepository;
            _permissionSettingRepository = permissionSettingRepository;
            _session = session;
        }

        #endregion

        #region IQueryableRoleStore

        public IQueryable<TRole> Roles
        {
            get { return _roleRepository.GetAll(); }
        }

        public Task CreateAsync(TRole role)
        {
            role.TenantId = _session.TenantId;
            return Task.Factory.StartNew(() => _roleRepository.Insert(role));
        }

        public Task UpdateAsync(TRole role)
        {
            return Task.Factory.StartNew(() => _roleRepository.Update(role));
        }

        public Task DeleteAsync(TRole role)
        {
            return Task.Factory.StartNew(() => _roleRepository.Delete(role.Id));
        }

        public Task<TRole> FindByIdAsync(int roleId)
        {
            return Task.Factory.StartNew(() => _roleRepository.FirstOrDefault(roleId));
        }

        public Task<TRole> FindByNameAsync(string roleName)
        {
            return Task.Factory.StartNew(() => _roleRepository.FirstOrDefault(role => role.Name == roleName && role.TenantId == _session.TenantId)); //TODO: Tenant should be automatically filtered
        }

        #endregion

        #region IRolePermissionStore

        public Task AddPermissionAsync(TRole role, PermissionGrantInfo permissionGrant)
        {
            return Task.Factory.StartNew(() =>
                                         {
                                             if (HasPermissionAsync(role, permissionGrant).Result) //TODO: async/await?
                                             {
                                                 return;
                                             }

                                             _permissionSettingRepository.Insert(
                                                 new PermissionSetting
                                                 {
                                                     RoleId = role.Id,
                                                     Name = permissionGrant.Name,
                                                     IsGranted = permissionGrant.IsGranted
                                                 });
                                         });
        }

        public Task RemovePermissionAsync(TRole role, PermissionGrantInfo permissionGrant)
        {
            return Task.Factory.StartNew(() =>
                                         {
                                             var permissionEntity = _permissionSettingRepository.FirstOrDefault(
                                                 p => p.RoleId == role.Id &&
                                                      p.Name == permissionGrant.Name &&
                                                      p.IsGranted == permissionGrant.IsGranted);
                                             if (permissionEntity == null)
                                             {
                                                 return;
                                             }

                                             _permissionSettingRepository.Delete(permissionEntity);
                                         });
        }

        public Task<IList<PermissionGrantInfo>> GetPermissionsAsync(TRole role)
        {
            return Task.Factory.StartNew<IList<PermissionGrantInfo>>(
                () => _permissionSettingRepository
                    .GetAllList(p => p.RoleId == role.Id)
                    .Select(p => new PermissionGrantInfo(p.Name, p.IsGranted))
                    .ToList()
                );
        }

        public Task<bool> HasPermissionAsync(TRole role, PermissionGrantInfo permissionGrant)
        {
            return Task.Factory.StartNew(() => _permissionSettingRepository.FirstOrDefault(p => p.RoleId == role.Id && p.Name == permissionGrant.Name && p.IsGranted == permissionGrant.IsGranted) != null);
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
