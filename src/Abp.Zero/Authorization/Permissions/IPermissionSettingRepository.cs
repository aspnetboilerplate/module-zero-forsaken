using Abp.Domain.Repositories;

namespace Abp.Authorization.Permissions
{
    public interface IPermissionSettingRepository : IRepository<PermissionSetting, long>
    {

    }
}
