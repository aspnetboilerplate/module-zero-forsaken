using System.Collections.Generic;
using Abp.Domain.Repositories;

namespace Abp.Authorization.Roles
{
    public interface IAbpRoleRepository : IRepository<AbpRole>
    {
        List<AbpRole> GetAllListWithPermissions();
    }
}
