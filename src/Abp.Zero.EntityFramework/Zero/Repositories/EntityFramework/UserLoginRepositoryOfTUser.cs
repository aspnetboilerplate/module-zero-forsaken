using Abp.Authorization.Users;

namespace Abp.Zero.Repositories.EntityFramework
{
    public class UserLoginRepository : AbpZeroEfRepositoryBase<UserLogin, long>, IUserLoginRepository
    {

    }
}