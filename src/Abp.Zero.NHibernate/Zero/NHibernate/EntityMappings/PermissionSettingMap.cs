using Abp.Authorization;
using Abp.NHibernate.EntityMappings;

namespace Abp.Zero.NHibernate.EntityMappings
{
    public abstract class PermissionSettingMap : EntityMap<PermissionSetting, long>
    {
        protected PermissionSettingMap()
            : base("AbpPermissions")
        {
            DiscriminateSubClassesOnColumn("Discriminator");

            Map(x => x.Name);
            Map(x => x.IsGranted);

            this.MapCreationAudited();
        }
    }
}