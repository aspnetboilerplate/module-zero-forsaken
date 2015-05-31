using System.Threading.Tasks;
using Abp.Authorization.Users;
using Abp.MultiTenancy;

namespace Abp.Zero.SampleApp.Tests.Users
{
    public interface IExternalAuthenticationSource<TTenant, TUser>
        where TTenant : AbpTenant<TTenant, TUser>
        where TUser : AbpUser<TTenant, TUser>
    {
        string Name { get; }

        Task<bool> TryAuthenticateAsync(string userNameOrEmailAddress, string plainPassword, TTenant tenant);

        Task<TUser> CreateUserAsync(string userNameOrEmailAddress, TTenant tenant);

        Task UpdateUser(TUser user, TTenant tenant);
    }
}