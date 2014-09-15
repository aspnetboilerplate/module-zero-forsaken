using Abp.Domain.Repositories;

namespace Abp.Authorization.Users
{
    public interface IUserLoginRepository : IRepository<UserLogin, long>
    {

    }
}