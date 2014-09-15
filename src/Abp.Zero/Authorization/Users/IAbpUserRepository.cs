using Abp.Domain.Repositories;

namespace Abp.Authorization.Users
{
    public interface IAbpUserRepository : IRepository<AbpUser, long>
    {
        void UpdatePassword(long userId, string password);

        void UpdateEmail(long userId, string emailAddress);

        void UpdateIsEmailConfirmed(long userId, bool confirmed);
    }
}