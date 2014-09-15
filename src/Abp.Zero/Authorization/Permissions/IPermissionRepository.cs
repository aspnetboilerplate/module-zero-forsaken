using Abp.Domain.Repositories;

namespace Abp.Authorization.Permissions
{
    public interface IPermissionRepository : IRepository<Permission, long>
    {

    }
}
