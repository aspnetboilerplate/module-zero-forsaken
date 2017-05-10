using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Abp.Authorization.Users;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Zero;
using Castle.Core.Logging;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;

namespace Abp.Authorization.Roles
{
    /// <summary>
    /// Creates a new instance of a persistence store for roles.
    /// </summary>
    public abstract class AbpRoleStore<TRole, TUser> :
        IRoleStore<TRole>,
        IRoleClaimStore<TRole>,
        IRolePermissionStore<TRole>,
        IQueryableRoleStore<TRole>,
        ITransientDependency

        where TRole : AbpRole<TUser>
        where TUser : AbpUser<TUser>
    {
        public ILogger Logger { get; set; }
        
        /// <summary>
        /// Gets or sets the <see cref="IdentityErrorDescriber"/> for any error that occurred with the current operation.
        /// </summary>
        public IdentityErrorDescriber ErrorDescriber { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating if changes should be persisted after CreateAsync, UpdateAsync and DeleteAsync are called.
        /// </summary>
        /// <value>
        /// True if changes should be automatically persisted, otherwise false.
        /// </value>
        public bool AutoSaveChanges { get; set; } = true;

        public IQueryable<TRole> Roles => _roleRepository.GetAll();

        private readonly IRepository<TRole> _roleRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IRepository<RolePermissionSetting, long> _rolePermissionSettingRepository;

        protected AbpRoleStore(
            IUnitOfWorkManager unitOfWorkManager,
            IRepository<TRole> roleRepository, 
            IRepository<RolePermissionSetting, long> rolePermissionSettingRepository)
        {
            _unitOfWorkManager = unitOfWorkManager;
            _roleRepository = roleRepository;
            _rolePermissionSettingRepository = rolePermissionSettingRepository;

            ErrorDescriber = new IdentityErrorDescriber();
            Logger = NullLogger.Instance;
        }

        /// <summary>Saves the current store.</summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
        protected Task SaveChanges(CancellationToken cancellationToken)
        {
            if (!AutoSaveChanges || _unitOfWorkManager.Current == null)
            {
                return Task.CompletedTask;
            }

            return _unitOfWorkManager.Current.SaveChangesAsync();
        }

        /// <summary>
        /// Creates a new role in a store as an asynchronous operation.
        /// </summary>
        /// <param name="role">The role to create in the store.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="Task{TResult}"/> that represents the <see cref="IdentityResult"/> of the asynchronous query.</returns>
        public virtual async Task<IdentityResult> CreateAsync([NotNull] TRole role, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            Check.NotNull(role, nameof(role));

            await _roleRepository.InsertAsync(role);
            await SaveChanges(cancellationToken);

            return IdentityResult.Success;
        }

        /// <summary>
        /// Updates a role in a store as an asynchronous operation.
        /// </summary>
        /// <param name="role">The role to update in the store.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="Task{TResult}"/> that represents the <see cref="IdentityResult"/> of the asynchronous query.</returns>
        public virtual async Task<IdentityResult> UpdateAsync([NotNull] TRole role, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            Check.NotNull(role, nameof(role));

            role.ConcurrencyStamp = Guid.NewGuid().ToString();
            await _roleRepository.UpdateAsync(role);

            try
            {
                await SaveChanges(cancellationToken);
            }
            catch (AbpDbConcurrencyException ex)
            {
                Logger.Warn(ex.ToString(), ex);
                return IdentityResult.Failed(ErrorDescriber.ConcurrencyFailure());
            }

            await SaveChanges(cancellationToken);

            return IdentityResult.Success;
        }

        /// <summary>
        /// Deletes a role from the store as an asynchronous operation.
        /// </summary>
        /// <param name="role">The role to delete from the store.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="Task{TResult}"/> that represents the <see cref="IdentityResult"/> of the asynchronous query.</returns>
        public virtual async Task<IdentityResult> DeleteAsync([NotNull] TRole role, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            Check.NotNull(role, nameof(role));

            await _roleRepository.DeleteAsync(role);

            try
            {
                await SaveChanges(cancellationToken);
            }
            catch (AbpDbConcurrencyException ex)
            {
                Logger.Warn(ex.ToString(), ex);
                return IdentityResult.Failed(ErrorDescriber.ConcurrencyFailure());
            }

            await SaveChanges(cancellationToken);
            
            return IdentityResult.Success;
        }

        /// <summary>
        /// Gets the ID for a role from the store as an asynchronous operation.
        /// </summary>
        /// <param name="role">The role whose ID should be returned.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="Task{TResult}"/> that contains the ID of the role.</returns>
        public Task<string> GetRoleIdAsync([NotNull] TRole role, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            Check.NotNull(role, nameof(role));

            return Task.FromResult(role.Id.ToString());
        }

        /// <summary>
        /// Gets the name of a role from the store as an asynchronous operation.
        /// </summary>
        /// <param name="role">The role whose name should be returned.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="Task{TResult}"/> that contains the name of the role.</returns>
        public Task<string> GetRoleNameAsync([NotNull] TRole role, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            Check.NotNull(role, nameof(role));

            return Task.FromResult(role.Name);
        }

        /// <summary>
        /// Sets the name of a role in the store as an asynchronous operation.
        /// </summary>
        /// <param name="role">The role whose name should be set.</param>
        /// <param name="roleName">The name of the role.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
        public Task SetRoleNameAsync([NotNull] TRole role, string roleName, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            Check.NotNull(role, nameof(role));

            role.Name = roleName;
            return Task.CompletedTask;
        }

        /// <summary>
        /// Finds the role who has the specified ID as an asynchronous operation.
        /// </summary>
        /// <param name="id">The role ID to look for.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="Task{TResult}"/> that result of the look up.</returns>
        public virtual Task<TRole> FindByIdAsync(string id, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            return _roleRepository.GetAsync(id.To<int>());
        }

        /// <summary>
        /// Finds the role who has the specified normalized name as an asynchronous operation.
        /// </summary>
        /// <param name="normalizedName">The normalized role name to look for.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="Task{TResult}"/> that result of the look up.</returns>
        public virtual Task<TRole> FindByNameAsync([NotNull] string normalizedName, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            Check.NotNull(normalizedName, nameof(normalizedName));

            return _roleRepository.FirstOrDefaultAsync(r => r.NormalizedName == normalizedName);
        }

        /// <summary>
        /// Get a role's normalized name as an asynchronous operation.
        /// </summary>
        /// <param name="role">The role whose normalized name should be retrieved.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="Task{TResult}"/> that contains the name of the role.</returns>
        public virtual Task<string> GetNormalizedRoleNameAsync([NotNull] TRole role, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            Check.NotNull(role, nameof(role));

            return Task.FromResult(role.NormalizedName);
        }

        /// <summary>
        /// Set a role's normalized name as an asynchronous operation.
        /// </summary>
        /// <param name="role">The role whose normalized name should be set.</param>
        /// <param name="normalizedName">The normalized name to set</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
        public virtual Task SetNormalizedRoleNameAsync([NotNull] TRole role, string normalizedName, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            Check.NotNull(role, nameof(role));

            role.NormalizedName = normalizedName;

            return Task.CompletedTask;
        }

        /// <summary>
        /// Dispose the stores
        /// </summary>
        public void Dispose()
        {
        }

        /// <summary>
        /// Get the claims associated with the specified <paramref name="role"/> as an asynchronous operation.
        /// </summary>
        /// <param name="role">The role whose claims should be retrieved.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="Task{TResult}"/> that contains the claims granted to a role.</returns>
        public virtual async Task<IList<Claim>> GetClaimsAsync([NotNull] TRole role, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            Check.NotNull(role, nameof(role));

            await _roleRepository.EnsureLoadedAsync(role, u => u.Claims, cancellationToken);

            return role.Claims.Select(c => new Claim(c.ClaimType, c.ClaimValue)).ToList();
        }

        /// <summary>
        /// Adds the <paramref name="claim"/> given to the specified <paramref name="role"/>.
        /// </summary>
        /// <param name="role">The role to add the claim to.</param>
        /// <param name="claim">The claim to add to the role.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
        public async Task AddClaimAsync([NotNull] TRole role, [NotNull] Claim claim, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            Check.NotNull(role, nameof(role));
            Check.NotNull(claim, nameof(claim));

            await _roleRepository.EnsureLoadedAsync(role, u => u.Claims, cancellationToken);

            role.Claims.Add(new RoleClaim(role, claim));
        }

        /// <summary>
        /// Removes the <paramref name="claim"/> given from the specified <paramref name="role"/>.
        /// </summary>
        /// <param name="role">The role to remove the claim from.</param>
        /// <param name="claim">The claim to remove from the role.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
        public async Task RemoveClaimAsync([NotNull] TRole role, [NotNull] Claim claim, CancellationToken cancellationToken = default(CancellationToken))
        {
            Check.NotNull(role, nameof(role));
            Check.NotNull(claim, nameof(claim));

            await _roleRepository.EnsureLoadedAsync(role, u => u.Claims, cancellationToken);

            role.Claims.RemoveAll(c => c.ClaimValue == claim.Value && c.ClaimType == claim.Type);
        }

        public virtual async Task<TRole> FindByDisplayNameAsync(string displayName)
        {
            return await _roleRepository.FirstOrDefaultAsync(
                role => role.DisplayName == displayName
                );
        }

        public virtual async Task AddPermissionAsync(TRole role, PermissionGrantInfo permissionGrant)
        {
            if (await HasPermissionAsync(role.Id, permissionGrant))
            {
                return;
            }

            await _rolePermissionSettingRepository.InsertAsync(
                new RolePermissionSetting
                {
                    TenantId = role.TenantId,
                    RoleId = role.Id,
                    Name = permissionGrant.Name,
                    IsGranted = permissionGrant.IsGranted
                });
        }

        /// <inheritdoc/>
        public virtual async Task RemovePermissionAsync(TRole role, PermissionGrantInfo permissionGrant)
        {
            await _rolePermissionSettingRepository.DeleteAsync(
                permissionSetting => permissionSetting.RoleId == role.Id &&
                                     permissionSetting.Name == permissionGrant.Name &&
                                     permissionSetting.IsGranted == permissionGrant.IsGranted
                );
        }

        /// <inheritdoc/>
        public virtual Task<IList<PermissionGrantInfo>> GetPermissionsAsync(TRole role)
        {
            return GetPermissionsAsync(role.Id);
        }

        public async Task<IList<PermissionGrantInfo>> GetPermissionsAsync(int roleId)
        {
            return (await _rolePermissionSettingRepository.GetAllListAsync(p => p.RoleId == roleId))
                .Select(p => new PermissionGrantInfo(p.Name, p.IsGranted))
                .ToList();
        }

        /// <inheritdoc/>
        public virtual async Task<bool> HasPermissionAsync(int roleId, PermissionGrantInfo permissionGrant)
        {
            return await _rolePermissionSettingRepository.FirstOrDefaultAsync(
                p => p.RoleId == roleId &&
                     p.Name == permissionGrant.Name &&
                     p.IsGranted == permissionGrant.IsGranted
                ) != null;
        }

        /// <inheritdoc/>
        public virtual async Task RemoveAllPermissionSettingsAsync(TRole role)
        {
            await _rolePermissionSettingRepository.DeleteAsync(s => s.RoleId == role.Id);
        }
    }
}
