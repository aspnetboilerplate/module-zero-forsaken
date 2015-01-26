using System.Linq;
using Abp.Authorization;
using Abp.Localization;
using NSubstitute;

namespace Abp.Tests.Authorization
{
    public static class FakePermissionManagerBuilder
    {
        public class FakePermissionInfo
        {
            public string Name { get; private set; }

            public bool IsGrantedByDefault { get; private set; }

            public FakePermissionInfo(string name, bool isGrantedByDefault = false)
            {
                Name = name;
                IsGrantedByDefault = isGrantedByDefault;
            }
        }

        public static IPermissionManager Build(params FakePermissionInfo[] fakePermissions)
        {
            var permissionManager = Substitute.For<IPermissionManager>();
            var permissions = fakePermissions.Select(fp => new Permission(fp.Name, new FixedLocalizableString(fp.Name), fp.IsGrantedByDefault)).ToList();

            permissionManager.GetAllPermissions().Returns(permissions);
            permissionManager.GetPermission(Arg.Any<string>()).Returns(ci => permissions.Single(p => p.Name == ci.Args()[0].ToString()));
            permissionManager.GetPermissionOrNull(Arg.Any<string>()).Returns(ci => permissions.FirstOrDefault(p => p.Name == ci.Args()[0].ToString()));

            return permissionManager;
        }
    }
}