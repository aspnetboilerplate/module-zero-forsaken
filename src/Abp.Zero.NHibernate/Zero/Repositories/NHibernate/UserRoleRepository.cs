using Abp.Authorization.Users;
using Abp.Domain.Repositories.NHibernate;

namespace Abp.Zero.Repositories.NHibernate
{
    public class UserRoleRepository : NhRepositoryBase<UserRole, long>, IUserRoleRepository
    {

    }
}