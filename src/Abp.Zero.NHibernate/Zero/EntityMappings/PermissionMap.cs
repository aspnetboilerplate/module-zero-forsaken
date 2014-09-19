using Abp.Authorization.Permissions;
using Abp.Domain.Entities.Mapping;

namespace Abp.Zero.EntityMappings
{
    public class PermissionMap : EntityMap<PermissionSetting, long>
    {
        public PermissionMap()
            : base("AbpPermissions")
        {
            Map(x => x.RoleId);
            Map(x => x.UserId);
            Map(x => x.Name);
            Map(x => x.IsGranted);
            this.MapCreationAudited();
        }
    }
}