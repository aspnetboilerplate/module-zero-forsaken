using Abp.Authorization.Users;
using Abp.Domain.Repositories.NHibernate;

namespace Abp.Zero.NHibernate.Repositories
{
    public class AbpUserRepository : NhRepositoryBase<AbpUser, long>, IAbpUserRepository
    {
        public void UpdatePassword(long userId, string password)
        {
            var user = Load(userId);
            user.Password = password; //TODO: Test
        }

        public void UpdateEmail(long userId, string emailAddress)
        {
            var user = Load(userId);
            user.EmailAddress = emailAddress; //TODO: Test
        }

        public void UpdateIsEmailConfirmed(long userId, bool confirmed)
        {
            var user = Load(userId);
            user.IsEmailConfirmed = confirmed; //TODO: Test
        }
    }
}