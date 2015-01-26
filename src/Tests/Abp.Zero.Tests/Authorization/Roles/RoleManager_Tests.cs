using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Tests._TestBasis;
using Castle.MicroKernel.Registration;
using Shouldly;
using Xunit;

namespace Abp.Tests.Authorization.Roles
{
    public class RoleManager_Tests : TestBase
    {
        private readonly TestRoleManager _roleManager;
        private readonly IPermissionManager _permissionManager;

        public RoleManager_Tests()
        {
            _permissionManager = FakePermissionManagerBuilder.Build(
                new FakePermissionManagerBuilder.FakePermissionInfo("Permission1"),
                new FakePermissionManagerBuilder.FakePermissionInfo("Permission2"),
                new FakePermissionManagerBuilder.FakePermissionInfo("Permission3", true),
                new FakePermissionManagerBuilder.FakePermissionInfo("Permission4", true)
                );

            LocalIocManager.IocContainer.Register(
                Component.For<IPermissionManager>().UsingFactoryMethod(() => _permissionManager).LifestyleSingleton()
                );

            _roleManager = LocalIocManager.Resolve<TestRoleManager>();
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
            await DbContext.SaveChangesAsync();

            grantedPermissions = await _roleManager.GetGrantedPermissionsAsync(role1);
            
            grantedPermissions.Count.ShouldBe(3);
            grantedPermissions.ShouldContain(p => p.Name == "Permission1");
            grantedPermissions.ShouldContain(p => p.Name == "Permission2");
            grantedPermissions.ShouldContain(p => p.Name == "Permission3");
        }

        private async Task ProhibitPermission(TestRole role, string permissionName)
        {
            await _roleManager.ProhibitPermissionAsync(role, _permissionManager.GetPermission(permissionName));
            await DbContext.SaveChangesAsync();
            (await _roleManager.HasPermissionAsync(role, _permissionManager.GetPermission(permissionName))).ShouldBe(false);
        }

        private async Task GrantPermission(TestRole role, string permissionName)
        {
            await _roleManager.GrantPermissionAsync(role, _permissionManager.GetPermission(permissionName));
            await DbContext.SaveChangesAsync();
            (await _roleManager.HasPermissionAsync(role, _permissionManager.GetPermission(permissionName))).ShouldBe(true);
        }

        private async Task<TestRole> CreateRole(string name)
        {
            var role = new TestRole(null, name, name);

            (await _roleManager.CreateAsync(role)).Succeeded.ShouldBe(true);
            DbContext.SaveChanges();

            var createdRole = await DbContext.Roles.FirstOrDefaultAsync(r => r.Name == name);
            createdRole.ShouldNotBe(null);

            return role;
        }
    }
}