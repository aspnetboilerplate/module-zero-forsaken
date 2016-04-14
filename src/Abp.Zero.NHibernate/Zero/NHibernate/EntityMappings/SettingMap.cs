using Abp.Configuration;
using Abp.NHibernate.EntityMappings;
using System;

namespace Abp.Zero.NHibernate.EntityMappings
{
    public class SettingMap : EntityMap<Setting, Guid>
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