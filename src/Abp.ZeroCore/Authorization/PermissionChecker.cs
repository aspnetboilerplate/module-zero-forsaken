﻿using System.Threading.Tasks;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.MultiTenancy;
using Abp.Runtime.Session;
using Castle.Core.Logging;

namespace Abp.Authorization
{
    /// <summary>
    /// Application should inherit this class to implement <see cref="IPermissionChecker"/>.
    /// </summary>
    /// <typeparam name="TTenant"></typeparam>
    /// <typeparam name="TRole"></typeparam>
    /// <typeparam name="TUser"></typeparam>
    public abstract class PermissionChecker<TTenant, TRole, TUser> : IPermissionChecker, ITransientDependency, IIocManagerAccessor
        where TRole : AbpRole<TUser>, new()
        where TUser : AbpUser<TUser>
        where TTenant : AbpTenant<TUser>
    {
        private readonly AbpUserManager<TRole, TUser> _userManager;

        public IIocManager IocManager { get; set; }

        public ILogger Logger { get; set; }

        public IAbpSession AbpSession { get; set; }

        public ICurrentUnitOfWorkProvider CurrentUnitOfWorkProvider { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        protected PermissionChecker(AbpUserManager<TRole, TUser> userManager)
        {
            _userManager = userManager;

            Logger = NullLogger.Instance;
            AbpSession = NullAbpSession.Instance;
        }

        public virtual async Task<bool> IsGrantedAsync(string permissionName)
        {
            return AbpSession.UserId.HasValue && await IsGrantedAsync(AbpSession.UserId.Value, permissionName);
        }

        public virtual async Task<bool> IsGrantedAsync(long userId, string permissionName)
        {
            return await _userManager.IsGrantedAsync(userId, permissionName);
        }

        [UnitOfWork]
        public virtual async Task<bool> IsGrantedAsync(UserIdentifier user, string permissionName)
        {
            if (CurrentUnitOfWorkProvider?.Current == null)
            {
                return await IsGrantedAsync(user.UserId, permissionName);
            }

            using (CurrentUnitOfWorkProvider.Current.SetTenantId(user.TenantId))
            {
                return await IsGrantedAsync(user.UserId, permissionName);
            }
        }
    }
}
