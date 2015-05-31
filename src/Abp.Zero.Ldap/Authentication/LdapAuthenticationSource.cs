using System.DirectoryServices.AccountManagement;
using System.Threading.Tasks;
using Abp.Authorization.Users;
using Abp.Dependency;
using Abp.MultiTenancy;
using Abp.Zero.SampleApp.Tests.Users;

namespace Abp.Zero.Ldap.Authentication
{
    public abstract class LdapAuthenticationSource<TTenant, TUser> : DefaultExternalAuthenticationSource<TTenant, TUser>, ITransientDependency
        where TTenant : AbpTenant<TTenant, TUser>
        where TUser : AbpUser<TTenant, TUser>, new()
    {
        public override string Name
        {
            get { return "LDAP"; }
        }

        private readonly ILdapConfiguration _configuration;

        protected LdapAuthenticationSource(ILdapConfiguration configuration)
        {
            _configuration = configuration;
        }

        public override Task<bool> TryAuthenticateAsync(string userNameOrEmailAddress, string plainPassword, TTenant tenant)
        {
            using (var principalContext = CreatePrincipalContext())
            {
                var result = principalContext.ValidateCredentials(userNameOrEmailAddress, plainPassword, ContextOptions.Negotiate);
                return Task.FromResult(result);
            }
        }

        public override Task<TUser> CreateUserAsync(string userNameOrEmailAddress, TTenant tenant)
        {
            using (var principalContext = CreatePrincipalContext())
            {
                var userPrincipal = UserPrincipal.FindByIdentity(principalContext, userNameOrEmailAddress);

                if (userPrincipal == null)
                {
                    throw new AbpException("Unknown LDAP user: " + userNameOrEmailAddress);
                }

                return Task.FromResult(
                    new TUser
                    {
                        UserName = userPrincipal.SamAccountName,
                        Name = userPrincipal.GivenName,
                        Surname = userPrincipal.Surname,
                        EmailAddress = userPrincipal.EmailAddress,
                        IsEmailConfirmed = true,
                        IsActive = true
                    });
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
