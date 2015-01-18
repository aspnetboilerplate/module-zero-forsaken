using System.Collections.Generic;
using System.Linq;
using Abp.Authorization;
using Abp.Localization;
using Abp.Tests._TestBasis;
using Castle.MicroKernel.Registration;
using Microsoft.AspNet.Identity;
using NSubstitute;
using Shouldly;
using Xunit;

namespace Abp.Tests.Authorization.Users
{
    public class RoleManager_Tests : TestBase
    {
        private readonly TestRoleManager _roleManager;

        public RoleManager_Tests()
        {
            var permissionManager = Substitute.For<IPermissionManager>();

            //permissionManager.GetAllPermissions().Returns((ci) => new List<Permission>
            //                                                      {
            //                                                          new Permission("Permission-1", new FixedLocalizableString("Permission-1"))
            //                                                      });

            LocalIocManager.IocContainer.Register(
                Component.For<IPermissionManager>().UsingFactoryMethod(() => permissionManager).LifestyleSingleton()
                );

            _roleManager = LocalIocManager.Resolve<TestRoleManager>();
        }

        [Fact]
        public void Should_Create_Role()
        {
            _roleManager.Create(new TestRole(null, "Role1", "Role one")).Succeeded.ShouldBe(true);
            DbContext.SaveChanges();

            DbContext.Roles.FirstOrDefault(r => r.Name == "Role1").ShouldNotBe(null);
        }

        [Fact]
        public void Should_Set_Permissions()
        {
            _roleManager.Create(new TestRole(null, "Role1", "Role one")).Succeeded.ShouldBe(true);
            DbContext.SaveChanges();

            DbContext.Roles.FirstOrDefault(r => r.Name == "Role1").ShouldNotBe(null);

            
        }
    }
}