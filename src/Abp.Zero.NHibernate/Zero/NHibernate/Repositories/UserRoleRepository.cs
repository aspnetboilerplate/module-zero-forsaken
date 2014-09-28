using Abp.Authorization.Users;
using Abp.Domain.Repositories.NHibernate;

namespace Abp.Zero.NHibernate.Repositories
{
    public class UserRoleRepository : NhRepositoryBase<UserRole, long>, IUserRoleRepository
    {

    }
}