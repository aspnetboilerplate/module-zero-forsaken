using Abp.Authorization.Users;
using Abp.Domain.Repositories.NHibernate;

namespace Abp.Zero.Repositories.NHibernate
{
    public class UserLoginRepository : NhRepositoryBase<UserLogin, long>, IUserLoginRepository
    {

    }
}