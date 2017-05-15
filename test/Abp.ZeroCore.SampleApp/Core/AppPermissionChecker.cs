using Abp.Authorization;

namespace Abp.ZeroCore.SampleApp.Core
{
    public class AppPermissionChecker : PermissionChecker<Tenant, Role, User>
    {
        public AppPermissionChecker(UserManager userManager)
            : base(userManager)
        {
        }
    }
}
