using Abp.Authorization.Permissions;
using Abp.Domain.Repositories.NHibernate;

namespace Abp.Zero.Repositories.NHibernate
{
    public class PermissionSettingRepository : NhRepositoryBase<PermissionSetting, long>, IPermissionSettingRepository
    {

    }
}