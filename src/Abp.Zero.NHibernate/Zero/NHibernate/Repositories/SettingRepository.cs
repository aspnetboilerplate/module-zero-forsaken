using Abp.Configuration;
using Abp.Domain.Repositories.NHibernate;

namespace Abp.Zero.NHibernate.Repositories
{
    public class SettingRepository : NhRepositoryBase<Setting, long>, ISettingRepository
    {

    }
}