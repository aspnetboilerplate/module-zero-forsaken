using Abp.Authorization.Users;
using Abp.Authorization.Users.Logins;

namespace Abp.Zero.Repositories.EntityFramework
{
    public class UserLoginRepository : CoreModuleEfRepositoryBase<UserLogin, long>, IUserLoginRepository
    {

    }
}