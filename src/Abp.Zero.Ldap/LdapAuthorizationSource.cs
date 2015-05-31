using System.Threading.Tasks;
using Abp.Authorization.Users;
using Abp.MultiTenancy;
using Abp.Zero.SampleApp.Tests.Users;

namespace Abp.Zero.Ldap
{
    public class LdapAuthorizationSource<TTenant, TUser> : DefaultExternalAuthenticationSource<TTenant, TUser>
        where TTenant : AbpTenant<TTenant, TUser>
        where TUser : AbpUser<TTenant, TUser>, new()
    {
        public override string Name
        {
            get { return "LDAP"; }
        }

        public override Task<bool> TryAuthenticateAsync(string userNameOrEmailAddress, string plainPassword, TTenant tenant)
        {
            return Task.FromResult(false);
        }
    }
}
