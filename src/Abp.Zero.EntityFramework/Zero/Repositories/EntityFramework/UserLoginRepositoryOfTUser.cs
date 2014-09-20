using Abp.Authorization.Users;
using Abp.Authorization.Users.Logins;

namespace Abp.Zero.Repositories.EntityFramework
{
    public class UserLoginRepository : AbpZeroEfRepositoryBase<UserLogin, long>, IUserLoginRepository
    {

    }
}