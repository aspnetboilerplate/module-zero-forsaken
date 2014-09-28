using Abp.Authorization.Users;
using Abp.NHibernate.Repositories;

namespace Abp.Zero.NHibernate.Repositories
{
    public class UserRoleRepository : NhRepositoryBase<UserRole, long>, IUserRoleRepository
    {

    }
}