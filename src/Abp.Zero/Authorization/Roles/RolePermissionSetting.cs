namespace Abp.Authorization.Roles
{
    public class RolePermissionSetting : PermissionSetting
    {
        public virtual int RoleId { get; set; }
    }
}