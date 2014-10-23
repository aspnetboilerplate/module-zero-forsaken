namespace Abp.Authorization.Users
{
    public class UserPermissionSetting : PermissionSetting
    {
        public virtual long UserId { get; set; }
    }
}