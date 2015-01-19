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
    public abstract class AbpRoleStore<TTenant, TRole, TUser> :
        IQueryableRoleStore<TRole, int>,
        IRolePermissionStore<TTenant, TRole, TUser>,
        ITransientDependency
        where TRole : AbpRole<TTenant, TUser>
        where TUser : AbpUser<TTenant, TUser>
        where TTenant : AbpTenant<TTenant, TUser>
    {
        private readonly IRepository<TRole> _roleRepository;
        private readonly IRepository<RolePermissionSetting, long> _rolePermissionSettingRepository;
        private readonly IAbpSession _session;

        /// <summary>
        /// Constructor.
        /// </summary>
        protected AbpRoleStore(
            IRepository<TRole> roleRepository, 
            IRepository<RolePermissionSetting, long> rolePermissionSettingRepository, 
            IAbpSession session) //TODO: Make prop injection?
        {
            _roleRepository = roleRepository;
            _rolePermissionSettingRepository = rolePermissionSettingRepository;
            _session = session;
        }

        public IQueryable<TRole> Roles
        {
            get { return _roleRepository.GetAll(); }
        }

        public async Task CreateAsync(TRole role)
        {
            role.TenantId = _session.TenantId; //TODO: Should set automatically when ABP implements it
            await _roleRepository.InsertAsync(role);
        }

        public async Task UpdateAsync(TRole role)
        {
            await _roleRepository.UpdateAsync(role);
        }

        public async Task DeleteAsync(TRole role)
        {
            await _roleRepository.DeleteAsync(role.Id);
        }

        public async Task<TRole> FindByIdAsync(int roleId)
        {
            return await _roleRepository.FirstOrDefaultAsync(roleId);
        }

        public async Task<TRole> FindByNameAsync(string roleName)
        {
            return await _roleRepository.FirstOrDefaultAsync(
                role =>
                    role.Name == roleName &&
                    role.TenantId == _session.TenantId //TODO: Should filter automatically when ABP implements it
                );
        }

        /// <inheritdoc/>
        public async Task AddPermissionAsync(TRole role, PermissionGrantInfo permissionGrant)
        {
            if (await HasPermissionAsync(role, permissionGrant))
            {
                return;
            }

            await _rolePermissionSettingRepository.InsertAsync(
                new RolePermissionSetting
                {
                    RoleId = role.Id,
                    Name = permissionGrant.Name,
                    IsGranted = permissionGrant.IsGranted
                });
        }

        /// <inheritdoc/>
        public async Task RemovePermissionAsync(TRole role, PermissionGrantInfo permissionGrant)
        {
            await _rolePermissionSettingRepository.DeleteAsync(
                permissionSetting => permissionSetting.RoleId == role.Id &&
                                     permissionSetting.Name == permissionGrant.Name &&
                                     permissionSetting.IsGranted == permissionGrant.IsGranted
                );
        }

        /// <inheritdoc/>
        public async Task<IList<PermissionGrantInfo>> GetPermissionsAsync(TRole role)
        {
            return (await _rolePermissionSettingRepository.GetAllListAsync(p => p.RoleId == role.Id))
                .Select(p => new PermissionGrantInfo(p.Name, p.IsGranted))
                .ToList();
        }

        /// <inheritdoc/>
        public async Task<bool> HasPermissionAsync(TRole role, PermissionGrantInfo permissionGrant)
        {
            return await _rolePermissionSettingRepository.FirstOrDefaultAsync(
                p => p.RoleId == role.Id &&
                     p.Name == permissionGrant.Name &&
                     p.IsGranted == permissionGrant.IsGranted
                ) != null;
        }

        /// <inheritdoc/>
        public async Task RemoveAllPermissionSettingsAsync(TRole role)
        {
            await _rolePermissionSettingRepository.DeleteAsync(s => s.RoleId == role.Id);
        }

        public void Dispose()
        {
            //No need to dispose since using IOC.
        }
    }
}
