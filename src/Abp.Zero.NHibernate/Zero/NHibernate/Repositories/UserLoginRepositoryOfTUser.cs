using Abp.Authorization.Users;
using Abp.Domain.Repositories.NHibernate;

namespace Abp.Zero.NHibernate.Repositories
{
    public class UserLoginRepository : NhRepositoryBase<UserLogin, long>, IUserLoginRepository
    {

    }
}