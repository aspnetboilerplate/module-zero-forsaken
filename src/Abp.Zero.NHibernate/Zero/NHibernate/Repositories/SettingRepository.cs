using Abp.Configuration;
using Abp.NHibernate.Repositories;

namespace Abp.Zero.NHibernate.Repositories
{
    public class SettingRepository : NhRepositoryBase<Setting, long>, ISettingRepository
    {

    }
}