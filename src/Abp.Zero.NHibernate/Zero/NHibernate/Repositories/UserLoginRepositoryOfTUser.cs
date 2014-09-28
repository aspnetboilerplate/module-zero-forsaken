using Abp.Authorization.Users;
using Abp.NHibernate.Repositories;

namespace Abp.Zero.NHibernate.Repositories
{
    public class UserLoginRepository : NhRepositoryBase<UserLogin, long>, IUserLoginRepository
    {

    }
}