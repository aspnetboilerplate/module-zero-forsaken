using System;
using System.Globalization;
using System.Security.Claims;
using System.Threading.Tasks;
using Abp.Authorization.Roles;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.MultiTenancy;
using Abp.Runtime.Security;
using Abp.Zero.Configuration;
using Microsoft.AspNet.Identity;

namespace Abp.Authorization.Users
{
    /// <summary>
    /// Extends <see cref="UserManager{TRole,TKey}"/> of ASP.NET Identity Framework.
    /// </summary>
    public abstract class AbpUserManager<TTenant, TRole, TUser> : UserManager<TUser, long>, ITransientDependency
        where TTenant : AbpTenant<TTenant, TUser>
        where TRole : AbpRole<TTenant, TUser>
        where TUser : AbpUser<TTenant, TUser>
    {
        private readonly AbpRoleManager<TTenant, TRole, TUser> _roleManager;
        private readonly IRepository<TTenant> _tenantRepository;
        private readonly MultiTenancyConfig _multiTenancyConfig;
        private readonly AbpUserStore<TTenant, TRole, TUser> _abpUserStore;

        protected AbpUserManager(
            AbpUserStore<TTenant, TRole, TUser> userStore,
            AbpRoleManager<TTenant, TRole, TUser> roleManager,
            IRepository<TTenant> tenantRepository,
            MultiTenancyConfig multiTenancyConfig)
            : base(userStore)
        {
            _abpUserStore = userStore;
            _roleManager = roleManager;
            _tenantRepository = tenantRepository;
            _multiTenancyConfig = multiTenancyConfig;
        }

        /// <summary>
        /// Check whether a user is granted for a permission.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="permissionName">Permission name</param>
        public async Task<bool> IsGrantedAsync(long userId, string permissionName)
        {
            foreach (var role in await GetRolesAsync(userId))
            {
                if (await _roleManager.HasPermissionAsync(role, permissionName))
                {
                    return true;
                }
            }

            return false;
        }

        public async Task<TUser> FindByNameOrEmailAsync(string userNameOrEmailAddress)
        {
            return await _abpUserStore.FindByNameOrEmailAsync(userNameOrEmailAddress);
        }

        public async Task<AbpLoginResult> LoginAsync(string userNameOrEmailAddress, string plainPassword, string tenancyName = null)
        {
            //TODO: Email confirmation check? (optional)

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

            user.LastLoginTime = DateTime.Now;

            await Store.UpdateAsync(user);

            var identity = await CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            if (user.TenantId.HasValue)
            {
                identity.AddClaim(new Claim(AbpClaimTypes.TenantId, user.TenantId.Value.ToString(CultureInfo.InvariantCulture)));
            }

            return new AbpLoginResult(user, identity);
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