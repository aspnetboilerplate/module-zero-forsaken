namespace Abp.Authorization.Permissions
{
    public class PermissionSettingInfo
    {
        public string Name { get; set; }

        public bool IsGranted { get; set; }

        public PermissionSettingInfo()
        {
            
        }

        public PermissionSettingInfo(string name, bool isGranted)
        {
            Name = name;
            IsGranted = isGranted;
        }
    }
}