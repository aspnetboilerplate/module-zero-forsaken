using Abp.Authorization;
using Abp.Domain.Repositories.NHibernate;

namespace Abp.Zero.NHibernate.Repositories
{
    public class PermissionSettingRepository : NhRepositoryBase<PermissionSetting, long>, IPermissionSettingRepository
    {

    }
}