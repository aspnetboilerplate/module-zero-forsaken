using Abp.Authorization;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.NHibernate.EntityMappings;

namespace Abp.Zero.NHibernate.EntityMappings
{
    public abstract class PermissionSettingMap<TPermissionSetting> : EntityMap<TPermissionSetting, long>
        where TPermissionSetting : PermissionSetting
    {
        protected PermissionSettingMap()
            : base("AbpPermissions")
        {
            Map(x => x.Name);
            Map(x => x.IsGranted);
            this.MapCreationAudited();
        }
    }

    public class UserPermissionSettingMap : PermissionSettingMap<UserPermissionSetting>
    {
        public UserPermissionSettingMap()
        {
            Map(x => x.UserId);
        }
    }

    public class RolePermissionSettingMap : PermissionSettingMap<RolePermissionSetting>
    {
        public RolePermissionSettingMap()
        {
            Map(x => x.RoleId);
        }
    }
}