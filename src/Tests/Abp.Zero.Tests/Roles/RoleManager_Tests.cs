using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Zero.SampleApp.Roles;
using Shouldly;
using Xunit;

namespace Abp.Zero.SampleApp.Tests.Roles
{
    public class RoleManager_Tests : SampleAppTestBase
    {
        private readonly RoleManager _roleManager;
        private readonly IPermissionManager _permissionManager;

        public RoleManager_Tests()
        {
            _roleManager = LocalIocManager.Resolve<RoleManager>();
            _permissionManager = LocalIocManager.Resolve<IPermissionManager>();
        }

        [Fact]
        public async Task Should_Create_And_Retrieve_Role()
        {
            await CreateRole("Role1");

            var role1Retrieved = await _roleManager.FindByNameAsync("Role1");
            role1Retrieved.ShouldNotBe(null);
            role1Retrieved.Name.ShouldBe("Role1");
        }

        [Fact]
        public async Task PermissionTests()
        {
            var role1 = await CreateRole("Role1");

            (await _roleManager.HasPermissionAsync(role1, _permissionManager.GetPermission("Permission1"))).ShouldBe(false);
            (await _roleManager.HasPermissionAsync(role1, _permissionManager.GetPermission("Permission3"))).ShouldBe(true);
            
            await GrantPermission(role1, "Permission1");
            await ProhibitPermission(role1, "Permission1");

            await ProhibitPermission(role1, "Permission3");
            await GrantPermission(role1, "Permission3");

            await GrantPermission(role1, "Permission1");
            await ProhibitPermission(role1, "Permission3");

            var grantedPermissions = await _roleManager.GetGrantedPermissionsAsync(role1);
            grantedPermissions.Count.ShouldBe(2);
            grantedPermissions.ShouldContain(p => p.Name == "Permission1");
            grantedPermissions.ShouldContain(p => p.Name == "Permission4");

            var newPermissionList = new List<Permission>
                                    {
                                        _permissionManager.GetPermission("Permission1"),
                                        _permissionManager.GetPermission("Permission2"),
                                        _permissionManager.GetPermission("Permission3")
                                    };

            await _roleManager.SetGrantedPermissionsAsync(role1, newPermissionList);

            grantedPermissions = await _roleManager.GetGrantedPermissionsAsync(role1);
            
            grantedPermissions.Count.ShouldBe(3);
            grantedPermissions.ShouldContain(p => p.Name == "Permission1");
            grantedPermissions.ShouldContain(p => p.Name == "Permission2");
            grantedPermissions.ShouldContain(p => p.Name == "Permission3");
        }

        private async Task ProhibitPermission(Role role, string permissionName)
        {
            await _roleManager.ProhibitPermissionAsync(role, _permissionManager.GetPermission(permissionName));
            (await _roleManager.HasPermissionAsync(role, _permissionManager.GetPermission(permissionName))).ShouldBe(false);
        }

        private async Task GrantPermission(Role role, string permissionName)
        {
            await _roleManager.GrantPermissionAsync(role, _permissionManager.GetPermission(permissionName));
            (await _roleManager.HasPermissionAsync(role, _permissionManager.GetPermission(permissionName))).ShouldBe(true);
        }

        private async Task<Role> CreateRole(string name)
        {
            var role = new Role(null, name, name);

            (await _roleManager.CreateAsync(role)).Succeeded.ShouldBe(true);
            
            await UsingDbContext(async context =>
            {
                var createdRole = await context.Roles.FirstOrDefaultAsync(r => r.Name == name);
                createdRole.ShouldNotBe(null);
            });

            return role;
        }
    }
}