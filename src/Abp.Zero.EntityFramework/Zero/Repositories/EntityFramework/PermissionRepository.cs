using Abp.Authorization.Permissions;

namespace Abp.Zero.Repositories.EntityFramework
{
    public class PermissionRepository : CoreModuleEfRepositoryBase<Permission, long>, IPermissionRepository
    {

    }
}