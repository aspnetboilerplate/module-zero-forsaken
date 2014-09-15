using Abp.Authorization.Users;
using Abp.Authorization.Users.Logins;
using Abp.Domain.Repositories.NHibernate;

namespace Abp.Zero.Repositories.NHibernate
{
    public class UserLoginRepository : NhRepositoryBase<UserLogin, long>, IUserLoginRepository
    {

    }
}