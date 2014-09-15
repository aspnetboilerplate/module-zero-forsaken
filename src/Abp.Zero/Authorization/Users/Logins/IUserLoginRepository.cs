using Abp.Domain.Repositories;

namespace Abp.Authorization.Users.Logins
{
    public interface IUserLoginRepository : IRepository<UserLogin, long>
    {

    }
}