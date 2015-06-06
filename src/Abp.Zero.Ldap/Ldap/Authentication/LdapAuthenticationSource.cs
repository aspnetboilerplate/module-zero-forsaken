using System.DirectoryServices.AccountManagement;
using System.Threading.Tasks;
using Abp.Authorization.Users;
using Abp.Dependency;
using Abp.MultiTenancy;
using Abp.Zero.Ldap.Configuration;

namespace Abp.Zero.Ldap.Authentication
{
    /// <summary>
    /// Implements <see cref="IExternalAuthenticationSource{TTenant,TUser}"/> to authenticate users from LDAP.
    /// Extend this class using application's User and Tenant classes as type parameters.
    /// Also, all needed methods can be overridden and changed upon your needs.
    /// </summary>
    /// <typeparam name="TTenant">Tenant type</typeparam>
    /// <typeparam name="TUser">User type</typeparam>
    public abstract class LdapAuthenticationSource<TTenant, TUser> : DefaultExternalAuthenticationSource<TTenant, TUser>, ITransientDependency
        where TTenant : AbpTenant<TTenant, TUser>
        where TUser : AbpUser<TTenant, TUser>, new()
    {
        /// <summary>
        /// LDAP
        /// </summary>
        public const string SourceName = "LDAP";

        public override string Name
        {
            get { return SourceName; }
        }

        private readonly ILdapConfiguration _configuration;

        protected LdapAuthenticationSource(ILdapConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <inheritdoc/>
        public override async Task<bool> TryAuthenticateAsync(string userNameOrEmailAddress, string plainPassword, TTenant tenant)
        {
            if (!(await _configuration.GetIsEnabled(GetIdOrNull(tenant))))
            {
                return false;
            }

            using (var principalContext = await CreatePrincipalContext(tenant))
            {
                return ValidateCredentials(principalContext, userNameOrEmailAddress, plainPassword);
            }
        }

        /// <inheritdoc/>
        public async override Task<TUser> CreateUserAsync(string userNameOrEmailAddress, TTenant tenant)
        {
            await CheckIsEnabled(tenant);

            var user = await base.CreateUserAsync(userNameOrEmailAddress, tenant);

            using (var principalContext = await CreatePrincipalContext(tenant))
            {
                var userPrincipal = UserPrincipal.FindByIdentity(principalContext, userNameOrEmailAddress);

                if (userPrincipal == null)
                {
                    throw new AbpException("Unknown LDAP user: " + userNameOrEmailAddress);
                }

                UpdateUserFromPrincipal(user, userPrincipal);

                user.IsEmailConfirmed = true;
                user.IsActive = true;

                return user;
            }
        }

        public async override Task UpdateUserAsync(TUser user, TTenant tenant)
        {
            await CheckIsEnabled(tenant);

            await base.UpdateUserAsync(user, tenant);

            using (var principalContext = await CreatePrincipalContext(tenant))
            {
                var userPrincipal = UserPrincipal.FindByIdentity(principalContext, user.UserName);

                if (userPrincipal == null)
                {
                    throw new AbpException("Unknown LDAP user: " + user.UserName);
                }

                UpdateUserFromPrincipal(user, userPrincipal);
            }
        }

        protected virtual bool ValidateCredentials(PrincipalContext principalContext, string userNameOrEmailAddress, string plainPassword)
        {
            return principalContext.ValidateCredentials(userNameOrEmailAddress, plainPassword, ContextOptions.Negotiate);
        }

        protected virtual void UpdateUserFromPrincipal(TUser user, UserPrincipal userPrincipal)
        {
            user.UserName = userPrincipal.SamAccountName;
            user.Name = userPrincipal.GivenName;
            user.Surname = userPrincipal.Surname;
            user.EmailAddress = userPrincipal.EmailAddress;
        }

        protected virtual async Task<PrincipalContext> CreatePrincipalContext(TTenant tenant)
        {
            var tenantId = GetIdOrNull(tenant);

            return new PrincipalContext(
                await _configuration.GetContextType(tenantId),
                await _configuration.GetContainer(tenantId),
                await _configuration.GetDomain(tenantId),
                await _configuration.GetUserName(tenantId),
                await _configuration.GetPassword(tenantId)
                );
        }

        private async Task CheckIsEnabled(TTenant tenant)
        {
            if (!await _configuration.GetIsEnabled(GetIdOrNull(tenant)))
            {
                throw new AbpException("Ldap Authentication is disabled! You can enable it by setting '" + LdapSettingNames.IsEnabled + "' to true");
            }
        }

        private static int? GetIdOrNull(TTenant tenant)
        {
            return tenant == null
                ? (int?)null
                : tenant.Id;
        }
    }
}
