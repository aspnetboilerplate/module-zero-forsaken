using Abp.Authorization;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.NHibernate.EntityMappings;

namespace Abp.Zero.NHibernate.EntityMappings
{
    public abstract class PermissionMap<TPermissionSetting> : EntityMap<TPermissionSetting, long>
        where TPermissionSetting : PermissionSetting
    {
        protected PermissionMap()
            : base("AbpPermissions")
        {
            Map(x => x.Name);
            Map(x => x.IsGranted);
            this.MapCreationAudited();
        }
    }

    public class UserPermissionMap : PermissionMap<UserPermissionSetting>
    {
        public UserPermissionMap()
        {
            Map(x => x.UserId);
        }
    }

    public class RolePermissionMap : PermissionMap<RolePermissionSetting>
    {
        public RolePermissionMap()
        {
            Map(x => x.RoleId);
        }
    }
}