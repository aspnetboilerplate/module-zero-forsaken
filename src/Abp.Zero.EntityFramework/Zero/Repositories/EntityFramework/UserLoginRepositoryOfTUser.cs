using Abp.Authorization.Users;

namespace Abp.Zero.Repositories.EntityFramework
{
    public class UserLoginRepository : CoreModuleEfRepositoryBase<UserLogin, long>, IUserLoginRepository
    {

    }
}