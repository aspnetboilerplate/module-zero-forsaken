using Abp.Configuration;
using Abp.Domain.Entities.Mapping;

namespace Abp.Zero.NHibernate.EntityMappings
{
    public class SettingMap : EntityMap<Setting, long>
    {
        public SettingMap()
            : base("AbpSettings")
        {
            Map(x => x.TenantId);
            Map(x => x.UserId);
            Map(x => x.Name);
            Map(x => x.Value);

            this.MapAudited();
        }
    }
}