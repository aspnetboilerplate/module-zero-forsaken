using System.Threading.Tasks;
using Abp.Authorization.Users;
using Abp.MultiTenancy;

namespace Abp.Zero.SampleApp.Tests.Users
{
    public abstract class DefaultExternalAuthenticationSource<TTenant, TUser> : IExternalAuthenticationSource<TTenant, TUser>
        where TTenant : AbpTenant<TTenant, TUser>
        where TUser : AbpUser<TTenant, TUser>, new()
    {
        public abstract string Name { get; }

        public abstract Task<bool> TryAuthenticateAsync(string userNameOrEmailAddress, string plainPassword, TTenant tenant);

        public virtual Task<TUser> CreateUserAsync(string userNameOrEmailAddress, TTenant tenant)
        {
            return Task.FromResult(
                new TUser
                {
                    UserName = userNameOrEmailAddress,
                    Name = userNameOrEmailAddress,
                    Surname = userNameOrEmailAddress,
                    EmailAddress = userNameOrEmailAddress,
                    IsEmailConfirmed = true,
                    IsActive = true
                });
        }

        public virtual Task UpdateUser(TUser user, TTenant tenant)
        {
            return Task.FromResult(0);
        }
    }
}