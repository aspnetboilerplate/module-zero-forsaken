using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Abp.Authorization.Roles;
using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.MultiTenancy;
using Abp.Runtime.Security;
using Abp.Runtime.Session;
using Abp.Zero.Configuration;
using Microsoft.AspNet.Identity;

namespace Abp.Authorization.Users
{
    /// <summary>
    /// Extends <see cref="UserManager{TUser,TKey}"/> of ASP.NET Identity Framework.
    /// </summary>
    public abstract class AbpUserManager<TTenant, TRole, TUser> : UserManager<TUser, long>, ITransientDependency
        where TTenant : AbpTenant<TTenant, TUser>
        where TRole : AbpRole<TTenant, TUser>, new() where TUser : AbpUser<TTenant, TUser>
    {
        private IUserPermissionStore<TTenant, TUser> UserPermissionStore
        {
            get
            {
                if (!(Store is IUserPermissionStore<TTenant, TUser>))
                {
                    throw new AbpException("Store is not IUserPermissionStore");
                }

                return Store as IUserPermissionStore<TTenant, TUser>;
            }
        }

        public IUserManagementConfig UserManagementConfig //TODO: Add to constructor?
        {
            private get
            {
                if (_userManagementConfig == null)
                {
                    throw new AbpException("Should set UserManagementConfig before use it!");
                }

                return _userManagementConfig;
            }
            set { _userManagementConfig = value; }
        }
        private IUserManagementConfig _userManagementConfig;

        public IAbpSession AbpSession { get; set; }

        private readonly IPermissionManager _permissionManager;
        private readonly AbpRoleManager<TTenant, TRole, TUser> _roleManager;
        private readonly IRepository<TTenant> _tenantRepository;
        private readonly IMultiTenancyConfig _multiTenancyConfig;
        private readonly AbpUserStore<TTenant, TRole, TUser> _abpUserStore;

        //TODO: Non-generic parameters may be converted to property-injection
        protected AbpUserManager(
            AbpUserStore<TTenant, TRole, TUser> userStore,
            AbpRoleManager<TTenant, TRole, TUser> roleManager,
            IRepository<TTenant> tenantRepository,
            IMultiTenancyConfig multiTenancyConfig, 
            IPermissionManager permissionManager)
            : base(userStore)
        {
            _abpUserStore = userStore;
            _roleManager = roleManager;
            _tenantRepository = tenantRepository;
            _multiTenancyConfig = multiTenancyConfig;
            _permissionManager = permissionManager;
        }

        public override async Task<IdentityResult> CreateAsync(TUser user)
        {
            if (AbpSession.TenantId.HasValue)
            {
                user.TenantId = AbpSession.TenantId.Value;
            }

            return await base.CreateAsync(user);
        }
        
        /// <summary>
        /// Check whether a user is granted for a permission.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="permissionName">Permission name</param>
        public virtual async Task<bool> IsGrantedAsync(long userId, string permissionName)
        {
            return await IsGrantedAsync(
                await GetUserByIdAsync(userId),
                _permissionManager.GetPermission(permissionName)
                );
        }

        /// <summary>
        /// Check whether a user is granted for a permission.
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="permission">Permission</param>
        public virtual async Task<bool> IsGrantedAsync(TUser user, Permission permission)
        {
            //Check for multi-tenancy side
            if (!permission.MultiTenancySides.HasFlag(AbpSession.MultiTenancySide))
            {
                return false;
            }

            //Check for user-specific value
            if (await UserPermissionStore.HasPermissionAsync(user, new PermissionGrantInfo(permission.Name, true)))
            {
                return true;
            }

            if (await UserPermissionStore.HasPermissionAsync(user, new PermissionGrantInfo(permission.Name, false)))
            {
                return false;
            }

            //Check for roles
            var roles = await GetRolesAsync(user.Id);
            if (!roles.Any())
            {
                return permission.IsGrantedByDefault;
            }

            foreach (var role in roles)
            {
                if (await _roleManager.HasPermissionAsync(role, permission.Name))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Gets granted permissions for a user.
        /// </summary>
        /// <param name="user">Role</param>
        /// <returns>List of granted permissions</returns>
        public virtual async Task<IReadOnlyList<Permission>> GetGrantedPermissionsAsync(TUser user)
        {
            var permissionList = new List<Permission>();

            foreach (var permission in _permissionManager.GetAllPermissions())
            {
                if (await IsGrantedAsync(user, permission))
                {
                    permissionList.Add(permission);
                }
            }

            return permissionList;
        }
        
        /// <summary>
        /// Sets all granted permissions of a user at once.
        /// Prohibits all other permissions.
        /// </summary>
        /// <param name="user">The user</param>
        /// <param name="permissions">Permissions</param>
        public virtual async Task SetGrantedPermissionsAsync(TUser user, IEnumerable<Permission> permissions)
        {
            var oldPermissions = await GetGrantedPermissionsAsync(user);
            var newPermissions = permissions.ToArray();

            foreach (var permission in oldPermissions.Where(p => !newPermissions.Contains(p)))
            {
                await ProhibitPermissionAsync(user, permission);
            }

            foreach (var permission in newPermissions.Where(p => !oldPermissions.Contains(p)))
            {
                await GrantPermissionAsync(user, permission);
            }
        }

        /// <summary>
        /// Prohibits all permissions for a user.
        /// </summary>
        /// <param name="user">User</param>
        public async Task ProhibitAllPermissionsAsync(TUser user)
        {
            foreach (var permission in _permissionManager.GetAllPermissions())
            {
                await ProhibitPermissionAsync(user, permission);
            }
        }

        /// <summary>
        /// Resets all permission settings for a user.
        /// It removes all permission settings for the user.
        /// User will have permissions according to his roles.
        /// This method does not prohibit all permissions.
        /// For that, use <see cref="ProhibitAllPermissionsAsync"/>.
        /// </summary>
        /// <param name="user">User</param>
        public async Task ResetAllPermissionsAsync(TUser user)
        {
            await UserPermissionStore.RemoveAllPermissionSettingsAsync(user);
        }

        /// <summary>
        /// Grants a permission for a user if not already granted.
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="permission">Permission</param>
        public virtual async Task GrantPermissionAsync(TUser user, Permission permission)
        {
            await UserPermissionStore.RemovePermissionAsync(user, new PermissionGrantInfo(permission.Name, false));                

            if (await IsGrantedAsync(user, permission))
            {
                return;
            }

            await UserPermissionStore.AddPermissionAsync(user, new PermissionGrantInfo(permission.Name, true));
        }

        /// <summary>
        /// Prohibits a permission for a user if it's granted.
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="permission">Permission</param>
        public virtual async Task ProhibitPermissionAsync(TUser user, Permission permission)
        {
            await UserPermissionStore.RemovePermissionAsync(user, new PermissionGrantInfo(permission.Name, true));

            if (!await IsGrantedAsync(user, permission))
            {
                return;
            }

            await UserPermissionStore.AddPermissionAsync(user, new PermissionGrantInfo(permission.Name, false));
        }

        public virtual async Task<TUser> FindByNameOrEmailAsync(string userNameOrEmailAddress)
        {
            return await _abpUserStore.FindByNameOrEmailAsync(userNameOrEmailAddress);
        }

        public virtual async Task<AbpLoginResult> LoginAsync(string userNameOrEmailAddress, string plainPassword, string tenancyName = null)
        {
            //TODO: Disable tenancy filter?

            if (userNameOrEmailAddress.IsNullOrEmpty())
            {
                throw new ArgumentNullException("userNameOrEmailAddress");
            }

            if (plainPassword.IsNullOrEmpty())
            {
                throw new ArgumentNullException("plainPassword");                
            }

            TUser user;

            if (!_multiTenancyConfig.IsEnabled)
            {
                //Log in with default denant
                user = await FindByNameOrEmailAsync(userNameOrEmailAddress);
                if (user == null)
                {
                    return new AbpLoginResult(AbpLoginResultType.InvalidUserNameOrEmailAddress);
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(tenancyName))
                {
                    //Log in as tenancy owner user
                    user = await _abpUserStore.FindByNameOrEmailAsync(null, userNameOrEmailAddress);
                }
                else
                {
                    //Log in as tenant user
                    var tenant = await _tenantRepository.FirstOrDefaultAsync(t => t.TenancyName == tenancyName);
                    if (tenant == null)
                    {
                        return new AbpLoginResult(AbpLoginResultType.InvalidTenancyName);
                    }

                    if (!tenant.IsActive)
                    {
                        return new AbpLoginResult(AbpLoginResultType.TenantIsNotActive);
                    }

                    user = await _abpUserStore.FindByNameOrEmailAsync(tenant.Id, userNameOrEmailAddress);
                }
                
                if (user == null)
                {
                    return new AbpLoginResult(AbpLoginResultType.InvalidUserNameOrEmailAddress);
                }
            }

            var verificationResult = new PasswordHasher().VerifyHashedPassword(user.Password, plainPassword);
            if (verificationResult != PasswordVerificationResult.Success)
            {
                return new AbpLoginResult(AbpLoginResultType.InvalidPassword);
            }

            if (!user.IsActive)
            {
                return new AbpLoginResult(AbpLoginResultType.UserIsNotActive);
            }

            if (UserManagementConfig.IsEmailConfirmationRequiredForLogin && !user.IsEmailConfirmed)
            {
                return new AbpLoginResult(AbpLoginResultType.UserEmailIsNotConfirmed);
            }

            user.LastLoginTime = DateTime.Now;

            await Store.UpdateAsync(user);

            return new AbpLoginResult(user, await CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie));
        }

        /// <summary>
        /// Gets a user by given id.
        /// Throws exception if no user found with given id.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <returns>User</returns>
        /// <exception cref="AbpException">Throws exception if no user found with given id</exception>
        public virtual async Task<TUser> GetUserByIdAsync(long userId)
        {
            var user = await FindByIdAsync(userId);
            if (user == null)
            {
                throw new AbpException("There is no user with id: " + userId);
            }

            return user;
        }

        public async override Task<ClaimsIdentity> CreateIdentityAsync(TUser user, string authenticationType)
        {
            var identity = await base.CreateIdentityAsync(user, authenticationType);
            if (user.TenantId.HasValue)
            {
                identity.AddClaim(new Claim(AbpClaimTypes.TenantId, user.TenantId.Value.ToString(CultureInfo.InvariantCulture)));
            }

            return identity;
        }

        public class AbpLoginResult
        {
            public AbpLoginResultType Result { get; private set; }

            public TUser User { get; private set; }

            public ClaimsIdentity Identity { get; private set; }

            public AbpLoginResult(AbpLoginResultType result)
            {
                Result = result;
            }

            public AbpLoginResult(TUser user, ClaimsIdentity identity)
                :this(AbpLoginResultType.Success)
            {
                User = user;
                Identity = identity;
            }
        }
    }
}