using Abp.Authorization;
using Abp.NHibernate.Repositories;

namespace Abp.Zero.NHibernate.Repositories
{
    public class PermissionSettingRepository : NhRepositoryBase<PermissionSetting, long>, IPermissionSettingRepository
    {

    }
}