using System.Collections.Generic;
using Abp.Authorization.Roles;
using Abp.Domain.Repositories;

namespace Abp.Authorization.Users
{
    public interface IUserRoleRepository : IRepository<UserRole, long>
    {
        List<AbpRole> GetRolesOfUser(int userId);
    }
}