using System.DirectoryServices.AccountManagement;
using System.Threading.Tasks;
using Abp.Authorization.Users;
using Abp.Dependency;
using Abp.MultiTenancy;
using Abp.Zero.Ldap.Configuration;
using Abp.Zero.SampleApp.Tests.Users;

namespace Abp.Zero.Ldap.Authentication
{
    public abstract class LdapAuthenticationSource<TTenant, TUser> : DefaultExternalAuthenticationSource<TTenant, TUser>, ITransientDependency
        where TTenant : AbpTenant<TTenant, TUser>
        where TUser : AbpUser<TTenant, TUser>, new()
    {
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

        public override Task<bool> TryAuthenticateAsync(string userNameOrEmailAddress, string plainPassword, TTenant tenant)
        {
            if (!_configuration.IsEnabled)
            {
                return Task.FromResult(false);
            }

            using (var principalContext = CreatePrincipalContext())
            {
                var result = principalContext.ValidateCredentials(userNameOrEmailAddress, plainPassword, ContextOptions.Negotiate);
                return Task.FromResult(result);
            }
        }

        public async override Task<TUser> CreateUserAsync(string userNameOrEmailAddress, TTenant tenant)
        {
            CheckIsEnabled();

            var user = await base.CreateUserAsync(userNameOrEmailAddress, tenant);

            using (var principalContext = CreatePrincipalContext())
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

        public async override Task UpdateUser(TUser user, TTenant tenant)
        {
            await base.UpdateUser(user, tenant);

            using (var principalContext = CreatePrincipalContext())
            {
                var userPrincipal = UserPrincipal.FindByIdentity(principalContext, user.UserName);

                if (userPrincipal == null)
                {
                    throw new AbpException("Unknown LDAP user: " + user.UserName);
                }

                UpdateUserFromPrincipal(user, userPrincipal);
            }
        }

        private static void UpdateUserFromPrincipal(TUser user, UserPrincipal userPrincipal)
        {
            user.UserName = userPrincipal.SamAccountName;
            user.Name = userPrincipal.GivenName;
            user.Surname = userPrincipal.Surname;
            user.EmailAddress = userPrincipal.EmailAddress;
        }

        private void CheckIsEnabled()
        {
            if (!_configuration.IsEnabled)
            {
                throw new AbpException("Ldap Authentication is disabled! You can enable it by setting '" +
                                       LdapSettingNames.IsEnabled + "' to true");
            }
        }

        private PrincipalContext CreatePrincipalContext()
        {
            return new PrincipalContext(
                _configuration.ContextType,
                _configuration.Container,
                _configuration.Domain,
                _configuration.UserName,
                _configuration.Password
                );
        }
    }
}
